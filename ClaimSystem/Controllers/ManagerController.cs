using ClaimSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClaimSystem.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ClaimService _claimService;

        public ManagerController(ClaimService claimService)
        {
            _claimService = claimService;
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("AccessDenied", "Home");

            ViewBag.TotalAccepted = _claimService.GetTotalAcceptedAmount();
            ViewBag.PendingCount = _claimService.GetPendingClaimsCount();
            ViewBag.ApprovedCount = _claimService.GetCoordinatorApprovedCount();
            ViewBag.TotalClaims = _claimService.GetTotalClaimsCount();

            return View(_claimService.GetAllClaims());
        }

        [HttpPost]
        public IActionResult Accept(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("AccessDenied", "Home");

            var claim = _claimService.GetClaimById(id);
            if (claim != null && claim.Status == "Coordinator Approved")
            {
                if (_claimService.UpdateClaimStatus(id, "Accepted"))
                {
                    TempData["SuccessMessage"] = "Claim accepted successfully! Payment will be processed.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to accept claim.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Only coordinator-approved claims can be accepted.";
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult Reject(int id, string rejectionReason)
        {
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("AccessDenied", "Home");

            var claim = _claimService.GetClaimById(id);
            if (claim != null && claim.Status == "Coordinator Approved")
            {
                if (_claimService.UpdateClaimStatus(id, "Rejected", rejectionReason))
                {
                    TempData["SuccessMessage"] = "Claim rejected successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to reject claim.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Only coordinator-approved claims can be rejected.";
            }

            return RedirectToAction("Dashboard");
        }
    }
}