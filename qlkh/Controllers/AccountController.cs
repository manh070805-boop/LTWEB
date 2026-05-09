using Microsoft.AspNetCore.Mvc;
using qlkh.Data;
using qlkh.Models;
using qlkh.Models.ViewModels;

namespace qlkh.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("AccountId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Accounts
            .FirstOrDefault(x => x.Username == username
                              && x.PasswordHash == password
                              && x.IsActive);

            if (user == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

            HttpContext.Session.SetInt32("AccountId", (int)user.AccountId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetInt32("RoleId", user.RoleId);

            switch (user.RoleId)
            {
                case 1: 
                    return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

                case 2: 
                    return RedirectToAction("Home", "Teacher");

                case 3: 
                    return RedirectToAction("Index", "Home");

                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Đăng xuất thành công.";
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult LogoutPost()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Đăng xuất thành công.";
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usernameExists = _context.Accounts
                .Any(a => a.Username == model.Username);

            if (usernameExists)
            {
                ModelState.AddModelError("Username", "Username đã tồn tại.");
                return View(model);
            }

            var emailExists = _context.Accounts
                .Any(a => a.Email == model.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return View(model);
            }

            var phoneExists = _context.Students
                .Any(s => s.Phone == model.Phone);

            if (phoneExists)
            {
                ModelState.AddModelError("Phone", "Số điện thoại đã tồn tại.");
                return View(model);
            }

            var studentRole = _context.Roles
                .FirstOrDefault(r => r.RoleName == "Student" || r.RoleName == "STUDENT");

            if (studentRole == null)
            {
                TempData["Error"] = "Hệ thống chưa cấu hình role Student.";
                return View(model);
            }

            var account = new Account
            {
                Username = model.Username.Trim(),
                PasswordHash = model.Password.Trim(), 
                Email = model.Email.Trim(),
                FullName = model.FullName.Trim(),
                RoleId = studentRole.RoleId,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Accounts.Add(account);
            _context.SaveChanges();

            var nextStudentNumber = _context.Students.Count() + 1;

            var student = new Student
            {
                StudentId = "HV" + nextStudentNumber.ToString("D3"),
                AccountId = account.AccountId,
                Phone = model.Phone.Trim(),
                Address = model.Address?.Trim(),
                Birthday = model.Birthday
            };

            _context.Students.Add(student);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký tài khoản thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Login", "Account");
        }

    }
}
