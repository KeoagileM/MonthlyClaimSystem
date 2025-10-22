using ClaimSystem.Models;

namespace ClaimSystemTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void ModelsExist()
        {
            //Create instances
            User user = new User();
            Claim claim = new Claim();

            //expected - both should exist
            Assert.NotNull(user);
            Assert.NotNull(claim);
        }

        [Fact]
        public void UserHasProperties()
        {
            //Instance for the User class/model
            User user = new User();

            //Setting values
            user.Id = 5;
            user.Username = "homeuser";

            //expected
            Assert.Equal(5, user.Id);
            Assert.Equal("homeuser", user.Username);
        }

        [Fact]
        public void ClaimHasProperties()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //Setting values
            claim.Id = 10;
            claim.SubmittedBy = "homelecturer";

            //expected
            Assert.Equal(10, claim.Id);
            Assert.Equal("homelecturer", claim.SubmittedBy);
        }
    }
}