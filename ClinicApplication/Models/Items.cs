using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicApplication.Models
{
    public class Items
    {
        [Key]
        [Column("ID")]
        public string? ID { get; set; }

        [Column("Kind_ID")]
        public string? Kind_ID { get; set; }

        [Column("E_Name")]
        public string? E_Name { get; set; }

        [Column("C_Name")]
        public string? C_Name { get; set; }

        [Column("Unit")]
        public string? Unit { get; set; }

        [Column("PX")]
        public string? PX { get; set; }

        [Column("NHI_ID")]
        public string? NHI_ID { get; set; }

        [Column("STD")]
        public string? STD { get; set; }

        [Column("M_War_Low")]
        public string? M_War_Low { get; set; }

        [Column("M_Std_Low")]
        public string? M_Std_Low { get; set; }

        [Column("M_Std_Up")]
        public string? M_Std_Up { get; set; }

        [Column("M_War_Up")]
        public string? M_War_Up { get; set; }

        [Column("M_Ref")]
        public string? M_Ref { get; set; }

        [Column("M_Memo")]
        public string? M_Memo { get; set; }

        [Column("F_War_Low")]
        public string? F_War_Low { get; set; }

        [Column("F_Std_Low")]
        public string? F_Std_Low { get; set; }

        [Column("F_Std_Up")]
        public string? F_Std_Up { get; set; }

        [Column("F_War_Up")]
        public string? F_War_Up { get; set; }

        [Column("F_Ref")]
        public string? F_Ref { get; set; }

        [Column("F_Memo")]
        public string? F_Memo { get; set; }

        [Column("SX")]
        public string? SX { get; set; }

        [Column("CX")]
        public string? CX { get; set; }

        [Column("WN")]
        public string? WN { get; set; }

        [Column("YN")]
        public string? YN { get; set; }

        [Column("Cost")]
        public int? Cost { get; set; }

        [Column("OPD_Price")]
        public int? OPD_Price { get; set; }

        [Column("Fix_Price")]
        public int? Fix_Price { get; set; }

        [Column("NHI_Price")]
        public int? NHI_Price { get; set; }

        [Column("Spec_Price")]
        public int? Spec_Price { get; set; }

        [Column("REMARK")]
        public string? REMARK { get; set; }

        [Column("Check_kind")]
        public string? Check_kind { get; set; }

        [Column("Send_Out")]
        public char? Send_Out { get; set; }

        [Column("NIH_Give")]
        public char? NIH_Give { get; set; }

        [Column("Group_BR")]
        public char? Group_BR { get; set; }

        [Column("mSay")]
        public char? mSay { get; set; }

        [Column("PerformanceDays")]
        public int? PerformanceDays { get; set; }

        [Column("CE_P")]
        public string? CE_P { get; set; }

        [Column("STA")]
        public string? STA { get; set; }

        [Column("CHEC")]
        public string? CHEC { get; set; }

        [Column("XTYPE")]
        public string? XTYPE { get; set; }

        [Column("IsActive")]
        public bool? IsActive { get; set; }

        [Column("USED_YN")]
        public string? USED_YN { get; set; }

        [Column("Print_Order")]
        public int? Print_Order { get; set; }

        [Column("Mod_Date")]
        public string? Mod_Date { get; set; }

        [Column("Abbr")]
        public string? Abbr { get; set; }

        [Column("Work_ID")]
        public string? Work_ID { get; set; }

        [Column("QC_Section")]
        public string? QC_Section { get; set; }

        [Column("QC_Name")]
        public string? QC_Name { get; set; }

        [Column("myWeek")]
        public string? myWeek { get; set; }

        [Column("RE_Unit")]
        public string? RE_Unit { get; set; }

        [Column("QC_Secton")]
        public string? QC_Secton { get; set; }

        [Column("Tcode")]
        public string? Tcode { get; set; }

        [Column("RefOutCode")]
        public string? RefOutCode { get; set; }

        [Column("NIH_YES")]
        public string? NIH_YES { get; set; }

        [Column("DangVal")]
        public int? DangVal { get; set; }

        [Column("M_Ref_21")]
        public string? M_Ref_21 { get; set; }

        [Column("F_Ref_21")]
        public string? F_Ref_21 { get; set; }

        [Column("DelayTime")]
        public int? DelayTime { get; set; }

        [Column("NonInput")]
        public string? NonInput { get; set; }

        [Column("Sample_KindID")]
        public int? Sample_KindID { get; set; }

        [Column("CalType")]
        public bool? CalType { get; set; }

        [Column("Formula")]
        public string? Formula { get; set; }

        [Column("PrintMemo")]
        public bool? PrintMemo { get; set; }

        [Column("PrintRef")]
        public bool? PrintRef { get; set; }

        [Column("ToAsiaVision")]
        public bool? ToAsiaVision { get; set; }

        [Column("ReportType")]
        public string? ReportType { get; set; }

        [Column("SyhContracts")]
        public bool? SyhContracts { get; set; }

        [Column("X_Memo")]
        public string? X_Memo { get; set; }

        [Column("RepClass")]
        public string? RepClass { get; set; }

        [Column("IsBarcode")]
        public bool? IsBarcode { get; set; }

        [Column("TestMethod")]
        public string? TestMethod { get; set; }

        [Column("SpType")]
        public int? SpType { get; set; }
    }
}