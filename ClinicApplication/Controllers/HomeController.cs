using ClinicApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace ClinicApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // 初始化 DBContext
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;

                return RedirectToAction("ClinicHome", "Home", new { customerId = claimValue });
            }
            else
            {
                // Return the login view if the user is not logged in
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult WaitPublish()
        {
            return View();
        }

        // 進入組織首頁
        [Authorize]
		public IActionResult ClinicHome(string customerId)
		{
			try
			{
				// 0. 取得診所資料
				var customer = _context.Customer.Find(customerId);
				// 0.1 當天日期
				DateTime currentDate = DateTime.Now.Date;
                DateTime endOfDay = currentDate.Date.AddDays(1).AddTicks(-1);

                // 1. 先從資料庫拉出這間診所的紀錄
                List<TestDOC> tempResult = _context.TestDOC
					.Where(doc => doc.CustID == customerId)
					.Select(td => new TestDOC
					{
						SNO = td.SNO,
						SubName = td.SubName,
						OrderNo = td.OrderNo,
						IsPass = td.IsPass,
						RecDate = td.RecDate
					})
					.ToList();
				// 2. 再過濾期間內的資料
				List<TestDOC> finalResult = tempResult.Where(doc =>
				{
					// Check if RecDate is null or empty
					if (string.IsNullOrEmpty(doc.RecDate))
					{
						return false; // Skip null or empty RecDate values
					}

					// Split RecDate by '/'
					var dateParts = doc.RecDate.Split('/');

					// Ensure the RecDate is in the correct format (i.e., it has three parts)
					if (dateParts.Length != 3)
					{
						return false; // Skip improperly formatted dates
					}

					// Convert year to Gregorian by adding 1911 and subtracting 1
					int minguoYear = int.Parse(dateParts[0]);
					int gregorianYear = minguoYear + 1911;

					// Construct the DateTime
					DateTime recDate;
					try
					{
						recDate = new DateTime(gregorianYear, int.Parse(dateParts[1]), int.Parse(dateParts[2]));
					}
					catch
					{
						return false; // Skip dates that can't be converted to DateTime
					}

					return recDate >= currentDate && recDate <= endOfDay;
				})
				.Select(td => new TestDOC
				{
					SNO = td.SNO,
					SubName = td.SubName,
					OrderNo = td.OrderNo,
					IsPass = td.IsPass,
					RecDate = td.RecDate
				}).ToList();


				ClinicHomeViewModel model = new ClinicHomeViewModel
				{
					customer = customer,
					TestDOCList = finalResult
				};

				return View(model);
			}
			catch (Exception ex)
			{
				// Handle the exception here
				// You can log the exception or display an error message
				// For example:
				ModelState.AddModelError("", ex.Message);
				return View();
			}
		}

        // 診所重新給條件來取得列表
        [HttpPost]
        public IActionResult GetClinicTestDoscList([FromBody] ClinicTestDocRequest param)
        {
            try
            {
                // 0. 取得診所資料
                var customerId = param.CustID; // Declare the 'customerId' variable
                var customer = _context.Customer.Find(customerId);
                // 0.1 當天日期
                DateTime beginDate = DateTime.Now.Date;
                DateTime endDate = DateTime.Now.Date;
                if (param.BeginDate != null)
                {
                    beginDate = (DateTime)param.BeginDate;
                }
                if (param.EndDate != null)
                {
                    endDate = ((DateTime)param.EndDate).AddDays(1).AddTicks(-1);
                }

                // 1. 先從資料庫拉出這間診所的紀錄
                List<TestDOC> tempResult = _context.TestDOC
                    .Where(doc => doc.CustID == customerId && (doc.SubName.Contains(param.Keywords) || doc.SNO.Contains(param.Keywords) || doc.OrderNo.Contains(param.Keywords)))
                    .Select(td => new TestDOC
                    {
                        SNO = td.SNO,
                        SubName = td.SubName,
                        OrderNo = td.OrderNo,
                        IsPass = td.IsPass,
                        RecDate = td.RecDate
                    })
                    .ToList();
                // 2. 再過濾期間內的資料
                List<TestDOC> finalResult = tempResult.Where(doc =>
                {
                    // Check if RecDate is null or empty
                    if (string.IsNullOrEmpty(doc.RecDate))
                    {
                        return false; // Skip null or empty RecDate values
                    }

                    // Split RecDate by '/'
                    var dateParts = doc.RecDate.Split('/');

                    // Ensure the RecDate is in the correct format (i.e., it has three parts)
                    if (dateParts.Length != 3)
                    {
                        return false; // Skip improperly formatted dates
                    }

                    // Convert year to Gregorian by adding 1911 and subtracting 1
                    int minguoYear = int.Parse(dateParts[0]);
                    int gregorianYear = minguoYear + 1911;

                    // Construct the DateTime
                    DateTime recDate;
                    try
                    {
                        recDate = new DateTime(gregorianYear, int.Parse(dateParts[1]), int.Parse(dateParts[2]));
                    }
                    catch
                    {
                        return false; // Skip dates that can't be converted to DateTime
                    }

                    return recDate >= beginDate && recDate <= endDate;
                })
                .Select(td => new TestDOC
                {
                    SNO = td.SNO,
                    SubName = td.SubName,
                    OrderNo = td.OrderNo,
                    IsPass = td.IsPass,
                    RecDate = td.RecDate
                }).ToList();

                var response = new
                {
                    IsSuccess = true,
                    Message = "",
                    Data = finalResult
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = ""
                };
                return Json(response);
            }
        }

        // 進入詳細畫面
        [Authorize]
        public IActionResult ClinicDetails(string id)
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
                            where a.SNO == id
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
                return View(result);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
