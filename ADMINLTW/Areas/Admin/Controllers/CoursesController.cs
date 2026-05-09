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
    public class CoursesController : Controller
    {
        private readonly TrungTamLapTrinhContext _context;

        public CoursesController(TrungTamLapTrinhContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var courses = _context.Courses
                .Include(c => c.Level)
                .OrderByDescending(c => c.CourseId)
                .ToList();
            return View(courses);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseFormViewModel model)
        {
            if (_context.Courses.Any(c => c.CourseId == model.CourseId))
            {
                ModelState.AddModelError("CourseId", "Mã khóa học đã tồn tại");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(model.LevelId);
                return View(model);
            }

            var course = new Course
            {
                CourseId = model.CourseId,
                CourseName = model.CourseName,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                DurationMonths = model.DurationMonths,
                Roadmap = model.Roadmap,
                LevelId = model.LevelId,
                IsActive = model.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Thêm khóa học thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();

            var model = new CourseFormViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                Price = course.Price,
                DurationMonths = course.DurationMonths,
                Roadmap = course.Roadmap,
                LevelId = course.LevelId,
                IsActive = course.IsActive
            };

            LoadDropdowns(course.LevelId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CourseFormViewModel model)
        {
            var course = _context.Courses.Find(model.CourseId);
            if (course == null) return NotFound();

            if (!ModelState.IsValid)
            {
                LoadDropdowns(model.LevelId);
                return View(model);
            }

            course.CourseName = model.CourseName;
            course.Description = model.Description;
            course.ImageUrl = model.ImageUrl;
            course.Price = model.Price;
            course.DurationMonths = model.DurationMonths;
            course.Roadmap = model.Roadmap;
            course.LevelId = model.LevelId;
            course.IsActive = model.IsActive;

            _context.Courses.Update(course);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật khóa học thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var course = _context.Courses
                .Include(c => c.Level)
                .Include(c => c.CenterClasses)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null) return NotFound();

            return View(course);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var course = _context.Courses
                .Include(c => c.Level)
                .FirstOrDefault(c => c.CourseId == id);

            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Xóa khóa học thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActive(string id)
        {
            var course = _context.Courses.Find(id);
            if (course == null) return NotFound();
            
            course.IsActive = !course.IsActive;
            _context.Courses.Update(course);
            _context.SaveChanges();
            
            TempData["SuccessMessage"] = course.IsActive ? "Mở khóa khóa học thành công" : "Khóa khóa học thành công";
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(object? selectedValue = null)
        {
            ViewBag.LevelId = new SelectList(_context.CourseLevels.ToList(), "LevelId", "LevelName", selectedValue);
        }
    }
}