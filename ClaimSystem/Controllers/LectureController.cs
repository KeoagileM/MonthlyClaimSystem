using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class LectureController : Controller
    {
        public static List<Claim> Claims = new List<Claim>();

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("Index", "Home");

            ViewBag.Modules = new List<string> { "PROG5121", "PRLD5121", "PROG6112", "WEDE5121" };
            return View(Claims ?? new List<Claim>());
        }

        [HttpPost]
        public IActionResult SubmitClaim(Claim claim)
        {
            if (HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("Index", "Home");

            if (claim != null)
            {
                claim.Id = Claims.Count + 1;
                claim.DateSubmitted = DateTime.Now;
                claim.Status = "Pending";
                claim.TotalAmount = claim.HoursWorked * claim.HourlyRate;

                if (string.IsNullOrEmpty(claim.DocumentPath))
                    claim.DocumentPath = "No Document";

                Claims.Add(claim);
            }

            return RedirectToAction("Dashboard");
        }
    }

}
