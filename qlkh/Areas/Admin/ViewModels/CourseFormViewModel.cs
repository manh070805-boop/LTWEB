using System.ComponentModel.DataAnnotations;

namespace qlkh.Areas.Admin.ViewModels
{
    public class CourseFormViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã khóa học")]
        [Display(Name = "Mã khóa học")]
        [StringLength(5)]
        public string CourseId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tên khóa học")]
        [Display(Name = "Tên khóa học")]
        [StringLength(250)]
        public string CourseName { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Đường dẫn ảnh")]
        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập học phí")]
        [Display(Name = "Học phí")]
        [Range(0, 1000000000)]
        public decimal Price { get; set; }

        [Display(Name = "Thời lượng (Tháng)")]
        [Range(1, 60)]
        public int? DurationMonths { get; set; }

        [Display(Name = "Lộ trình")]
        public string? Roadmap { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn cấp độ")]
        [Display(Name = "Cấp độ")]
        public int LevelId { get; set; }

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; } = true;
    }
}
