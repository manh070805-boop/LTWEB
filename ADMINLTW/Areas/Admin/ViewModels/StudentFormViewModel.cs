using System.ComponentModel.DataAnnotations;

namespace TrungTamLapTrinh.Web.Areas.Admin.ViewModels
{
    public class StudentFormViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập mã học viên")]
        [Display(Name = "Mã học viên")]
        [StringLength(5)]
        public string StudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn tài khoản")]
        [Display(Name = "Tài khoản")]
        public long AccountId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [StringLength(15)]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Địa chỉ")]
        [StringLength(250)]
        public string? Address { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly? Birthday { get; set; }
    }
}
