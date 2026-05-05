using System.ComponentModel.DataAnnotations;

namespace qlkh.Models.ViewModels
{
    public class StudentProfileViewModel
    {
        public long AccountId { get; set; }
        public string StudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username không được để trống")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string Phone { get; set; } = string.Empty;

        public string? Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
    }
}