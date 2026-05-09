using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using qlkh.Areas.Admin.ViewModels;
using qlkh.Data;
using qlkh.Filters;
using qlkh.Models;

namespace qlkh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorizeAttribute]
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Student.Account)
                .Include(e => e.Course)
                .Include(e => e.CenterClass)
                .Include(e => e.EnrollmentStatus)
                .OrderByDescending(e => e.EnrollmentId)
                .ToList();
            return View(enrollments);
        }


        [HttpGet]
        public IActionResult Edit(long id)
        {
            var enrollment = _context.Enrollments.Find(id);
            if (enrollment == null)
                return NotFound();

            var model = new EnrollmentFormViewModel
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                ClassId = enrollment.ClassId,
                StatusId = enrollment.StatusId,
                ApprovedByTeacherId = enrollment.ApprovedByTeacherId,
                ApprovedAt = enrollment.ApprovedAt,
                Note = enrollment.Note
            };

            LoadDropdowns(enrollment.StudentId, enrollment.CourseId, enrollment.ClassId, enrollment.StatusId, enrollment.ApprovedByTeacherId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EnrollmentFormViewModel model)
        {
            var enrollment = _context.Enrollments.Find(model.EnrollmentId);
            if (enrollment == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                enrollment.StudentId = model.StudentId;
                enrollment.CourseId = model.CourseId;
                enrollment.ClassId = model.ClassId;
                enrollment.StatusId = model.StatusId;
                enrollment.ApprovedByTeacherId = model.ApprovedByTeacherId;
                enrollment.ApprovedAt = model.ApprovedAt;
                enrollment.Note = model.Note;

                _context.Enrollments.Update(enrollment);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật ghi danh thành công!";
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(model.StudentId, model.CourseId, model.ClassId, model.StatusId, model.ApprovedByTeacherId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            var enrollment = _context.Enrollments
                .Include(e => e.Student.Account)
                .Include(e => e.Course)
                .Include(e => e.CenterClass)
                .Include(e => e.EnrollmentStatus)
                .Include(e => e.ApprovedByTeacher!.Account)
                .Include(e => e.Payments)
                .FirstOrDefault(m => m.EnrollmentId == id);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        [HttpGet]
        public IActionResult Delete(long id)
        {
            var enrollment = _context.Enrollments
                .Include(e => e.Student.Account)
                .Include(e => e.Course)
                .FirstOrDefault(m => m.EnrollmentId == id);

            if (enrollment == null)
                return NotFound();

            return View(enrollment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var enrollment = _context.Enrollments.Find(id);
            if (enrollment != null)
            {
                bool hasPayments = _context.Payments.Any(p => p.EnrollmentId == id);
                if (hasPayments)
                {
                    TempData["ErrorMessage"] = "Không thể xóa ghi danh này vì đã có thanh toán liên kết.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Enrollments.Remove(enrollment);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Xóa ghi danh thành công!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(object? studentId = null, object? courseId = null, object? classId = null, object? statusId = null, object? teacherId = null)
        {
            var students = _context.Students
                .Include(s => s.Account)
                .Select(s => new { s.StudentId, DisplayText = s.Account.FullName + " (" + s.StudentId + ")" })
                .ToList();
            ViewBag.StudentId = new SelectList(students, "StudentId", "DisplayText", studentId);

            ViewBag.CourseId = new SelectList(_context.Courses.ToList(), "CourseId", "CourseName", courseId);

            ViewBag.ClassId = new SelectList(_context.CenterClasses.ToList(), "ClassId", "ClassName", classId);

            ViewBag.StatusId = new SelectList(_context.EnrollmentStatuses.ToList(), "StatusId", "StatusName", statusId);

            var teachers = _context.Teachers
                .Include(t => t.Account)
                .Select(t => new { t.TeacherId, DisplayText = t.Account.FullName + " (" + t.TeacherId + ")" })
                .ToList();
            ViewBag.ApprovedByTeacherId = new SelectList(teachers, "TeacherId", "DisplayText", teacherId);
        }
    }
}