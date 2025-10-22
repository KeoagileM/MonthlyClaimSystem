using ClaimSystem.Models;

namespace ClaimSystemTests
{
    public class ClaimServiceTests
    {
        [Fact]
        public void ClaimHasModule()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //Setting value
            claim.Module = "Programming 1A";

            //expected
            Assert.Equal("Programming 1A", claim.Module);
        }

        [Fact]
        public void ClaimHasEmployeeNumber()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //Setting value
            claim.EmployeeNumber = "12345";

            //expected
            Assert.Equal("12345", claim.EmployeeNumber);
        }

        [Fact]
        public void ClaimHasStatus()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //Setting value
            claim.Status = "Accepted";

            //expected
            Assert.Equal("Accepted", claim.Status);
        }
    }
}