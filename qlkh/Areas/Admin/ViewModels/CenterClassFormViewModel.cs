using System.ComponentModel.DataAnnotations;

namespace qlkh.Areas.Admin.ViewModels
{
    public class CenterClassFormViewModel
    {
        [Required(ErrorMessage = "Mã lớp là bắt buộc")]
        [StringLength(20, ErrorMessage = "Mã lớp không vượt quá 20 ký tự")]
        [Display(Name = "Mã lớp")]
        public string ClassId { get; set; } = null!;

        [Required(ErrorMessage = "Tên lớp là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên lớp không vượt quá 100 ký tự")]
        [Display(Name = "Tên lớp")]
        public string ClassName { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Tên phòng không vượt quá 50 ký tự")]
        [Display(Name = "Phòng học")]
        public string? RoomName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khóa học")]
        [Display(Name = "Khóa học")]
        public string CourseId { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn giảng viên")]
        [Display(Name = "Giảng viên")]
        public string TeacherId { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [Display(Name = "Trạng thái")]
        public int StatusId { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Range(1, 100, ErrorMessage = "Số lượng học viên tối đa từ 1 đến 100")]
        [Display(Name = "Học viên tối đa")]
        public int? MaxStudents { get; set; }
    }
}
