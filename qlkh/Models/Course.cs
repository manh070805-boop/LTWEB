using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        public string CourseId { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string CourseName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal Price { get; set; }

        public int? DurationMonths { get; set; }

        public string? Roadmap { get; set; }

        public int LevelId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        [ForeignKey("LevelId")]
        public CourseLevel? CourseLevel { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}