using System.ComponentModel.DataAnnotations;

namespace qlkh.Areas.Admin.ViewModels
{
    public class EnrollmentFormViewModel
    {
        public long EnrollmentId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn học viên")]
        [Display(Name = "Học viên")]
        public string StudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn khóa học")]
        [Display(Name = "Khóa học")]
        public string CourseId { get; set; } = string.Empty;

        [Display(Name = "Lớp học")]
        public string? ClassId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [Display(Name = "Trạng thái")]
        public int StatusId { get; set; }

        [Display(Name = "Giảng viên phê duyệt")]
        public string? ApprovedByTeacherId { get; set; }

        [Display(Name = "Ngày phê duyệt")]
        [DataType(DataType.DateTime)]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string? Note { get; set; }
    }
}
