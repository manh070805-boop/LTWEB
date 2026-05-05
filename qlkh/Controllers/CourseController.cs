using Microsoft.AspNetCore.Mvc;
using qlkh.Data;

namespace qlkh.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var courses = _context.Courses
                .Where(c => c.IsActive)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            return View(courses);
        }

        public IActionResult Details(string id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
    }
}