namespace ClinicApplication.Models
{
    public class ExcelExportLog
    {
        public string DiagnosisType { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string IdNumber { get; set; }
        public string BirthDate { get; set; }
        public string Department { get; set; }
        public string PrescribingDoctor { get; set; }
        public string ConsultationDate { get; set; }
        public string MedicalRecordNumber { get; set; }
        public string SerialNumber { get; set; }
        public string TestCode { get; set; }
        public string SubitemCode { get; set; }
        public string SubitemName { get; set; }
        public string TransmissionCode { get; set; }
        public string SamplingDate { get; set; }
        public string Reporter { get; set; }
        public string GroupName { get; set; } // Corrected naming to match your SQL script
        public string MachineNumber { get; set; }
        public string OrderSerialNumber { get; set; }
        public string TestName { get; set; }
        public string SpecimenQuantity { get; set; }
        public string ExternalDeliveryCode { get; set; }
        public string ExternalDeliveryName { get; set; }
        public string InsuranceCode { get; set; }
        public string BedNumber { get; set; }
        public string SpecimenName { get; set; }
        public string VisitSerialNumber { get; set; }
        public string PrimaryDiagnosisCode { get; set; }
    }

}
