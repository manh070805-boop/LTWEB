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
    public class SchedulesController : Controller
    {
        private readonly TrungTamLapTrinhContext _context;

        public SchedulesController(TrungTamLapTrinhContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var schedules = _context.Schedules
                .Include(s => s.Class)
                .OrderByDescending(s => s.ScheduleId)
                .ToList();
            return View(schedules);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ScheduleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var schedule = new Schedule
                {
                    ClassId = model.ClassId,
                    DayOfWeek = model.DayOfWeek,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    RoomName = model.RoomName
                };

                _context.Schedules.Add(schedule);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Thêm lịch học thành công!";
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(model.ClassId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var schedule = _context.Schedules.Find(id);
            if (schedule == null)
                return NotFound();

            var model = new ScheduleFormViewModel
            {
                ScheduleId = schedule.ScheduleId,
                ClassId = schedule.ClassId,
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                RoomName = schedule.RoomName
            };

            LoadDropdowns(schedule.ClassId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ScheduleFormViewModel model)
        {
            var schedule = _context.Schedules.Find(model.ScheduleId);
            if (schedule == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                schedule.ClassId = model.ClassId;
                schedule.DayOfWeek = model.DayOfWeek;
                schedule.StartTime = model.StartTime;
                schedule.EndTime = model.EndTime;
                schedule.RoomName = model.RoomName;

                _context.Schedules.Update(schedule);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật lịch học thành công!";
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(model.ClassId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            var schedule = _context.Schedules
                .Include(s => s.Class.Course)
                .Include(s => s.Class.Teacher.Account)
                .FirstOrDefault(m => m.ScheduleId == id);

            if (schedule == null)
                return NotFound();

            return View(schedule);
        }

        [HttpGet]
        public IActionResult Delete(long id)
        {
            var schedule = _context.Schedules
                .Include(s => s.Class)
                .FirstOrDefault(m => m.ScheduleId == id);

            if (schedule == null)
                return NotFound();

            return View(schedule);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var schedule = _context.Schedules.Find(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Xóa lịch học thành công!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(object? selectedId = null)
        {
            ViewBag.ClassId = new SelectList(_context.CenterClasses.ToList(), "ClassId", "ClassName", selectedId);
        }
    }
}