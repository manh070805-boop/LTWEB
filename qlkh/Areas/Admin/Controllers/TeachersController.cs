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
    public class TeachersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeachersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var teachers = _context.Teachers
                .Include(t => t.Account!)
                .ThenInclude(a => a.Role)
                .OrderByDescending(t => t.TeacherId)
                .ToList();

            return View(teachers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TeacherFormViewModel model)
        {
            if (_context.Teachers.Any(t => t.TeacherId == model.TeacherId))
            {
                ModelState.AddModelError("TeacherId", "Mã giảng viên đã tồn tại");
            }

            if (_context.Teachers.Any(t => t.AccountId == model.AccountId))
            {
                ModelState.AddModelError("AccountId", "Tài khoản này đã được gán cho giảng viên khác");
            }

            var account = _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefault(a => a.AccountId == model.AccountId);

            if (account == null)
            {
                ModelState.AddModelError("AccountId", "Tài khoản không tồn tại");
            }
            else if (account.Role?.RoleName != "Teacher")
            {
                ModelState.AddModelError("AccountId", "Chỉ được chọn tài khoản có quyền Teacher");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(model.AccountId);
                return View(model);
            }

            var teacher = new Teacher
            {
                TeacherId = model.TeacherId,
                AccountId = model.AccountId,
                Phone = model.Phone,
                Specialization = model.Specialization,
                Bio = model.Bio
            };

            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Thêm giảng viên thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher == null)
                return NotFound();

            var model = new TeacherFormViewModel
            {
                TeacherId = teacher.TeacherId,
                AccountId = teacher.AccountId,
                Phone = teacher.Phone,
                Specialization = teacher.Specialization,
                Bio = teacher.Bio
            };

            LoadDropdowns(model.AccountId, teacher.TeacherId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TeacherFormViewModel model)
        {
            var teacher = _context.Teachers.Find(model.TeacherId);
            if (teacher == null)
                return NotFound();

            if (_context.Teachers.Any(t => t.AccountId == model.AccountId && t.TeacherId != model.TeacherId))
            {
                ModelState.AddModelError("AccountId", "Tài khoản này đã được gán cho giảng viên khác");
            }

            var account = _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefault(a => a.AccountId == model.AccountId);

            if (account == null)
            {
                ModelState.AddModelError("AccountId", "Tài khoản không tồn tại");
            }
            else if (account.Role?.RoleName != "Teacher")
            {
                ModelState.AddModelError("AccountId", "Chỉ được chọn tài khoản có quyền Teacher");
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns(model.AccountId, model.TeacherId);
                return View(model);
            }

            teacher.AccountId = model.AccountId;
            teacher.Phone = model.Phone;
            teacher.Specialization = model.Specialization;
            teacher.Bio = model.Bio;

            _context.Teachers.Update(teacher);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Cập nhật giảng viên thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var teacher = _context.Teachers
                .Include(t => t.Account)
                .Include(t => t.Classes)
                    .ThenInclude(c => c.Course)
                .FirstOrDefault(m => m.TeacherId == id);

            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var teacher = _context.Teachers
                .Include(t => t.Account!)
                .ThenInclude(a => a.Role)
                .FirstOrDefault(t => t.TeacherId == id);

            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher == null)
                return NotFound();

            _context.Teachers.Remove(teacher);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Xóa giảng viên thành công";
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(object? selectedValue = null, string? currentTeacherId = null)
        {
            var teacherRole = _context.Roles.FirstOrDefault(r => r.RoleName == "Teacher");

            if (teacherRole == null)
            {
                ViewBag.AccountId = new SelectList(new List<object>(), "AccountId", "FullName");
                return;
            }

            var usedAccountIds = _context.Teachers
                .Where(t => currentTeacherId == null || t.TeacherId != currentTeacherId)
                .Select(t => t.AccountId)
                .ToList();

            var accounts = _context.Accounts
                .Where(a => a.RoleId == teacherRole.RoleId && !usedAccountIds.Contains(a.AccountId))
                .Select(a => new
                {
                    a.AccountId,
                    DisplayText = a.Username + " - " + a.FullName
                })
                .ToList();

            ViewBag.AccountId = new SelectList(accounts, "AccountId", "DisplayText", selectedValue);
        }
    }
}