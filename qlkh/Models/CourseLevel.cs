using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
    [Table("CourseLevel")]
    public class CourseLevel
    {
        [Key]
        public int LevelId { get; set; }

        [Required]
        [StringLength(30)]
        public string LevelCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LevelName { get; set; } = string.Empty;

        public ICollection<Course>? Courses { get; set; }

    }
}