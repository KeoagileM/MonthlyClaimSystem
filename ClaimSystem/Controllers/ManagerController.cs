using ClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("Index", "Home");

            var claims = LectureController.Claims;
            ViewBag.TotalAccepted = claims.Where(c => c.Status == "Accepted").Sum(c => c.TotalAmount);
            return View(claims);
        }

        [HttpPost]
        public IActionResult Accept(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("Index", "Home");

            var claim = LectureController.Claims.FirstOrDefault(c => c.Id == id);
            if (claim != null && claim.Status == "Coordinator Approved")
            {
                claim.Status = "Accepted";
            }
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Reject(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("Index", "Home");

            var claim = LectureController.Claims.FirstOrDefault(c => c.Id == id);
            if (claim != null && (claim.Status == "Coordinator Approved" || claim.Status == "Coordinator Disapproved"))
            {
                claim.Status = "Rejected";
            }
            return RedirectToAction("Dashboard");
        }
    }
}
