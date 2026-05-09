using System.ComponentModel.DataAnnotations;

namespace qlkh.Areas.Admin.ViewModels
{
    public class AccountFormViewModel
    {
        public long AccountId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn quyền")]
        [Display(Name = "Quyền")]
        public int RoleId { get; set; }

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; } = true;
    }
}
