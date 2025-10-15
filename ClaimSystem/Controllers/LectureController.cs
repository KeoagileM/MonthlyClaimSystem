using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class LectureController : Controller
    {
        // Shared in-memory store (replace with DB in real app)
        public static List<Claim> Claims { get; } = new List<Claim>();

        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Lecturer") return RedirectToAction("Index", "Home");

            // pass all claims (in production filter to logged-in lecturer)
            return View(Claims);
        }

        [HttpPost]
        public IActionResult SubmitClaim(
            string EmployeeNumber,
            string LecturerName,
            string Module,
            DateTime DateSubmitted,
            decimal HourlyRate,
            int HoursWorked,
            IFormFile Document)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Lecturer") return RedirectToAction("Index", "Home");

            string filePath = null;
            if (Document != null && Document.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(Document.FileName)}";
                var fullPath = Path.Combine(uploadsFolder, uniqueFileName);
                using var stream = new FileStream(fullPath, FileMode.Create);
                Document.CopyTo(stream);

                filePath = $"/Documents/{uniqueFileName}"; // relative to web root
            }

            var claim = new Claim
            {
                Id = Claims.Count + 1,
                EmployeeNumber = EmployeeNumber,
                LecturerName = LecturerName,
                Module = Module,
                DateSubmitted = DateSubmitted == default ? DateTime.Now : DateSubmitted,
                HourlyRate = HourlyRate,
                HoursWorked = HoursWorked,
                TotalAmount = HourlyRate * HoursWorked,
                DocumentPath = filePath,
                Status = "Pending",
                RejectionReason = null
            };

            Claims.Add(claim);
            return RedirectToAction("Dashboard");
        }
    }

}
