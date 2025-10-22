using ClaimSystem.Models;

namespace ClaimSystemTests
{
    public class CoordinatorControllerTests
    {
        [Fact]
        public void CoordinatorUserProperties()
        {
            //Instance for the User class/model
            User user = new User();

            //Setting values for coordinator
            user.Username = "coordinator";
            user.Role = "Coordinator";

            //expected
            Assert.Equal("coordinator", user.Username);
            Assert.Equal("Coordinator", user.Role);
        }

        [Fact]
        public void CoordinatorClaimProperties()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //Setting values for coordinator
            claim.Status = "Under Review";
            claim.RejectionReason = "Needs more info";

            //expected
            Assert.Equal("Under Review", claim.Status);
            Assert.Equal("Needs more info", claim.RejectionReason);
        }
    }
}