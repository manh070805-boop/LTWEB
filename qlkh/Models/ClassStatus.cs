using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
[Table("ClassStatus")]
    public class ClassStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required]
        [StringLength(30)]
        public string StatusCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string StatusName { get; set; } = string.Empty;

        // 1 status có nhiều class
        public ICollection<CenterClass> Classes { get; set; } = new List<CenterClass>();
    }
}