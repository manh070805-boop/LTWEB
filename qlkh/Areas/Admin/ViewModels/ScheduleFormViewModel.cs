using System.ComponentModel.DataAnnotations;

namespace qlkh.Areas.Admin.ViewModels
{
    public class ScheduleFormViewModel
    {
        public long ScheduleId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn lớp học")]
        [Display(Name = "Lớp học")]
        public string ClassId { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn ngày trong tuần")]
        [Range(0, 6, ErrorMessage = "Ngày trong tuần không hợp lệ (0: CN, 1: T2... 6: T7)")]
        [Display(Name = "Ngày trong tuần (0=CN, 1=T2, ..., 6=T7)")]
        public int DayOfWeek { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giờ bắt đầu")]
        [Display(Name = "Giờ bắt đầu")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giờ kết thúc")]
        [Display(Name = "Giờ kết thúc")]
        public TimeSpan EndTime { get; set; }

        [StringLength(50, ErrorMessage = "Tên phòng không được vượt quá 50 ký tự")]
        [Display(Name = "Phòng học (Tùy chọn)")]
        public string? RoomName { get; set; }
    }
}
