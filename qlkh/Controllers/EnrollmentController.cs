using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qlkh.Data;
using qlkh.Helpers;
using qlkh.Models;

namespace qlkh.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(string courseId)
        {
            if (!AuthHelper.IsLoggedIn(HttpContext))
            {
                TempData["Error"] = "Bạn cần đăng nhập trước khi đăng ký khóa học.";
                return RedirectToAction("Login", "Account");
            }

            if (!AuthHelper.IsStudent(HttpContext))
            {
                TempData["Error"] = "Chỉ học viên mới có quyền đăng ký khóa học.";
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrWhiteSpace(courseId))
            {
                TempData["Error"] = "Mã khóa học không hợp lệ.";
                return RedirectToAction("Index", "Course");
            }

            var accountId = AuthHelper.GetAccountId(HttpContext);

            if (accountId == null)
            {
                TempData["Error"] = "Phiên đăng nhập không hợp lệ.";
                return RedirectToAction("Login", "Account");
            }

            var student = _context.Students
                .FirstOrDefault(s => s.AccountId == accountId.Value);

            if (student == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin học viên.";
                return RedirectToAction("Index", "Home");
            }

            var course = _context.Courses
                .FirstOrDefault(c => c.CourseId == courseId && c.IsActive);

            if (course == null)
            {
                TempData["Error"] = "Khóa học không tồn tại hoặc đã ngừng hoạt động.";
                return RedirectToAction("Index", "Course");
            }

            var existingEnrollment = _context.Enrollments
                .FirstOrDefault(e => e.StudentId == student.StudentId && e.CourseId == courseId);

            if (existingEnrollment != null)
            {
                TempData["Error"] = "Bạn đã đăng ký khóa học này rồi.";
                return RedirectToAction("MyCourses", "Student");
            }

            var pendingEnrollmentStatus = _context.EnrollmentStatuses
                .FirstOrDefault(s => s.StatusCode == "PENDING");

            if (pendingEnrollmentStatus == null)
            {
                TempData["Error"] = "Hệ thống chưa cấu hình trạng thái đăng ký PENDING.";
                return RedirectToAction("Details", "Course", new { id = courseId });
            }

            var vietQrMethod = _context.PaymentMethods
                .FirstOrDefault(m => m.MethodCode == "VIETQR");

            if (vietQrMethod == null)
            {
                TempData["Error"] = "Hệ thống chưa cấu hình phương thức thanh toán VIETQR.";
                return RedirectToAction("Details", "Course", new { id = courseId });
            }

            var pendingPaymentStatus = _context.PaymentStatuses
                .FirstOrDefault(s => s.StatusCode == "PENDING");

            if (pendingPaymentStatus == null)
            {
                TempData["Error"] = "Hệ thống chưa cấu hình trạng thái thanh toán PENDING.";
                return RedirectToAction("Details", "Course", new { id = courseId });
            }

            var enrollment = new Enrollment
            {
                StudentId = student.StudentId,
                CourseId = course.CourseId,
                StatusId = pendingEnrollmentStatus.StatusId,
                CreatedAt = DateTime.Now,
                Note = "Đăng ký từ giao diện học viên"
            };

            try
            {
                _context.Enrollments.Add(enrollment);
                _context.SaveChanges();

                var payment = new Payment
                {
                    EnrollmentId = enrollment.EnrollmentId,
                    Amount = course.Price,
                    MethodId = vietQrMethod.MethodId,
                    StatusId = pendingPaymentStatus.StatusId,
                    CreatedAt = DateTime.Now
                };

                _context.Payments.Add(payment);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Khong the luu dang ky. Vui long kiem tra cau hinh du lieu hoac khoa ngoai trong database.";
                return RedirectToAction("Details", "Course", new { id = courseId });
            }

            TempData["Success"] = "Đăng ký khóa học thành công. Vui lòng thanh toán học phí.";

            return RedirectToAction("Checkout", "Payment", new
            {
                enrollmentId = enrollment.EnrollmentId
            });
        }
    }
}
