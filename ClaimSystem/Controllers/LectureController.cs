using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class LectureController : Controller
    {
        // Static list so the data persists across requests
        private static List<Claim> claims = new List<Claim>
        {
            new Claim { Id = 1, DateSubmitted = DateTime.Now, HoursWorked = 5, HourlyRate = 200, Status = "Approved", DocumentPath = "TestDoc.pdf" }
        };

        // GET: /Lecture/Dashboard
        public ActionResult Dashboard()
        {
            // Always send a valid list
            return View(claims ?? new List<Claim>());
        }

        // POST: /Lecture/SubmitClaim
        [HttpPost]
        public ActionResult SubmitClaim(Claim claim)
        {
            if (claim != null)
            {
                claim.Id = claims.Count + 1;
                claim.DateSubmitted = DateTime.Now;
                claim.Status = "Pending";

                if (string.IsNullOrEmpty(claim.DocumentPath))
                    claim.DocumentPath = "No Document";

                claims.Add(claim);
            }

            return RedirectToAction("Dashboard");
        }
    }
}
