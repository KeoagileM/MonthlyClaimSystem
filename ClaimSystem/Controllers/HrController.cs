using ClaimSystem.Models;
using ClaimSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class HrController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly UserService _userService;

        public HrController(ClaimService claimService, UserService userService)
        {
            _claimService = claimService;
            _userService = userService;
        }

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "HR")
                return RedirectToAction("AccessDenied", "Home");

            var report = await _claimService.GenerateHrReportAsync();
            var lecturers = await _userService.GetAllLecturersAsync();

            ViewBag.TotalLecturers = lecturers.Count;
            ViewBag.TotalApprovedAmount = report.TotalAmount;
            ViewBag.TotalApprovedClaims = report.TotalClaims;

            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(DateTime startDate, DateTime endDate)
        {
            if (HttpContext.Session.GetString("Role") != "HR")
                return RedirectToAction("AccessDenied", "Home");

            var report = await _claimService.GenerateHrReportAsync(startDate, endDate);
            ViewBag.TotalLecturers = (await _userService.GetAllLecturersAsync()).Count;
            ViewBag.TotalApprovedAmount = report.TotalAmount;
            ViewBag.TotalApprovedClaims = report.TotalClaims;

            return View("Dashboard", report);
        }

        public async Task<IActionResult> LecturerManagement()
        {
            if (HttpContext.Session.GetString("Role") != "HR")
                return RedirectToAction("AccessDenied", "Home");

            var lecturers = await _userService.GetAllLecturersAsync();
            return View(lecturers);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLecturer(LecturerUpdateModel lecturer)
        {
            if (HttpContext.Session.GetString("Role") != "HR")
                return RedirectToAction("AccessDenied", "Home");

            if (await _userService.UpdateLecturerAsync(lecturer))
            {
                TempData["SuccessMessage"] = "Lecturer information updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update lecturer information.";
            }

            return RedirectToAction("LecturerManagement");
        }

        public async Task<IActionResult> GenerateInvoiceReport()
        {
            if (HttpContext.Session.GetString("Role") != "HR")
                return RedirectToAction("AccessDenied", "Home");

            // Get recent claims for the view
            var report = await _claimService.GenerateHrReportAsync(DateTime.Today.AddMonths(-1), DateTime.Today);
            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> DownloadInvoiceReport(DateTime startDate, DateTime endDate)
        {
            if (HttpContext.Session.GetString("Role") != "HR")
                return RedirectToAction("AccessDenied", "Home");

            var report = await _claimService.GenerateHrReportAsync(startDate, endDate);

            // Pass the dates to the view
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            return View("InvoiceReport", report);
        }

        public IActionResult InvoiceReport()
        {
            if (HttpContext.Session.GetString("Role") != "HR")
                return RedirectToAction("AccessDenied", "Home");

            return View();
        }

        // Add this method for Excel export (placeholder)
        public IActionResult ExportToExcel(DateTime startDate, DateTime endDate)
        {
            if (HttpContext.Session.GetString("Role") != "HR")
                return RedirectToAction("AccessDenied", "Home");

            // In a real application, you would generate an Excel file here
            // For now, just redirect back with a message
            TempData["InfoMessage"] = "Excel export feature would be implemented here. Selected period: " + startDate.ToString("MMM dd, yyyy") + " to " + endDate.ToString("MMM dd, yyyy");
            return RedirectToAction("Dashboard");
        }
    }
}