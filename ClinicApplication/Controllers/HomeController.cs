using ClinicApplication.Models;
using ClinicApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
// For upload Excel by ERPLUS library
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
// For upload Excel by NOPI
using System;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // For .xlsx
using NPOI.HSSF.UserModel; // For .xls


namespace ClinicApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        // 初始化 DBContext(固定資料庫)
        //private readonly ApplicationDbContext _context;
        // 初始化 DB Service(動態資料庫)
        private readonly ClinicService _clinicService;

        public HomeController(ILogger<HomeController> logger, ClinicService clinicService)
        {
            _logger = logger;
            _clinicService = clinicService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claimValue = User.FindFirst(ClaimTypes.Name)?.Value;
                var connectionName = User.FindFirst(ClaimTypes.Role)?.Value;

                return RedirectToAction("ClinicHome", "Home", new { customerId = claimValue, connectionName = connectionName });
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

        public IActionResult ExcelImport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["Error"] = "Please upload a valid Excel file.";
                return View("ExcelImport");
            }

            try
            {
                // Define the path to save the uploaded file temporarily
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserUpload");

                // Ensure the directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name to avoid conflicts
                var filePath = Path.Combine(uploadsFolder, Guid.NewGuid().ToString() + Path.GetExtension(excelFile.FileName));

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    excelFile.CopyTo(fileStream);
                }

                // Extract column names from the Excel file
                List<string> columnNames = new List<string>();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                IWorkbook workbook;
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (filePath.EndsWith(".xlsx"))
                    {
                        workbook = new XSSFWorkbook(fileStream); // For .xlsx
                    }
                    else if (filePath.EndsWith(".xls"))
                    {
                        workbook = new HSSFWorkbook(fileStream); // For .xls
                    }
                    else
                    {
                        TempData["Error"] = "Please upload a valid format Excel file.";
                        return View("ExcelImport");
                    }
                }

                ISheet sheet = workbook.GetSheetAt(0); // Get the first sheet
                List<ExcelExportLog> excelExportLogList = new List<ExcelExportLog>();

                // Start from the second row (skip header)
                for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    IRow row = sheet.GetRow(rowIndex);
                    if (row != null)
                    {
                        DateTime parsedVisitDate;
                        String rawVisitDate = row.GetCell(7)?.ToString() ?? "";
                        if (DateTime.TryParseExact(rawVisitDate, "yyyMMdd", null, System.Globalization.DateTimeStyles.None, out parsedVisitDate))
                        {
                            // 如果看診日可以轉換成日期,則繼續執行
                            var excelExportLogItem = new ExcelExportLog
                            {
                                DiagnosisType = row.GetCell(0)?.ToString() ?? "",
                                PatientName = row.GetCell(1)?.ToString() ?? "",
                                Gender = row.GetCell(2)?.ToString() ?? "",
                                IdNumber = row.GetCell(3)?.ToString() ?? "",
                                BirthDate = row.GetCell(4)?.ToString() ?? "",
                                Department = row.GetCell(5)?.ToString() ?? "",
                                PrescribingDoctor = row.GetCell(6)?.ToString() ?? "",
                                ConsultationDate = row.GetCell(7)?.ToString() ?? "",
                                MedicalRecordNumber = row.GetCell(8)?.ToString() ?? "",
                                SerialNumber = row.GetCell(9)?.ToString() ?? "",
                                TestCode = row.GetCell(10)?.ToString() ?? "",
                                SubitemCode = row.GetCell(11)?.ToString() ?? "",
                                SubitemName = row.GetCell(12)?.ToString() ?? "",
                                TransmissionCode = row.GetCell(13)?.ToString() ?? "",
                                SamplingDate = row.GetCell(14)?.ToString() ?? "",
                                Reporter = row.GetCell(15)?.ToString() ?? "",
                                GroupName = row.GetCell(16)?.ToString() ?? "",
                                MachineNumber = row.GetCell(17)?.ToString() ?? "",
                                OrderSerialNumber = row.GetCell(18)?.ToString() ?? "",
                                TestName = row.GetCell(19)?.ToString() ?? "",
                                SpecimenQuantity = row.GetCell(20)?.ToString() ?? "",
                                ExternalDeliveryCode = row.GetCell(21)?.ToString() ?? "",
                                ExternalDeliveryName = row.GetCell(22)?.ToString() ?? "",
                                InsuranceCode = row.GetCell(23)?.ToString() ?? "",
                                BedNumber = row.GetCell(24)?.ToString() ?? "",
                                SpecimenName = row.GetCell(25)?.ToString() ?? "",
                                VisitSerialNumber = row.GetCell(26)?.ToString() ?? "",
                                PrimaryDiagnosisCode = row.GetCell(27)?.ToString() ?? "",
                            };

                            excelExportLogList.Add(excelExportLogItem);
                        }
                    }
                }

                ExportExcelLogViewModel viewModel = new ExportExcelLogViewModel();
                viewModel.ExcelExportLogList = excelExportLogList;
                viewModel.ExcelFilePath = filePath;

                return View("ExcelImport", viewModel); // Pass the column names to the view
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while processing the file: {ex.Message}";
                return View("ExcelImport");
            }
        }

        // 下載轉出結果 Excel 檔案
        [HttpPost]
        public IActionResult ExportToExcel([FromBody] ExportExcelLogViewModel viewModel)
        {
            try {
                if (string.IsNullOrWhiteSpace(viewModel.ExcelFilePath))
                {
                    return BadRequest("File path is not provided.");
                }

                var filePath = viewModel.ExcelFilePath;

                // Step 1: Check if the file exists
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("The specified Excel file does not exist.");
                }

                // Step 2: Check file extension
                string fileExtension = Path.GetExtension(filePath).ToLower();
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    return BadRequest("Invalid file format. Please provide an Excel file with .xls or .xlsx extension.");
                }

                // Load the Excel file
                IWorkbook workbook;
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    if (fileExtension == ".xls")
                    {
                        workbook = new HSSFWorkbook(fileStream); // For .xls files
                    }
                    else
                    {
                        workbook = new XSSFWorkbook(fileStream); // For .xlsx files
                    }
                }

                // Step 3: Check/Create second sheet
                ISheet sheet;
                if (workbook.NumberOfSheets >= 2)
                {
                    sheet = workbook.GetSheetAt(1); // Get the second sheet (index starts at 0)
                }
                else
                {
                    sheet = workbook.CreateSheet("匯入格式"); // Create the second sheet if it doesn't exist
                }

                // Step 4: Add headers
                var headers = new[]{"看診日", "病歷號", "序號", "檢驗代碼", "細項代碼", "細項名稱", "傳送碼", "組別", "上機號", "醫令序號", "完報時間", "完報人", "結果值", "狀態值"};

                var headerRow = sheet.CreateRow(0);
                for (int i = 0; i < headers.Length; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(headers[i]);
                }

                string connectionName = "";
                if (User.Identity.IsAuthenticated)
                {
                    connectionName = User.FindFirst(ClaimTypes.Role)?.Value;
                }

                // Step 5: Insert row data
                if (viewModel.ExcelExportLogList != null)
                {
                    for (int rowIndex = 0; rowIndex < viewModel.ExcelExportLogList.Count; rowIndex++)
                    {
                        var log = viewModel.ExcelExportLogList[rowIndex];
                        string formattedConsultationDate = log.ConsultationDate.Insert(3, "/").Insert(6, "/");
                        // 轉換檢驗項目名稱
                        string convertItemName = _clinicService.GetInDatabaseItemName(connectionName, log.SubitemName, "G001");
                        // 轉換病歷號
                        string convertMedicalRecordNumber = log.MedicalRecordNumber.TrimStart('0');
                        InspectionViewModel? detailInfo = _clinicService.GetExportDetail(connectionName, log.PatientName, formattedConsultationDate, convertItemName, convertMedicalRecordNumber);
                        
                        var row = sheet.CreateRow(rowIndex + 1); // Data starts from the second row

                        row.CreateCell(0).SetCellValue(log.ConsultationDate);
                        row.CreateCell(1).SetCellValue(log.MedicalRecordNumber);
                        row.CreateCell(2).SetCellValue(log.SerialNumber);
                        row.CreateCell(3).SetCellValue(log.TestCode);
                        row.CreateCell(4).SetCellValue(log.SubitemCode);
                        row.CreateCell(5).SetCellValue(log.SubitemName);
                        row.CreateCell(6).SetCellValue(log.TransmissionCode);
                        row.CreateCell(7).SetCellValue(log.GroupName);
                        row.CreateCell(8).SetCellValue(log.MachineNumber);
                        row.CreateCell(9).SetCellValue(log.OrderSerialNumber);
                        string formattedAuditTimme = "";
                        if (detailInfo != null)
                        {
                            formattedAuditTimme = (detailInfo.AuditDate + detailInfo.AuditTime).Replace("/", "").Replace(" ", "").Replace(":", "");
                        }
                        row.CreateCell(10).SetCellValue(formattedAuditTimme);
                        row.CreateCell(11).SetCellValue(detailInfo == null ? "" : detailInfo.Reporter);
                        row.CreateCell(12).SetCellValue(detailInfo == null ? "" : detailInfo.Result);
                        row.CreateCell(13).SetCellValue("N");
                    }
                }

                // Save the workbook to a memory stream
                using (var memoryStream = new MemoryStream())
                {
                    workbook.Write(memoryStream);

                    // Step 6: Delete the existing Excel file
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedResult.xls");
                }
            

            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating Excel file: {ex.Message}");
            }
        }

        // 進入組織首頁
        [Authorize]
		public IActionResult ClinicHome(string customerId, string connectionName)
		{
            try
            {
                // 0. 取得診所資料
                var customer = _clinicService.GetCustomer(connectionName, customerId);

                // 0.1 當天日期
                DateTime currentDate = DateTime.Now.Date;
                DateTime endOfDay = currentDate.Date.AddDays(1).AddTicks(-1);

                var finalResult = _clinicService.GetTestDocList(connectionName, customerId, currentDate, endOfDay);

                ClinicHomeViewModel model = new ClinicHomeViewModel
                {
                    customer = customer,
                    TestDOCList = finalResult,
                    connectionName = connectionName
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // 診所重新給條件來取得列表
        [HttpPost]
        public IActionResult GetClinicTestDoscList([FromBody] ClinicTestDocRequest param)
        {
            string connectionName = "";
            if (User.Identity.IsAuthenticated)
            {
                connectionName = User.FindFirst(ClaimTypes.Role)?.Value;
            } else
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                // 0. 取得診所資料
                var customerId = param.CustID; // Declare the 'customerId' variable
                var customer = _clinicService.GetCustomer(connectionName, customerId);

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

                var finalResult = _clinicService.GetTestDocListWithKeywords(connectionName, customerId, beginDate, endDate, param.Keywords);

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

        // 進入詳細畫面(報告格式 1)
        [Authorize]
        public IActionResult ClinicDetails(string id)
        {
            string connectionName = "";
            if (User.Identity.IsAuthenticated)
            {
                connectionName = User.FindFirst(ClaimTypes.Role)?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var result = _clinicService.GetTestDocDetail(connectionName, id);
                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // 進入詳細畫面(報告格式 2)
        [Authorize]
        public IActionResult ClinicDetails2(string id)
        {
            string connectionName = "";
            if (User.Identity.IsAuthenticated)
            {
                connectionName = User.FindFirst(ClaimTypes.Role)?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var result = _clinicService.GetTestDocDetail(connectionName, id);
                return View(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // 進入詳細畫面(報告格式 3)
        [Authorize]
        public IActionResult ClinicDetails3(string id)
        {
            string connectionName = "";
            if (User.Identity.IsAuthenticated)
            {
                connectionName = User.FindFirst(ClaimTypes.Role)?.Value;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var result = _clinicService.GetTestDocDetail(connectionName, id);
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
