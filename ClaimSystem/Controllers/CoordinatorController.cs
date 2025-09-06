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

            return View(LectureController.Claims);
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            var claim = LectureController.Claims.FirstOrDefault(c => c.Id == id);
            if (claim != null && claim.Status == "Pending")
                claim.Status = "Coordinator Approved";

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Disapprove(int id)
        {
            var claim = LectureController.Claims.FirstOrDefault(c => c.Id == id);
            if (claim != null && claim.Status == "Pending")
                claim.Status = "Coordinator Disapproved";

            return RedirectToAction("Dashboard");
        }
    }
}
