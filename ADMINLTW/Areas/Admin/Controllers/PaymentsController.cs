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
    public class PaymentsController : Controller
    {
        private readonly TrungTamLapTrinhContext _context;

        public PaymentsController(TrungTamLapTrinhContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var payments = _context.Payments
                .Include(p => p.Enrollment.Student.Account)
                .Include(p => p.Enrollment.Course)
                .Include(p => p.Method)
                .Include(p => p.Status)
                .OrderByDescending(p => p.PaymentId)
                .ToList();
            return View(payments);
        }


        [HttpGet]
        public IActionResult Edit(long id)
        {
            var payment = _context.Payments.Find(id);
            if (payment == null)
                return NotFound();

            var model = new PaymentFormViewModel
            {
                PaymentId = payment.PaymentId,
                EnrollmentId = payment.EnrollmentId,
                Amount = payment.Amount,
                MethodId = payment.MethodId,
                StatusId = payment.StatusId,
                TransactionNo = payment.TransactionNo,
                PaidAt = payment.PaidAt
            };

            LoadDropdowns(payment.EnrollmentId, payment.MethodId, payment.StatusId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PaymentFormViewModel model)
        {
            var payment = _context.Payments.Find(model.PaymentId);
            if (payment == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                payment.EnrollmentId = model.EnrollmentId;
                payment.Amount = model.Amount;
                payment.MethodId = model.MethodId;
                payment.StatusId = model.StatusId;
                payment.TransactionNo = model.TransactionNo;
                payment.PaidAt = model.PaidAt;

                _context.Payments.Update(payment);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật thanh toán thành công!";
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(model.EnrollmentId, model.MethodId, model.StatusId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(long id)
        {
            var payment = _context.Payments
                .Include(p => p.Enrollment.Student.Account)
                .Include(p => p.Enrollment.Course)
                .Include(p => p.Method)
                .Include(p => p.Status)
                .FirstOrDefault(m => m.PaymentId == id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        [HttpGet]
        public IActionResult Delete(long id)
        {
            var payment = _context.Payments
                .Include(p => p.Enrollment.Student.Account)
                .Include(p => p.Enrollment.Course)
                .FirstOrDefault(m => m.PaymentId == id);

            if (payment == null)
                return NotFound();

            return View(payment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            var payment = _context.Payments.Find(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Xóa thanh toán thành công!";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private void LoadDropdowns(object? enrollmentId = null, object? methodId = null, object? statusId = null)
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Student.Account)
                .Include(e => e.Course)
                .Select(e => new { e.EnrollmentId, DisplayText = e.EnrollmentId + " - " + e.Student.Account.FullName + " (" + e.Course.CourseName + ")" })
                .ToList();
            ViewBag.EnrollmentId = new SelectList(enrollments, "EnrollmentId", "DisplayText", enrollmentId);

            ViewBag.MethodId = new SelectList(_context.PaymentMethods.ToList(), "MethodId", "MethodName", methodId);
            ViewBag.StatusId = new SelectList(_context.PaymentStatuses.ToList(), "StatusId", "StatusName", statusId);
        }
    }
}