using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qlkh.Data;
using qlkh.Models;
using qlkh.Models.ViewModels;

namespace qlkh.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsTeacher()
        {
            return HttpContext.Session.GetInt32("RoleId") == 2;
        }

        private int? GetAccountId()
        {
            return HttpContext.Session.GetInt32("AccountId");
        }

        private Teacher? GetCurrentTeacher()
        {
            var accountId = GetAccountId();

            if (accountId == null)
                return null;

            return _context.Teachers
                .Include(t => t.Account)
                .FirstOrDefault(t => t.AccountId == accountId.Value);
        }

        public IActionResult Home()
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsTeacher())
                return RedirectToAction("Index", "Home");

            var teacher = GetCurrentTeacher();

            if (teacher == null || teacher.Account == null)
                return RedirectToAction("Index", "Home");

            var classIds = _context.CenterClasses
                .Where(c => c.TeacherId == teacher.TeacherId)
                .Select(c => c.ClassId)
                .ToList();

            var classCount = classIds.Count;

            var lessonCount = _context.Lessons
                .Count(l => classIds.Contains(l.ClassId));

            var studentCount = _context.Enrollments
                .Count(e => e.ClassId != null && classIds.Contains(e.ClassId));

            var approvedStudentCount = _context.Enrollments
                .Include(e => e.EnrollmentStatus)
                .Count(e =>
                    e.ClassId != null &&
                    classIds.Contains(e.ClassId) &&
                    e.EnrollmentStatus != null &&
                    e.EnrollmentStatus.StatusCode == "APPROVED");

            decimal income = _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.EnrollmentStatus)
                .Where(e =>
                    e.ClassId != null &&
                    classIds.Contains(e.ClassId) &&
                    e.EnrollmentStatus != null &&
                    e.EnrollmentStatus.StatusCode == "APPROVED")
                .Sum(e => e.Course != null ? e.Course.Price : 0);

            ViewBag.TeacherName = teacher.Account.FullName;
            ViewBag.ClassCount = classCount;
            ViewBag.LessonCount = lessonCount;
            ViewBag.StudentCount = studentCount;
            ViewBag.ApprovedStudentCount = approvedStudentCount;
            ViewBag.Income = income;

            var recentClasses = _context.CenterClasses
                .Include(c => c.Course)
                .Include(c => c.Status)
                .Where(c => c.TeacherId == teacher.TeacherId)
                .OrderByDescending(c => c.CreatedAt)
                .Take(4)
                .ToList();

            return View(recentClasses);
        }

        public IActionResult MyClasses()
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsTeacher())
                return RedirectToAction("Index", "Home");

            var teacher = GetCurrentTeacher();

            if (teacher == null)
                return RedirectToAction("Index", "Home");

            var classes = _context.CenterClasses
                .Include(c => c.Course)
                .Include(c => c.Status)
                .Where(c => c.TeacherId == teacher.TeacherId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return View(classes);
        }

        public IActionResult Lessons(string classId)
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsTeacher())
                return RedirectToAction("Index", "Home");

            var teacher = GetCurrentTeacher();

            if (teacher == null)
                return RedirectToAction("Index", "Home");

            var centerClass = _context.CenterClasses
                .Include(c => c.Course)
                .FirstOrDefault(c => c.ClassId == classId && c.TeacherId == teacher.TeacherId);

            if (centerClass == null)
                return NotFound();

            var lessons = _context.Lessons
                .Where(l => l.ClassId == classId)
                .OrderBy(l => l.OrderIndex)
                .ToList();

            ViewBag.ClassId = centerClass.ClassId;
            ViewBag.ClassName = centerClass.ClassName;
            ViewBag.CourseName = centerClass.Course?.CourseName ?? "";

            return View(lessons);
        }

        [HttpPost]
        public IActionResult CreateLesson(string classId, string title, string? videoUrl, int orderIndex)
        {
            if (!IsTeacher())
                return RedirectToAction("Index", "Home");

            var teacher = GetCurrentTeacher();

            if (teacher == null)
                return RedirectToAction("Index", "Home");

            var ownsClass = _context.CenterClasses
                .Any(c => c.ClassId == classId && c.TeacherId == teacher.TeacherId);

            if (!ownsClass)
                return NotFound();

            if (string.IsNullOrWhiteSpace(title))
            {
                TempData["Error"] = "Tên bài học không được để trống.";
                return RedirectToAction("Lessons", new { classId });
            }

            var duplicate = _context.Lessons
                .Any(l => l.ClassId == classId && l.OrderIndex == orderIndex);

            if (duplicate)
            {
                TempData["Error"] = "Thứ tự bài học đã tồn tại.";
                return RedirectToAction("Lessons", new { classId });
            }

            var lesson = new Lesson
            {
                ClassId = classId,
                Title = title.Trim(),
                VideoUrl = videoUrl?.Trim(),
                OrderIndex = orderIndex
            };

            _context.Lessons.Add(lesson);
            _context.SaveChanges();

            TempData["Success"] = "Thêm video bài giảng thành công.";
            return RedirectToAction("Lessons", new { classId });
        }

        [HttpPost]
        public IActionResult UpdateLesson(long lessonId, string title, string? videoUrl, int orderIndex)
        {
            if (!IsTeacher())
                return RedirectToAction("Index", "Home");

            var teacher = GetCurrentTeacher();

            if (teacher == null)
                return RedirectToAction("Index", "Home");

            var lesson = _context.Lessons
                .Include(l => l.CenterClass)
                .FirstOrDefault(l => l.LessonId == lessonId);

            if (lesson == null || lesson.CenterClass == null)
                return NotFound();

            if (lesson.CenterClass.TeacherId != teacher.TeacherId)
                return NotFound();

            if (string.IsNullOrWhiteSpace(title))
            {
                TempData["Error"] = "Tên bài học không được để trống.";
                return RedirectToAction("Lessons", new { classId = lesson.ClassId });
            }

            var duplicate = _context.Lessons
                .Any(l =>
                    l.ClassId == lesson.ClassId &&
                    l.OrderIndex == orderIndex &&
                    l.LessonId != lessonId);

            if (duplicate)
            {
                TempData["Error"] = "Thứ tự bài học đã tồn tại.";
                return RedirectToAction("Lessons", new { classId = lesson.ClassId });
            }

            lesson.Title = title.Trim();
            lesson.VideoUrl = videoUrl?.Trim();
            lesson.OrderIndex = orderIndex;

            _context.SaveChanges();

            TempData["Success"] = "Cập nhật video bài giảng thành công.";
            return RedirectToAction("Lessons", new { classId = lesson.ClassId });
        }

        [HttpPost]
        public IActionResult DeleteLesson(long lessonId)
        {
            if (!IsTeacher())
                return RedirectToAction("Index", "Home");

            var teacher = GetCurrentTeacher();

            if (teacher == null)
                return RedirectToAction("Index", "Home");

            var lesson = _context.Lessons
                .Include(l => l.CenterClass)
                .FirstOrDefault(l => l.LessonId == lessonId);

            if (lesson == null || lesson.CenterClass == null)
                return NotFound();

            if (lesson.CenterClass.TeacherId != teacher.TeacherId)
                return NotFound();

            var classId = lesson.ClassId;

            _context.Lessons.Remove(lesson);
            _context.SaveChanges();

            TempData["Success"] = "Xóa bài học thành công.";
            return RedirectToAction("Lessons", new { classId });
        }

        public IActionResult Students()
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsTeacher())
                return RedirectToAction("Index", "Home");

            var teacher = GetCurrentTeacher();

            if (teacher == null)
                return RedirectToAction("Index", "Home");

            var enrollments = _context.Enrollments
                .Include(e => e.Student)
                    .ThenInclude(s => s!.Account)
                .Include(e => e.Course)
                .Include(e => e.EnrollmentStatus)
                .Include(e => e.CenterClass)
                .Where(e =>
                    e.ClassId != null &&
                    e.CenterClass != null &&
                    e.CenterClass.TeacherId == teacher.TeacherId)
                .OrderBy(e => e.ClassId)
                .ThenByDescending(e => e.CreatedAt)
                .ToList();

            return View(enrollments);
        }

        public IActionResult Profile()
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsTeacher())
                return RedirectToAction("Index", "Home");

            var teacher = GetCurrentTeacher();

            if (teacher == null || teacher.Account == null)
                return RedirectToAction("Index", "Home");

            var vm = new TeacherProfileViewModel
            {
                TeacherId = teacher.TeacherId,
                FullName = teacher.Account.FullName,
                Username = teacher.Account.Username,
                Email = teacher.Account.Email,
                Phone = teacher.Phone,
                Specialization = teacher.Specialization,
                Bio = teacher.Bio
            };

            return View(vm);
        }
        [HttpPost]
        public IActionResult UpdateProfile(TeacherProfileViewModel model)
        {
            if (GetAccountId() == null)
                return RedirectToAction("Login", "Account");

            if (!IsTeacher())
                return RedirectToAction("Index", "Home");

            var teacher = GetCurrentTeacher();

            if (teacher == null || teacher.Account == null)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View("Profile", model);

            var usernameExists = _context.Accounts
                .Any(a => a.Username == model.Username && a.AccountId != teacher.AccountId);

            if (usernameExists)
            {
                ModelState.AddModelError("Username", "Username đã tồn tại.");
                return View("Profile", model);
            }

            var emailExists = _context.Accounts
                .Any(a => a.Email == model.Email && a.AccountId != teacher.AccountId);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View("Profile", model);
            }

            teacher.Account.FullName = model.FullName.Trim();
            teacher.Account.Username = model.Username.Trim();
            teacher.Account.Email = model.Email.Trim();
            teacher.Account.UpdatedAt = DateTime.Now;

            teacher.Phone = model.Phone.Trim();
            teacher.Specialization = model.Specialization?.Trim();
            teacher.Bio = model.Bio?.Trim();

            _context.SaveChanges();

            HttpContext.Session.SetString("FullName", teacher.Account.FullName);
            HttpContext.Session.SetString("Username", teacher.Account.Username);

            TempData["Success"] = "Cập nhật hồ sơ thành công.";
            return RedirectToAction("Profile");
        }
    }
}
