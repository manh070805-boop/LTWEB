using Microsoft.AspNetCore.Mvc;
using qlkh.Data;

namespace qlkh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var featuredCourses = _context.Courses
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .Take(3)
                .ToList();

            return View(featuredCourses);
        }
    }
}