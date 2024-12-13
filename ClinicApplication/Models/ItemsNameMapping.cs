using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicApplication.Models
{
    public class ItemsNameMapping
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("InDatabaseName")]
        [Required]
        [StringLength(100)]
        public string InDatabaseName { get; set; }

        [Column("ConvertedName")]
        [Required]
        [StringLength(100)]
        public string ConvertedName { get; set; }

        [Column("CustID")]
        [Required]
        [StringLength(50)]
        public string CustId { get; set; }

        [Column("IsActive")]
        public int? IsActive { get; set; }
    }
}