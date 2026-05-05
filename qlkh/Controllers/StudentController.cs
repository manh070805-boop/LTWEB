using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qlkh.Data;
using qlkh.Models.ViewModels;

namespace qlkh.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsStudent()
        {
            return HttpContext.Session.GetInt32("RoleId") == 3;
        }

        private int? GetAccountId()
        {
            return HttpContext.Session.GetInt32("AccountId");
        }

        public IActionResult Profile()
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsStudent())
                return RedirectToAction("Index", "Home");

            var accountId = GetAccountId()!.Value;

            var student = _context.Students
                .Include(s => s.Account)
                .FirstOrDefault(s => s.AccountId == accountId);

            if (student == null || student.Account == null)
                return RedirectToAction("Index", "Home");

            var vm = new StudentProfileViewModel
            {
                AccountId = student.AccountId,
                StudentId = student.StudentId,
                FullName = student.Account.FullName,
                Username = student.Account.Username,
                Email = student.Account.Email,
                Phone = student.Phone,
                Address = student.Address,
                Birthday = student.Birthday
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdateProfile(StudentProfileViewModel model)
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsStudent())
                return RedirectToAction("Index", "Home");

            var accountId = GetAccountId()!.Value;

            var student = _context.Students
                .Include(s => s.Account)
                .FirstOrDefault(s => s.AccountId == accountId);

            if (student == null || student.Account == null)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var usernameExists = _context.Accounts
                .Any(a => a.Username == model.Username && a.AccountId != accountId);

            if (usernameExists)
            {
                ModelState.AddModelError("Username", "Username đã tồn tại.");
                return View("Profile", model);
            }

            var emailExists = _context.Accounts
                .Any(a => a.Email == model.Email && a.AccountId != accountId);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View("Profile", model);
            }

            student.Account.FullName = model.FullName.Trim();
            student.Account.Username = model.Username.Trim();
            student.Account.Email = model.Email.Trim();
            student.Account.UpdatedAt = DateTime.Now;

            student.Phone = model.Phone.Trim();
            student.Address = model.Address?.Trim();
            student.Birthday = model.Birthday;

            _context.SaveChanges();

            HttpContext.Session.SetString("FullName", student.Account.FullName);
            HttpContext.Session.SetString("Username", student.Account.Username);

            TempData["Success"] = "Cập nhật hồ sơ thành công.";
            return RedirectToAction("Profile");
        }

        public IActionResult MyCourses()
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsStudent())
                return RedirectToAction("Index", "Home");

            var accountId = GetAccountId()!.Value;

            var student = _context.Students
                .FirstOrDefault(s => s.AccountId == accountId);

            if (student == null)
                return RedirectToAction("Index", "Home");

            var enrollments = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.EnrollmentStatus)
                .Where(e => e.StudentId == student.StudentId)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();

            return View(enrollments);
        }

        public IActionResult Learn(long enrollmentId, long? lessonId)
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsStudent())
                return RedirectToAction("Index", "Home");

            var accountId = GetAccountId()!.Value;

            var student = _context.Students
                .FirstOrDefault(s => s.AccountId == accountId);

            if (student == null)
                return RedirectToAction("Index", "Home");

            var enrollment = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.EnrollmentStatus)
                .FirstOrDefault(e =>
                    e.EnrollmentId == enrollmentId &&
                    e.StudentId == student.StudentId);

            if (enrollment == null)
                return NotFound();

            if (enrollment.EnrollmentStatus == null ||
                enrollment.EnrollmentStatus.StatusCode != "APPROVED")
            {
                TempData["Error"] = "Khóa học chưa được admin duyệt nên bạn chưa thể vào học.";
                return RedirectToAction("MyCourses");
            }

            if (string.IsNullOrWhiteSpace(enrollment.ClassId))
            {
                TempData["Error"] = "Bạn chưa được xếp lớp nên chưa có bài học.";
                return RedirectToAction("MyCourses");
            }

            var lessons = _context.Lessons
                .Where(l => l.ClassId == enrollment.ClassId)
                .OrderBy(l => l.OrderIndex)
                .ToList();

            var currentLesson = lessonId == null
                ? lessons.FirstOrDefault()
                : lessons.FirstOrDefault(l => l.LessonId == lessonId.Value);

            var vm = new LearnViewModel
            {
                Enrollment = enrollment,
                Lessons = lessons,
                CurrentLesson = currentLesson
            };

            return View(vm);
        }
    }
}