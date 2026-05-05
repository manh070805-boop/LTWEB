using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
    [Table("EnrollmentStatus")]
    public class EnrollmentStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [StringLength(30)]
        public string StatusCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string StatusName { get; set; } = string.Empty;

        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}