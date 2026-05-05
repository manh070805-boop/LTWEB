using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
    [Table("Enrollment")]
    public class Enrollment
    {
        [Key]
        public long EnrollmentId { get; set; }

        [Required]
        [StringLength(5)]
        public string StudentId { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string CourseId { get; set; } = string.Empty;

        [StringLength(5)]
        public string? ClassId { get; set; }

        public int StatusId { get; set; }

        [StringLength(5)]
        public string? ApprovedByTeacherId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        [StringLength(500)]
        public string? Note { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        [ForeignKey("StatusId")]
        public EnrollmentStatus? EnrollmentStatus { get; set; }

        [ForeignKey("ClassId")]
        public CenterClass? CenterClass { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
