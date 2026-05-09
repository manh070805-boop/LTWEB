using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using qlkh.Data;
using qlkh.Filters;

namespace qlkh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorizeAttribute]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var accounts = _context.Accounts.AsNoTracking().ToList();
            var students = _context.Students.AsNoTracking().ToList();
            var teachers = _context.Teachers.AsNoTracking().ToList();
            var courses = _context.Courses
                .Include(c => c.CourseLevel)
                .AsNoTracking()
                .ToList();
            var classes = _context.CenterClasses
                .Include(c => c.Status)
                .AsNoTracking()
                .ToList();
            var enrollments = _context.Enrollments
                .Include(e => e.EnrollmentStatus)
                .Include(e => e.Course)
                .AsNoTracking()
                .ToList();
            var payments = _context.Payments
                .Include(p => p.Status)
                .AsNoTracking()
                .ToList();

            ViewBag.TotalAccounts = _context.Accounts.Count();
            ViewBag.TotalStudents = students.Count;
            ViewBag.TotalTeachers = teachers.Count;
            ViewBag.TotalCourses = courses.Count;
            ViewBag.TotalClasses = classes.Count;
            ViewBag.TotalEnrollments = enrollments.Count;
            ViewBag.TotalPayments = payments.Count;
            ViewBag.ActiveAccounts = accounts.Count(a => a.IsActive);
            ViewBag.ActiveClasses = classes.Count(c =>
                c.Status.StatusCode.Equals("ONGOING", StringComparison.OrdinalIgnoreCase));

            var receivedPayments = payments
                .Where(p => p.Status.StatusCode.Equals("PAID", StringComparison.OrdinalIgnoreCase)
                    || p.Status.StatusCode.Equals("PARTIAL", StringComparison.OrdinalIgnoreCase))
                .ToList();

            ViewBag.TotalRevenue = receivedPayments.Sum(p => p.Amount);
            ViewBag.PendingPayments = payments.Count(p =>
                p.Status.StatusCode.Equals("PENDING", StringComparison.OrdinalIgnoreCase)
                || p.Status.StatusCode.Equals("REVIEW", StringComparison.OrdinalIgnoreCase));

            var currentMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var monthStarts = Enumerable.Range(0, 6)
                .Select(offset => currentMonth.AddMonths(offset - 5))
                .ToList();

            var monthlyRevenueValues = monthStarts
                .Select(month => receivedPayments
                    .Where(p => p.PaidAt.HasValue
                        && p.PaidAt.Value.Year == month.Year
                        && p.PaidAt.Value.Month == month.Month)
                    .Sum(p => p.Amount))
                .ToArray();

            ViewBag.MonthlyRevenueChart = SerializeChart(
                monthStarts.Select(month => $"T{month.Month}/{month.Year}"),
                monthlyRevenueValues);

            ViewBag.EnrollmentStatusChart = SerializeChart(
                enrollments
                    .GroupBy(e => e.EnrollmentStatus.StatusName)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Key),
                enrollments
                    .GroupBy(e => e.EnrollmentStatus.StatusName)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Count()));

            ViewBag.CourseLevelChart = SerializeChart(
                courses
                    .GroupBy(c => c.CourseLevel.LevelName)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Key),
                courses
                    .GroupBy(c => c.CourseLevel.LevelName)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Count()));

            ViewBag.PaymentStatusChart = SerializeChart(
                payments
                    .GroupBy(p => p.Status.StatusName)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Key),
                payments
                    .GroupBy(p => p.Status.StatusName)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Count()));

            ViewBag.ClassStatusChart = SerializeChart(
                classes
                    .GroupBy(c => c.Status.StatusName)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Key),
                classes
                    .GroupBy(c => c.Status.StatusName)
                    .OrderByDescending(group => group.Count())
                    .Select(group => group.Count()));

            ViewBag.TopCourseChart = SerializeChart(
                enrollments
                    .GroupBy(e => e.Course.CourseName)
                    .OrderByDescending(group => group.Count())
                    .Take(5)
                    .Select(group => group.Key),
                enrollments
                    .GroupBy(e => e.Course.CourseName)
                    .OrderByDescending(group => group.Count())
                    .Take(5)
                    .Select(group => group.Count()));

            return View();
        }

        private static string SerializeChart<TValue>(
            IEnumerable<string> labels,
            IEnumerable<TValue> values)
        {
            return JsonSerializer.Serialize(new
            {
                labels = labels.ToArray(),
                values = values.ToArray()
            });
        }
    }
}
