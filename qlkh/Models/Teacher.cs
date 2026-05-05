using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
    [Table("Teacher")]
    public class Teacher
    {
        [Key]
        [StringLength(5)]
        public string TeacherId { get; set; } = string.Empty;

        public long AccountId { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Specialization { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        [ForeignKey("AccountId")]
        public Account? Account { get; set; }

        public ICollection<CenterClass> Classes { get; set; } = new List<CenterClass>();
    }
}