using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
[Table("CenterClass")]
    public class CenterClass
    {
        [Key]
        [StringLength(5)]
        public string ClassId { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string ClassName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? RoomName { get; set; }

        // FK
        [Required]
        [StringLength(5)]
        public string CourseId { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string TeacherId { get; set; } = string.Empty;

        [Required]
        public int StatusId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int? MaxStudents { get; set; }

        public DateTime CreatedAt { get; set; }

        // ========================
        // NAVIGATION
        // ========================

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }

        [ForeignKey("StatusId")]
        public ClassStatus? Status { get; set; }

        // 1 class có nhiều lesson
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        // 1 class có nhiều student (qua Enrollment)
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}