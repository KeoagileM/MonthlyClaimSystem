using ClaimSystem.Models;
using ClaimSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class LectureController : Controller
    {
        private readonly ClaimService _claimService;

        public LectureController(ClaimService claimService)
        {
            _claimService = claimService;
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("AccessDenied", "Home");

            var username = HttpContext.Session.GetString("Username");
            var userClaims = _claimService.GetClaimsByUser(username);
            return View(userClaims);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(
            string employeeNumber,
            string lecturerName,
            string module,
            DateTime dateSubmitted,
            decimal hourlyRate,
            int hoursWorked,
            IFormFile document)
        {
            if (HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("AccessDenied", "Home");

            if (string.IsNullOrWhiteSpace(employeeNumber) || string.IsNullOrWhiteSpace(lecturerName))
            {
                TempData["ErrorMessage"] = "Employee number and lecturer name are required.";
                return RedirectToAction("Dashboard");
            }

            if (hourlyRate <= 0 || hoursWorked <= 0)
            {
                TempData["ErrorMessage"] = "Hourly rate and hours worked must be greater than zero.";
                return RedirectToAction("Dashboard");
            }

            string filePath = null;
            if (document != null && document.Length > 0)
            {
                if (document.Length > 10 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "File size must be less than 10MB.";
                    return RedirectToAction("Dashboard");
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(document.FileName)}";
                var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await document.CopyToAsync(stream);

                filePath = $"/documents/{uniqueFileName}";
            }
            else
            {
                TempData["ErrorMessage"] = "Please upload a supporting document.";
                return RedirectToAction("Dashboard");
            }

            var claim = new Claim
            {
                EmployeeNumber = employeeNumber,
                LecturerName = lecturerName,
                Module = module,
                DateSubmitted = dateSubmitted == default ? DateTime.Today : dateSubmitted,
                HourlyRate = hourlyRate,
                HoursWorked = hoursWorked,
                DocumentPath = filePath,
                SubmittedBy = HttpContext.Session.GetString("Username")
            };

            _claimService.AddClaim(claim);

            TempData["SuccessMessage"] = "Claim submitted successfully! Waiting for coordinator approval.";
            return RedirectToAction("Dashboard");
        }
    }
}