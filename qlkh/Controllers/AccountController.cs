using Microsoft.AspNetCore.Mvc;
using qlkh.Data;

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
                    return RedirectToAction("Index", "Admin");

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

    }
}
