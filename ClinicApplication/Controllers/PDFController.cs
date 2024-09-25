using Microsoft.AspNetCore.Mvc;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ClinicApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ClinicApplication.Controllers
{
    [Route("PDF/[action]")]
    public class PDFController : Controller
    {
        private readonly IConverter _converter;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IServiceProvider _serviceProvider;
        // 初始化 DBContext
        private readonly ApplicationDbContext _context;

        public PDFController(IConverter converter, IRazorViewEngine razorViewEngine, IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            _converter = converter;
            _razorViewEngine = razorViewEngine;
            _serviceProvider = serviceProvider;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Method to render view to string
        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);
                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"View {viewName} not found");
                }

                // Create a new TempDataDictionary
                var tempData = new TempDataDictionary(actionContext.HttpContext, _serviceProvider.GetService<ITempDataProvider>());

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model },
                    tempData,  // Use the manually created TempDataDictionary
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }


        [HttpGet]
        public async Task<IActionResult> GeneratePdf(string sno)
        {
            try
            {
                var query = from a in _context.TestDOC
                            join b in _context.TestDetail on a.SNO equals b.SNO into ab
                            from b in ab.DefaultIfEmpty()
                            join c in _context.Items on b.ItemID equals c.ID into bc
                            from c in bc.DefaultIfEmpty()
                            join d in _context.Customer on a.CustID equals d.Id into ad
                            from d in ad.DefaultIfEmpty()
                            where a.SNO == sno
                            orderby b.SetID, b.SubID
                            select new InspectionViewModel
                            {
                                SNO = a.SNO,
                                InspectionSerialNo = a.OrderNo,
                                CustID = a.CustID,
                                Name = a.SubName,
                                DateOfBirth = a.SubBirthDay,
                                Age = a.SubAge,
                                IDNumber = a.SubIDNO,
                                Gender = a.SubGender,
                                Reporter = a.Examiner,
                                Reviewer = a.Reviewers,
                                IsSent = a.IsPass,
                                SpecimenStatus = a.SpecimenConditions,
                                AuditDate = a.AuditDay,
                                AuditTime = a.AuditTime,
                                ReceiptDate = a.RecDate,
                                InspectionItemUniqueID = b.ItemID,
                                CategoryOrder = b.SetID,
                                InspectionItem = b.Name,
                                Result = b.Result,
                                Interpretation = b.Interpretation,
                                ItemOrder = b.SubID,
                                ChineseName = c.C_Name,
                                BiologicalReferenceRange = c.STD,
                                Unit = c.Unit,
                                Remarks = c.F_Memo,
                                NHI_ID = c.NHI_ID,
                                SubmittingUnit = d.Name,
                                Phone = d.Tel1,
                                Fax = d.Fax,
                                Address = d.Address,
                                SubmittingUnitRemarks = d.UnitMeno,
                                InstitutionCode = d.AgencyCode
                            };

                List<InspectionViewModel> result = query.ToList();
                string htmlContent = @"
<html>
	<head>
		.header {
			border-bottom: 2px solid #ccc;
			padding-bottom: 10px;
		}

		.footer {
			margin-top: 20px;
			padding-top: 10px;
			border-top: 2px solid #ccc;
			text-align: center;
		}

		.section-title {
			background-color: #f8f9fa;
			padding: 10px;
			margin-bottom: 20px;
			font-weight: bold;
		}

		.patient-details, .diagnosis, .treatment-plan, .hematology-results {
			margin-bottom: 30px;
		}

		.signature {
			margin-top: 50px;
			text-align: right;
		}

			.signature img {
				width: 200px;
				d
	</head>
	<body>
		<div class='container'>
			<!-- Header Section -->
			<div class='header text-center'>
				<h1 style='color: dodgerblue'>現代醫事檢驗所</h1>
				<p>檢驗報告單</p>
				<p>電話: (06)-633-2100 | 地址: 台南市新營區中山路236號</p>
			</div>

			<!-- Patient Details Section -->
			<div class='patient-details'>
				<h3 class='section-title'>受檢者資訊</h3>
				<div class='row'>
					<div class='col-md-6' style='font-size: 18px'>
						<strong>姓名:</strong> John Doe<br>
						<strong>生日:</strong> 01/01/1980<br>
						<strong>年齡:</strong> 44<br>
						<strong>身分證字號:</strong> A123456789<br>
						<strong>性別:</strong> Male<br>
					</div>
					<div class='col-md-6' style='font-size: 18px'>
						<strong>簽收日期:</strong> 01/01/2024<br>
						<strong style='color: blue'>檢驗序號:</strong> <strong style='color: blue'>123456789</strong>
					</div>
				</div>
			</div>

			<!-- Hematology Results Section -->
			<div class='hematology-results'>
				<h3 class='section-title d-flex justify-content-between align-items-center'>
					<span>檢驗項目，共檢測 10 項</span>
					<button type='button' class='btn btn-warning'>
						<i class='fas fa-file-pdf'></i> 列印報告
					</button>
				</h3>
				<table class='table table-bordered table-striped'>
					<thead class='table-light'>
						<tr>
							<th>項次</th>
							<th>檢驗項目名稱</th>
							<th>檢驗結果</th>
							<th>判讀</th>
							<th>生物參考區間</th>
							<th>單位</th>
							<th>報告說明</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>1</td>
							<td>項目名稱1</td>
							<td>結果1</td>
							<td>判讀1</td>
							<td>區間1</td>
							<td>單位1</td>
							<td>說明1</td>
						</tr>
						<!-- More rows as needed -->
					</tbody>
				</table>
			</div>

			<!-- Treatment Plan Section -->
			<div class='treatment-plan'>
				<h3 class='section-title'>送檢單位</h3>
				<ul>
					<li><strong>送檢單位:</strong> Example Unit</li>
					<li><strong>電話:</strong> (06)-123-4567</li>
					<li><strong>地址:</strong> 台南市某某區某某路</li>
					<li><strong>機構代碼:</strong> 123456</li>
				</ul>
			</div>

			<!-- Signature Section -->
			<div class='signature'>
				<p>報告者: Dr. Smith</p>
				<p>覆核者: Dr. Johnson</p>
				<p>覆核時間: 01/01/2024 10:00 AM</p>
				<img style='display: inline-block' src='/images/companyStamp.png' alt='Inspection Organization Signature'>
			</div>

			<!-- Footer Section -->
			<div class='footer'>
				<p>Copyright © 2024 現代醫事檢驗所. 保留一切權利。</p>
			</div>
		</div>
	</body>
</html>";


                var pdfDocument = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                    Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = htmlContent,
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                        FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Clinic Report" }
                    }
                }
                };

                var pdf = _converter.Convert(pdfDocument);
                return File(pdf, "application/pdf", "ClinicReport.pdf");
            }
            catch (Exception ex)
            {
                return File("application/pdf", "ClinicReport.pdf");
            }
        }
    }
}
