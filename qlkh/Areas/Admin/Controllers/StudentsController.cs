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
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var students = _context.Students
                .Include(s => s.Account)
                .OrderByDescending(s => s.StudentId)
                .ToList();
            return View(students);
        }


        [HttpGet]
        public IActionResult Edit(string id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
                return NotFound();

            var model = new StudentFormViewModel
            {
                StudentId = student.StudentId,
                AccountId = student.AccountId,
                Phone = student.Phone,
                Address = student.Address,
                Birthday = student.Birthday
            };

            LoadDropdowns(student.AccountId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentFormViewModel model)
        {
            var student = _context.Students.Find(model.StudentId);
            if (student == null)
                return NotFound();

            if (_context.Students.Any(s => s.AccountId == model.AccountId && s.StudentId != model.StudentId))
            {
                ModelState.AddModelError("AccountId", "Tài khoản này đã được liên kết với một học viên khác.");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(model.AccountId);
                return View(model);
            }

            student.AccountId = model.AccountId;
            student.Phone = model.Phone;
            student.Address = model.Address;
            student.Birthday = model.Birthday;

            _context.Students.Update(student);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật học viên thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var student = _context.Students
                .Include(s => s.Account)
                .Include(s => s.Enrollments).ThenInclude(e => e.CenterClass).ThenInclude(c => c.Course)
                .Include(s => s.Enrollments).ThenInclude(e => e.Payments)
                .FirstOrDefault(m => m.StudentId == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var student = _context.Students
                .Include(s => s.Account)
                .FirstOrDefault(m => m.StudentId == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                bool hasEnrollments = _context.Enrollments.Any(e => e.StudentId == id);
                if (hasEnrollments)
                {
                    TempData["ErrorMessage"] = "Không thể xóa học viên này vì đã có đăng ký khóa học.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Students.Remove(student);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Xóa học viên thành công!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(object? selectedId = null)
        {
            var usedAccountIds = _context.Students
                .Where(s => selectedId == null || s.AccountId != (long)selectedId)
                .Select(s => s.AccountId)
                .ToList();

            var accounts = _context.Accounts
                .Include(a => a.Role)
                .Where(a => a.Role.RoleName == "Student" && !usedAccountIds.Contains(a.AccountId))
                .Select(a => new
                {
                    a.AccountId,
                    DisplayText = a.Username + " - " + a.FullName
                })
                .ToList();

            ViewBag.AccountId = new SelectList(accounts, "AccountId", "DisplayText", selectedId);
        }
    }
}
