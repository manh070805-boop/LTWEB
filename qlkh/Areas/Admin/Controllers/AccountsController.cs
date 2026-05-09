using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using qlkh.Data;
using qlkh.Models;
using qlkh.Filters;
using qlkh.Areas.Admin.ViewModels;

namespace qlkh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorizeAttribute]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            var accounts = _context.Accounts
                .Include(a => a.Role)
                .OrderByDescending(a => a.AccountId)
                .ToList();
            return View(accounts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadRoles();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AccountFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("Password", "Vui lòng nhập mật khẩu");
            }
            if (_context.Accounts.Any(x => x.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
            }
            if (_context.Accounts.Any(x => x.Email == model.Email))
            {
                ModelState.AddModelError("Email","Email đã tồn tại");
            }
            if (!ModelState.IsValid)
            {
                LoadRoles(model.RoleId);
                return View(model);
            }
            var accounts = new Account
            {
                Username = model.Username,
                PasswordHash = model.Password!,
                Email = model.Email,
                FullName = model.FullName,
                RoleId = model.RoleId,
                IsActive = model.IsActive,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };
            _context.Accounts.Add(accounts);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Thêm tài khoản thành công";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(long id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
                return NotFound();

            var model = new AccountFormViewModel
            {
                AccountId = account.AccountId,
                Username = account.Username,
                Password = null,
                Email = account.Email,
                FullName = account.FullName,
                RoleId = account.RoleId,
                IsActive = account.IsActive
            };
            LoadRoles(account.RoleId);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AccountFormViewModel model)
        {
            var account = _context.Accounts.Find(model.AccountId);
            if (account == null)
                return NotFound();
            if (_context.Accounts.Any(x => x.AccountId != model.AccountId && x.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
            }
            if (_context.Accounts.Any(x => x.AccountId != model.AccountId && x.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại");
            }
            if (!ModelState.IsValid)
            {
                LoadRoles(model.RoleId);
                return View(model);
            }
            account.Username = model.Username;
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                account.PasswordHash = model.Password;
            }
            account.Email = model.Email;
            account.FullName = model.FullName;
            account.RoleId = model.RoleId;
            account.IsActive = model.IsActive;
            account.UpdatedAt = DateTime.Now;
            _context.Accounts.Update(account);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Cập nhật tài khoản thành công";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(long id)
        {
            var account = _context.Accounts
                .Include(a => a.Role)
                .Include(a => a.Student)
                .FirstOrDefault(a => a.AccountId == id);

            if (account == null)
                return NotFound();
            
            return View(account);
        }
        [HttpGet]
        public IActionResult Delete(long id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)    
                return NotFound();
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(long id, IFormCollection form)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
                return NotFound();
            _context.Accounts.Remove(account);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Xóa tài khoản thành công";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActive(long id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
                return NotFound();
            account.IsActive = !account.IsActive;
            account.UpdatedAt = DateTime.Now;   
            _context.Accounts.Update(account);
            _context.SaveChanges();
            TempData["SuccessMessage"] = account.IsActive ? "Mở khóa tài khoản thành công" : "Khóa tài khoản thành công";
            return RedirectToAction(nameof(Index));
        }
        private void LoadRoles(object? selectedValue = null)
        {
            ViewBag.RoleId = new SelectList(_context.Roles.ToList(), "RoleId", "RoleName", selectedValue);
        }
    }
}
