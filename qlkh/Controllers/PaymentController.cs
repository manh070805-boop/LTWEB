using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qlkh.Data;

namespace qlkh.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

    public IActionResult Checkout(long enrollmentId)
   {
        var payment = _context.Payments
             .Include(p => p.Enrollment!)
            .ThenInclude(e => e.Course)
            .Include(p => p.Method)
            .Include(p => p.Status)
            .FirstOrDefault(p => p.EnrollmentId == enrollmentId);

        if (payment == null)
        {
           return NotFound();
        }

            ViewBag.BankCode = "MB";
            ViewBag.AccountNo = "0123456789";
            ViewBag.AccountName = "NGUYEN VAN THAN";
            ViewBag.Amount = payment.Amount;
            ViewBag.Content = $"DKKH{payment.EnrollmentId}";

            return View(payment);
        }

        [HttpPost]
        public IActionResult ConfirmPaid(long paymentId)
        {
            var payment = _context.Payments
                .FirstOrDefault(p => p.PaymentId == paymentId);

            if (payment == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Không tìm thấy thanh toán."
                });
            }

            // Không đổi PAID, chỉ ghi nhận học viên đã xác nhận
            payment.TransactionNo = $"CONFIRM_{DateTime.Now:yyyyMMddHHmmss}";
            _context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Đã gửi xác nhận thanh toán. Vui lòng chờ admin duyệt."
            });
        }
    }
}