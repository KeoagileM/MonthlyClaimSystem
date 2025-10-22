using ClaimSystem.Models;

namespace ClaimSystemTests
{
    public class ClaimTests
    {
        [Fact]
        public void ClaimExists()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //expected - claim should not be null
            Assert.NotNull(claim);
        }

        [Fact]
        public void ClaimHasId()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //Setting value
            claim.Id = 1;

            //expected
            Assert.Equal(1, claim.Id);
        }

        [Fact]
        public void ClaimHasLecturerName()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //Setting value
            claim.LecturerName = "Test Lecturer";

            //expected
            Assert.Equal("Test Lecturer", claim.LecturerName);
        }
    }
}