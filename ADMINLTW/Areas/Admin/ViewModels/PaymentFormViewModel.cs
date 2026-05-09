using System.ComponentModel.DataAnnotations;

namespace TrungTamLapTrinh.Web.Areas.Admin.ViewModels
{
    public class PaymentFormViewModel
    {
        public long PaymentId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn mã ghi danh")]
        [Display(Name = "Ghi danh")]
        public long EnrollmentId { get; set; }

        [Required(ErrorMessage = "Số tiền là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền không hợp lệ")]
        [Display(Name = "Số tiền (VNĐ)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        [Display(Name = "Phương thức")]
        public int MethodId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        [Display(Name = "Trạng thái")]
        public int StatusId { get; set; }

        [StringLength(100, ErrorMessage = "Mã giao dịch không vượt quá 100 ký tự")]
        [Display(Name = "Mã giao dịch (Nếu có)")]
        public string? TransactionNo { get; set; }

        [Display(Name = "Ngày thanh toán")]
        public DateTime? PaidAt { get; set; }
    }
}
