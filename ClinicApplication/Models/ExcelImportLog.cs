namespace ClinicApplication.Models
{
    public class ExcelImportLog
    {
        public string VisitDate { get; set; } // 看診日

        public string MedicalRecordNo { get; set; } // 病歷號

        public string SequenceNo { get; set; } // 序號

        public string TestCode { get; set; } // 檢驗代碼

        public string SubItemCode { get; set; } // 細項代碼

        public string SubItemName { get; set; } // 細項名稱

        public string TransmissionCode { get; set; } // 傳送碼

        public string TestGroup { get; set; } // 組別

        public string MachineNo { get; set; } // 上機號

        public string OrderSequenceNo { get; set; } // 醫令序號
    }

}
