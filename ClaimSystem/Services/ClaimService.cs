using ClaimSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClaimSystem.Services
{
    public class ClaimService
    {
        private readonly List<Claim> _claims = new();
        private int _nextId = 1;

        public IEnumerable<Claim> GetAllClaims() => _claims.AsReadOnly();

        public IEnumerable<Claim> GetClaimsByUser(string username)
        {
            return _claims.Where(c => c.SubmittedBy == username);
        }

        public IEnumerable<Claim> GetPendingClaims()
        {
            return _claims.Where(c => c.Status == "Pending");
        }

        public IEnumerable<Claim> GetCoordinatorApprovedClaims()
        {
            return _claims.Where(c => c.Status == "Coordinator Approved");
        }

        public Claim GetClaimById(int id)
        {
            return _claims.FirstOrDefault(c => c.Id == id);
        }

        public void AddClaim(Claim claim)
        {
            claim.Id = _nextId++;
            claim.Status = "Pending";
            _claims.Add(claim);
        }

        public bool UpdateClaimStatus(int id, string status, string rejectionReason = null)
        {
            var claim = GetClaimById(id);
            if (claim != null)
            {
                claim.Status = status;
                if (!string.IsNullOrEmpty(rejectionReason))
                {
                    claim.RejectionReason = rejectionReason;
                }
                else if (status == "Coordinator Approved" || status == "Accepted")
                {
                    claim.RejectionReason = null;
                }
                return true;
            }
            return false;
        }

        public decimal GetTotalAcceptedAmount()
        {
            return _claims.Where(c => c.Status == "Accepted").Sum(c => c.TotalAmount);
        }

        public int GetPendingClaimsCount()
        {
            return _claims.Count(c => c.Status == "Pending");
        }

        public int GetCoordinatorApprovedCount()
        {
            return _claims.Count(c => c.Status == "Coordinator Approved");
        }

        public int GetTotalClaimsCount()
        {
            return _claims.Count;
        }
    }
}