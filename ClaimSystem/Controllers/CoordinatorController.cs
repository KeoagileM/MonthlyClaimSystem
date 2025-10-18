using ClaimSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly ClaimService _claimService;

        public CoordinatorController(ClaimService claimService)
        {
            _claimService = claimService;
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Coordinator")
                return RedirectToAction("AccessDenied", "Home");

            ViewBag.PendingCount = _claimService.GetPendingClaimsCount();
            ViewBag.ApprovedCount = _claimService.GetCoordinatorApprovedCount();

            return View(_claimService.GetAllClaims());
        }

        [HttpPost]
        public IActionResult Approve(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Coordinator")
                return RedirectToAction("AccessDenied", "Home");

            if (_claimService.UpdateClaimStatus(id, "Coordinator Approved"))
            {
                TempData["SuccessMessage"] = "Claim approved successfully! Sent to manager for final approval.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to approve claim.";
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Disapprove(int id, string rejectionReason)
        {
            if (HttpContext.Session.GetString("Role") != "Coordinator")
                return RedirectToAction("AccessDenied", "Home");

            if (string.IsNullOrWhiteSpace(rejectionReason))
            {
                TempData["ErrorMessage"] = "Please provide a reason for disapproval.";
                return RedirectToAction("Dashboard");
            }

            if (_claimService.UpdateClaimStatus(id, "Coordinator Disapproved", rejectionReason))
            {
                TempData["SuccessMessage"] = "Claim disapproved successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to disapprove claim.";
            }

            return RedirectToAction("Dashboard");
        }
    }
}