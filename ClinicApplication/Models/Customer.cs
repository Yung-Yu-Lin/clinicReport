using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicApplication.Models
{
	public class Customer
	{
		[Key]
		[Column("ID")]
		public string? Id { get; set; }

		[Column("AbbName")]
		public string? AbbName { get; set; }

		[Column("Name")]
		public string? Name { get; set; }

		[Column("Tel1")]
		public string? Tel1 { get; set; }

		[Column("Tel2")]
		public string? Tel2 { get; set; }

		[Column("Fax")]
		public string? Fax { get; set; }

		[Column("Mobile")]
		public string? Mobile { get; set; }

		[Column("Address")]
		public string? Address { get; set; }

		[Column("TaxAddress")]
		public string? TaxAddress { get; set; }

		[Column("Boss")]
		public string? Boss { get; set; }

		[Column("Contact")]
		public string? Contact { get; set; }

		[Column("Agency_Code")]
		public string? AgencyCode { get; set; }

		[Column("PostID")]
		public string? PostId { get; set; }

		[Column("Email")]
		public string? Email { get; set; }

		[Column("PricingMode")]
		public char? PricingMode { get; set; }

		[Column("OPDPercent")]
		public int? OPDPercent { get; set; }

		[Column("FixPercent")]
		public int? FixPercent { get; set; }

		[Column("NHIPercent")]
		public int? NHIPercent { get; set; }

		[Column("SpecPercent")]
		public int? SpecPercent { get; set; }

		[Column("NHIDiscount")]
		public int? NHIDiscount { get; set; }

		[Column("Payment")]
		public char? Payment { get; set; }

		[Column("IsPrint")]
		public bool? IsPrint { get; set; }

		[Column("MT_YN")]
		public bool? MTYN { get; set; }

		[Column("PaperTitleID")]
		public string? PaperTitleId { get; set; }

		[Column("NotifyMethod")]
		public string? NotifyMethod { get; set; }

		[Column("IsPrintUnit")]
		public bool? IsPrintUnit { get; set; }

		[Column("IsPrintUnitID")]
		public bool? IsPrintUnitId { get; set; }

		[Column("Check_MT_ID")]
		public string? CheckMtId { get; set; }

		[Column("IsPrintUnitAddress")]
		public bool? IsPrintUnitAddress { get; set; }

		[Column("ItemSort")]
		public char? ItemSort { get; set; }

		[Column("IsHeaderAD")]
		public bool? IsHeaderAD { get; set; }

		[Column("HeaderAD")]
		public string? HeaderAD { get; set; }

		[Column("IsFooter")]
		public bool? IsFooter { get; set; }

		[Column("Footer")]
		public string? Footer { get; set; }

		[Column("Medical_ID")]
		public string? MedicalId { get; set; }

		[Column("WebPassWD")]
		public string? WebPassWD { get; set; }

		[Column("Case_Source")]
		public char? CaseSource { get; set; }

		[Column("SameKindSamePage")]
		public bool? SameKindSamePage { get; set; }

		[Column("LMD")]
		public string? LMD { get; set; }

		[Column("LMB")]
		public string? LMB { get; set; }

		[Column("unit_meno")]
		public string? UnitMeno { get; set; }

		[Column("Unit_Kind")]
		public char? UnitKind { get; set; }

		[Column("DoH_Code")]
		public string? DoHCode { get; set; }

		[Column("rpGeneralHC")]
		public int? RpGeneralHC { get; set; }

		[Column("rpGeneralHC_Copies")]
		public int? RpGeneralHCCopies { get; set; }

		[Column("rpAdultHC")]
		public int? RpAdultHC { get; set; }

		[Column("rpAdultHC_Copies")]
		public int? RpAdultHCCopies { get; set; }

		[Column("IsFloat")]
		public bool? IsFloat { get; set; }

		[Column("WorkList")]
		public bool? WorkList { get; set; }

		[Column("BillingFormat")]
		public int? BillingFormat { get; set; }

		[Column("IsAsiaVision")]
		public bool? IsAsiaVision { get; set; }

		[Column("prnMe")]
		public bool? PrnMe { get; set; }

		[Column("IsRealSun")]
		public bool? IsRealSun { get; set; }

		[Column("IsRoche")]
		public bool? IsRoche { get; set; }

		[Column("rpMicrobeCopies")]
		public int? RpMicrobeCopies { get; set; }

		[Column("RepWithClass")]
		public bool? RepWithClass { get; set; }

		[Column("prnSuggest")]
		public bool? PrnSuggest { get; set; }

		[Column("rpGeneralHC1")]
		public int? RpGeneralHC1 { get; set; }

		[Column("rpGeneralHC1_Copies")]
		public int? RpGeneralHC1Copies { get; set; }

		[Column("ppA4_Copies")]
		public int? PpA4Copies { get; set; }

		[Column("ppB5_Copies")]
		public int? PpB5Copies { get; set; }

		[Column("isPackages")]
		public bool? IsPackages { get; set; }

		[Column("IsAdd")]
		public bool? IsAdd { get; set; }

		[Column("Deactivate")]
		public bool? Deactivate { get; set; }

		[Column("PreBNO")]
		public string? PreBNO { get; set; }

		[Column("ppEmail")]
		public bool? PpEmail { get; set; }

		[Column("Sales")]
		public string? Sales { get; set; }
	}
}
