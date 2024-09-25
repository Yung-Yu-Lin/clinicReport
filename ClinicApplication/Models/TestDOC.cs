using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApplication.Models
{
	public class TestDOC
	{
        [Key]
        [Column("SNO")]
        public string? SNO { get; set; }

        [Column("OrderNo")]
        public string? OrderNo { get; set; }

        [Column("CustID")]
        public string? CustID { get; set; }

        [Column("SubName")]
        public string? SubName { get; set; }

        [Column("SubBirthDay")]
        public string? SubBirthDay { get; set; }

        [Column("SubAge")]
        public string? SubAge { get; set; }

        [Column("SubIDNO")]
        public string? SubIDNO { get; set; }

        [Column("SubGender")]
        public string? SubGender { get; set; }

        [Column("MedicalNo")]
        public string? MedicalNo { get; set; }

        [Column("SpecimenConditions")]
        public string? SpecimenConditions { get; set; }

        [Column("TestSpecies")]
        public string? TestSpecies { get; set; }

        [Column("RecDate")]
        public string? RecDate { get; set; }

        [Column("InspDate")]
        public string? InspDate { get; set; }

        [Column("ReportDate")]
        public string? ReportDate { get; set; }

        [Column("PickDate")]
        public string? PickDate { get; set; }

        [Column("Amount")]
        public decimal? Amount { get; set; }

        [Column("RegEmp")]
        public string? RegEmp { get; set; }

        [Column("Payment")]
        public string? Payment { get; set; }

        [Column("Tel")]
        public string? Tel { get; set; }

        [Column("Address")]
        public string? Address { get; set; }

        [Column("Reviewers")]
        public string? Reviewers { get; set; }

        [Column("AuditDay")]
        public string? AuditDay { get; set; }

        [Column("AuditTime")]
        public string? AuditTime { get; set; }

        [Column("Examiner")]
        public string? Examiner { get; set; }

        [Column("Completion")]
        public bool? Completion { get; set; }

        [Column("Printed")]
        public bool? Printed { get; set; }

        [Column("IsPass")]
        public bool? IsPass { get; set; }

        [Column("AccountMonth")]
        public string? AccountMonth { get; set; }

        [Column("GoldenGate")]
        public bool? GoldenGate { get; set; }

        [Column("OwnExpense")]
        public decimal? OwnExpense { get; set; }

        [Column("prnA4")]
        public bool? PrnA4 { get; set; }

        [Column("A4Printed")]
        public bool? A4Printed { get; set; }

        [Column("IsPrescription")]
        public bool? IsPrescription { get; set; }

        [Column("BarCodePlusOne")]
        public bool? BarCodePlusOne { get; set; }

        [Column("Paid")]
        public long? Paid { get; set; }

        [Column("CheckOut")]
        public bool? CheckOut { get; set; }

        [Column("HealthPrinted")]
        public bool? HealthPrinted { get; set; }

        [Column("IsFobt")]
        public bool? IsFobt { get; set; }

        [Column("Payee")]
        public string? Payee { get; set; }

        [Column("IsDialysis")]
        public bool? IsDialysis { get; set; }

        [Column("prnB5")]
        public bool? PrnB5 { get; set; }

        [Column("B5Printed")]
        public bool? B5Printed { get; set; }
    }
}