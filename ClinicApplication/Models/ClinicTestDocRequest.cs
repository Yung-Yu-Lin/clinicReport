namespace ClinicApplication.Models
{
    public class ClinicTestDocRequest
    {
        public string CustID { get; set; }
        public string? Keywords { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
