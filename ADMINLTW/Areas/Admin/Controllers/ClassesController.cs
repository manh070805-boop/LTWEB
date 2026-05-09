using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrungTamLapTrinh.Web.Areas.Admin.ViewModels;
using TrungTamLapTrinh.Web.Data;
using TrungTamLapTrinh.Web.Filters;
using TrungTamLapTrinh.Web.Models;

namespace TrungTamLapTrinh.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class ClassesController : Controller
    {
        private readonly TrungTamLapTrinhContext _context;

        public ClassesController(TrungTamLapTrinhContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var classes = _context.CenterClasses
                .Include(c => c.Course)
                .Include(c => c.Status)
                .Include(c => c.Teacher.Account)
                .OrderByDescending(c => c.ClassId)
                .ToList();
            return View(classes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CenterClassFormViewModel model)
        {
            if (_context.CenterClasses.Any(c => c.ClassId == model.ClassId))
            {
                ModelState.AddModelError("ClassId", "Mã lớp này đã tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(model.CourseId, model.TeacherId, model.StatusId);
                return View(model);
            }

            var centerClass = new CenterClass
            {
                ClassId = model.ClassId,
                ClassName = model.ClassName,
                RoomName = model.RoomName,
                CourseId = model.CourseId,
                TeacherId = model.TeacherId,
                StatusId = model.StatusId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                MaxStudents = model.MaxStudents,
                CreatedAt = DateTime.Now
            };

            _context.CenterClasses.Add(centerClass);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Thêm lớp học thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var centerClass = _context.CenterClasses.Find(id);
            if (centerClass == null)
                return NotFound();

            var model = new CenterClassFormViewModel
            {
                ClassId = centerClass.ClassId,
                ClassName = centerClass.ClassName,
                RoomName = centerClass.RoomName,
                CourseId = centerClass.CourseId,
                TeacherId = centerClass.TeacherId,
                StatusId = centerClass.StatusId,
                StartDate = centerClass.StartDate,
                EndDate = centerClass.EndDate,
                MaxStudents = centerClass.MaxStudents
            };

            LoadDropdowns(centerClass.CourseId, centerClass.TeacherId, centerClass.StatusId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CenterClassFormViewModel model)
        {
            var centerClass = _context.CenterClasses.Find(model.ClassId);
            if (centerClass == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                LoadDropdowns(model.CourseId, model.TeacherId, model.StatusId);
                return View(model);
            }

            centerClass.ClassName = model.ClassName;
            centerClass.RoomName = model.RoomName;
            centerClass.CourseId = model.CourseId;
            centerClass.TeacherId = model.TeacherId;
            centerClass.StatusId = model.StatusId;
            centerClass.StartDate = model.StartDate;
            centerClass.EndDate = model.EndDate;
            centerClass.MaxStudents = model.MaxStudents;

            _context.CenterClasses.Update(centerClass);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật lớp học thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var centerClass = _context.CenterClasses
                .Include(c => c.Course)
                .Include(c => c.Status)
                .Include(c => c.Teacher.Account)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Student.Account)
                .FirstOrDefault(m => m.ClassId == id);

            if (centerClass == null)
                return NotFound();

            return View(centerClass);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var centerClass = _context.CenterClasses
                .Include(c => c.Course)
                .Include(c => c.Status)
                .Include(c => c.Teacher.Account)
                .FirstOrDefault(m => m.ClassId == id);

            if (centerClass == null)
                return NotFound();

            return View(centerClass);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var centerClass = _context.CenterClasses.Find(id);
            if (centerClass != null)
            {
                bool hasEnrollments = _context.Enrollments.Any(e => e.ClassId == id);
                bool hasSchedules = _context.Schedules.Any(s => s.ClassId == id);

                if (hasEnrollments || hasSchedules)
                {
                    TempData["ErrorMessage"] = "Không thể xóa lớp học này vì đang có dữ liệu liên quan (Học viên, Lịch học).";
                    return RedirectToAction(nameof(Index));
                }

                _context.CenterClasses.Remove(centerClass);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Xóa lớp học thành công!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(object? courseId = null, object? teacherId = null, object? statusId = null)
        {
            ViewBag.CourseId = new SelectList(_context.Courses.ToList(), "CourseId", "CourseName", courseId);
            
            var teachers = _context.Teachers
                .Include(t => t.Account)
                .Select(t => new { t.TeacherId, DisplayText = t.Account.FullName + " (" + t.TeacherId + ")" })
                .ToList();
            ViewBag.TeacherId = new SelectList(teachers, "TeacherId", "DisplayText", teacherId);

            ViewBag.StatusId = new SelectList(_context.ClassStatuses.ToList(), "StatusId", "StatusName", statusId);
        }
    }
}