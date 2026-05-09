using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace qlkh.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        [Key]
        public long ScheduleId { get; set; }

        [Required]
        [StringLength(5)]
        public string ClassId { get; set; } = string.Empty;

        public int DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [StringLength(100)]
        public string? RoomName { get; set; }

        [ForeignKey("ClassId")]
        public CenterClass? CenterClass { get; set; }
    }
}
