using ClinicApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ClinicApplication.Services
{
    public class ClinicService
    {
        private readonly DbContextFactory _dbContextFactory;

        public ClinicService(DbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        // 登入使用
        public async Task<Customer?> ValidateCustomerAsync(string connectionName, string username, string password)
        {
            using var dbContext = _dbContextFactory.CreateDbContext(connectionName);
            return await dbContext.Customer
                .FirstOrDefaultAsync(c => c.Id == username && c.WebPassWD == password);
        }

        // 取得檢驗結果


        // 取得診所的資料
        public Customer? GetCustomer(string connectionName, string customerId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext(connectionName);
            var customer = dbContext.Customer.Find(customerId);
            return customer;
        }

        // 取得診所特定時間內的報告資料
        public List<TestDOC>? GetTestDocList(string connectionName, string customerId, DateTime currentDate, DateTime endOfDay) 
        {
            using var dbContext = _dbContextFactory.CreateDbContext(connectionName);
            // 1. 先從資料庫拉出這間診所的紀錄
            List<TestDOC> tempResult = dbContext.TestDOC
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

            return finalResult;
        }

        // 取得診所特定時間並過濾關鍵字的報告資料
        public List<TestDOC>? GetTestDocListWithKeywords(string connectionName, string customerId, DateTime currentDate, DateTime endOfDay, string Keywords)
        {
            using var dbContext = _dbContextFactory.CreateDbContext(connectionName);
            // 1. 先從資料庫拉出這間診所的紀錄
            List<TestDOC> tempResult = dbContext.TestDOC
                .Where(doc => doc.CustID == customerId && (doc.SubName.Contains(Keywords) || doc.SNO.Contains(Keywords) || doc.OrderNo.Contains(Keywords)))
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

            return finalResult;
        }
    
        // 取得檢驗報告詳細資料
        public List<InspectionViewModel> GetTestDocDetail(string connectionName, string id)
        {
            using var dbContext = _dbContextFactory.CreateDbContext(connectionName);
            var query = from a in dbContext.TestDOC
                        join b in dbContext.TestDetail on a.SNO equals b.SNO into ab
                        from b in ab.DefaultIfEmpty()
                        join c in dbContext.Items on b.ItemID equals c.ID into bc
                        from c in bc.DefaultIfEmpty()
                        join d in dbContext.Customer on a.CustID equals d.Id into ad
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
                            InstitutionCode = d.AgencyCode,
                            InspDate = a.InspDate
                        };

            List<InspectionViewModel> result = query.ToList();
            return result;
        }

        // 取得 勤美-安泰 外送格式結果(完整時間 完報人 結果值 狀態值)
        public InspectionViewModel? GetExportDetail(string connectionName, string patientName, string inspDate, string itemName)
        {
            using var dbContext = _dbContextFactory.CreateDbContext(connectionName);
            var query = from a in dbContext.TestDOC
                        join b in dbContext.TestDetail on a.SNO equals b.SNO into ab
                        from b in ab.DefaultIfEmpty()
                        join c in dbContext.Items on b.ItemID equals c.ID into bc
                        from c in bc.DefaultIfEmpty()
                        join d in dbContext.Customer on a.CustID equals d.Id into ad
                        from d in ad.DefaultIfEmpty()
                        where a.CustID == "G001" && a.InspDate == inspDate && a.SubName == patientName && b.Name == itemName && b.IsVerify == true
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
                            InstitutionCode = d.AgencyCode,
                            InspDate = a.InspDate
                        };
            return query.FirstOrDefault();
        }

    }
}
