using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ClinicApplication.Models
{
    [PrimaryKey(nameof(SNO), nameof(ItemID))]
    public class TestDetail
    {
        [Column("SNO")]
        public string? SNO { get; set; }

        [Column("ItemID")]
        public string? ItemID { get; set; }

        [Column("SetID")]
        public string? SetID { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Result")]
        public string? Result { get; set; }

        [Column("Interpretation")]
        public string? Interpretation { get; set; }

        [Column("Price")]
        public decimal? Price { get; set; }

        [Column("NHI_Price")]
        public int? NHI_Price { get; set; }

        [Column("Discount")]
        public int? Discount { get; set; }

        [Column("NHI_ID")]
        public string? NHI_ID { get; set; }

        [Column("IsNHI")]
        public bool? IsNHI { get; set; }

        [Column("IsPrint")]
        public bool? IsPrint { get; set; }

        [Column("TestID")]
        public string? TestID { get; set; }

        [Column("SubID")]
        public int? SubID { get; set; }

        [Column("Unquoted")]
        public bool? Unquoted { get; set; }

        [Column("KeyIn")]
        public string? KeyIn { get; set; }

        [Column("Recheck")]
        public bool? Recheck { get; set; }

        [Column("Sample_KindID")]
        public int? Sample_KindID { get; set; }

        [Column("IsExcep")]
        public bool? IsExcep { get; set; }

        [Column("OutSource")]
        public string? OutSource { get; set; }

        [Column("Agio")]
        public int? Agio { get; set; }

        [Column("NonPricing")]
        public bool? NonPricing { get; set; }

        [Column("IsChkD")]
        public bool? IsChkD { get; set; }
    }
}
