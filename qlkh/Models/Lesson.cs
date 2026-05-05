using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
[Table("Lesson")]
    public class Lesson
    {
        [Key]
        public long LessonId { get; set; }

        [Required]
        [StringLength(5)]
        public string ClassId { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Title { get; set; } = string.Empty;

        public string? VideoUrl { get; set; }

        public int OrderIndex { get; set; }

        [ForeignKey("ClassId")]
        public CenterClass? CenterClass { get; set; }
    }
}