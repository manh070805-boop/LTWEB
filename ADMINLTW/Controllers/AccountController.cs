using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrungTamLapTrinh.Web.Data;
using TrungTamLapTrinh.Web.ViewModels;

namespace TrungTamLapTrinh.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly TrungTamLapTrinhContext _context;
        public AccountController(TrungTamLapTrinhContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var user = _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefault(a => a.Username.ToLower() == model.Username.ToLower());

            if (user == null || user.PasswordHash != model.Password)
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
                return View(model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên.");
                return View(model);
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("Role", user.Role.RoleName);
            HttpContext.Session.SetString("AccountId", user.AccountId.ToString());
            
            if (user.Role.RoleName.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
