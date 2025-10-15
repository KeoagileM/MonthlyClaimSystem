using Microsoft.AspNetCore.Mvc;
using ClaimSystem.Models;

namespace ClaimSystem.Controllers
{
    public class CoordinatorController : Controller
    {
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Coordinator")
                return RedirectToAction("Index", "Home");

            // show all claims (or filter as needed)
            return View(LectureController.Claims);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Coordinator")
                return RedirectToAction("Index", "Home");

            var claim = LectureController.Claims.FirstOrDefault(c => c.Id == id);
            if (claim != null && claim.Status == "Pending")
            {
                claim.Status = "Coordinator Approved";
                claim.RejectionReason = null;
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Disapprove(int id, string rejectionReason)
        {
            if (HttpContext.Session.GetString("Role") != "Coordinator")
                return RedirectToAction("Index", "Home");

            var claim = LectureController.Claims.FirstOrDefault(c => c.Id == id);
            if (claim != null && claim.Status == "Pending")
            {
                claim.Status = "Coordinator Disapproved";
                claim.RejectionReason = rejectionReason;
            }
            return RedirectToAction("Dashboard");
        }
    }
}
