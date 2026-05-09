using System.ComponentModel.DataAnnotations;

namespace TrungTamLapTrinh.Web.Areas.Admin.ViewModels
{
    public class EnrollmentFormViewModel
    {
        public long EnrollmentId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn học viên")]
        [Display(Name = "Học viên")]
        public string StudentId { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn khóa học")]
        [Display(Name = "Khóa học")]
        public string CourseId { get; set; } = null!;

        [Display(Name = "Lớp học (Tùy chọn)")]
        public string? ClassId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [Display(Name = "Trạng thái ghi danh")]
        public int StatusId { get; set; }

        [Display(Name = "Giảng viên duyệt")]
        public string? ApprovedByTeacherId { get; set; }

        [Display(Name = "Ngày duyệt")]
        public DateTime? ApprovedAt { get; set; }

        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }
    }
}
