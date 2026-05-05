using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace qlkh.Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public string StudentId { get; set; } = string.Empty;

        public long AccountId { get; set; }
        
        [Required]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Address { get; set; }

        public DateTime? Birthday { get; set; }

        [ForeignKey("AccountId")]
        public Account? Account { get; set; }

        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}