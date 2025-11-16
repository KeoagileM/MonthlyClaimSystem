using ClaimSystem.Models;
using ClaimSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class LectureController : Controller
    {
        private readonly ClaimService _claimService;
        private readonly UserService _userService;

        public LectureController(ClaimService claimService, UserService userService)
        {
            _claimService = claimService;
            _userService = userService;
        }

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("AccessDenied", "Home");

            var username = HttpContext.Session.GetString("Username");
            var userClaims = await _claimService.GetClaimsByUserAsync(username);
            return View(userClaims);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitClaim(
            string module,
            DateTime dateSubmitted,
            decimal hourlyRate,
            int hoursWorked,
            IFormFile document)
        {
            if (HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("AccessDenied", "Home");

            try
            {
                if (hourlyRate <= 0 || hoursWorked <= 0)
                {
                    TempData["ErrorMessage"] = "Hourly rate and hours worked must be greater than zero.";
                    return RedirectToAction("Dashboard");
                }

                // Get the lecturer's registered information from session
                var firstName = HttpContext.Session.GetString("FirstName");
                var lastName = HttpContext.Session.GetString("LastName");
                var employeeNumber = HttpContext.Session.GetString("EmployeeNumber");
                var username = HttpContext.Session.GetString("Username");

                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(employeeNumber))
                {
                    // If session doesn't have information, get from database
                    var user = await _userService.GetUserByUsernameAsync(username);
                    if (user != null)
                    {
                        firstName = user.FirstName;
                        lastName = user.LastName;
                        employeeNumber = user.EmployeeNumber;
                        // Update session with the information
                        HttpContext.Session.SetString("FirstName", user.FirstName);
                        HttpContext.Session.SetString("LastName", user.LastName);
                        HttpContext.Session.SetString("EmployeeNumber", user.EmployeeNumber);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to retrieve your registered information. Please log in again.";
                        return RedirectToAction("Dashboard");
                    }
                }

                var lecturerName = $"{firstName} {lastName}";

                string filePath = null;
                if (document != null && document.Length > 0)
                {
                    if (document.Length > 10 * 1024 * 1024) // 10MB limit
                    {
                        TempData["ErrorMessage"] = "File size must be less than 10MB.";
                        return RedirectToAction("Dashboard");
                    }

                    // Create documents directory if it doesn't exist
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Generate unique file name
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(document.FileName)}";
                    var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file
                    using var stream = new FileStream(fullPath, FileMode.Create);
                    await document.CopyToAsync(stream);

                    filePath = $"/documents/{uniqueFileName}";
                }
                else
                {
                    TempData["ErrorMessage"] = "Please upload a supporting document.";
                    return RedirectToAction("Dashboard");
                }

                // Create and save the claim
                var claim = new Claim
                {
                    EmployeeNumber = employeeNumber,
                    LecturerName = lecturerName,
                    Module = module,
                    DateSubmitted = dateSubmitted == default ? DateTime.Today : dateSubmitted,
                    HourlyRate = hourlyRate,
                    HoursWorked = hoursWorked,
                    DocumentPath = filePath,
                    SubmittedBy = username
                };

                await _claimService.AddClaimAsync(claim);

                TempData["SuccessMessage"] = "Claim submitted successfully! Waiting for coordinator approval.";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error submitting claim: {ex.Message}";
                return RedirectToAction("Dashboard");
            }
        }
    }
}