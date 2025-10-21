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

        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("AccessDenied", "Home");

            ViewBag.TotalAccepted = await _claimService.GetTotalAcceptedAmountAsync();
            ViewBag.PendingCount = await _claimService.GetPendingClaimsCountAsync();
            ViewBag.ApprovedCount = await _claimService.GetCoordinatorApprovedCountAsync();
            ViewBag.TotalClaims = await _claimService.GetTotalClaimsCountAsync();

            var claims = await _claimService.GetAllClaimsAsync();
            return View(claims);
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("AccessDenied", "Home");

            var claim = await _claimService.GetClaimByIdAsync(id);
            if (claim != null && claim.Status == "Coordinator Approved")
            {
                if (await _claimService.UpdateClaimStatusAsync(id, "Accepted"))
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
                TempData["ErrorMessage"] = "Claim must be coordinator approved before final acceptance.";
            }

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string rejectionReason)
        {
            if (HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("AccessDenied", "Home");

            var claim = await _claimService.GetClaimByIdAsync(id);
            if (claim != null && claim.Status == "Coordinator Approved")
            {
                if (await _claimService.UpdateClaimStatusAsync(id, "Rejected", rejectionReason))
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