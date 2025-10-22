using ClaimSystem.Models;

namespace ClaimSystemTests
{
    public class ManagerControllerTests
    {
        [Fact]
        public void ManagerUserProperties()
        {
            //Instance for the User class/model
            User user = new User();

            //Setting values for manager
            user.Username = "manager";
            user.Role = "Manager";

            //expected
            Assert.Equal("manager", user.Username);
            Assert.Equal("Manager", user.Role);
        }

        [Fact]
        public void ManagerClaimProperties()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //Setting values for manager
            claim.Status = "Final Review";
            claim.DocumentPath = "/documents/file.pdf";

            //expected
            Assert.Equal("Final Review", claim.Status);
            Assert.Equal("/documents/file.pdf", claim.DocumentPath);
        }

        [Fact]
        public void ManagerUserCreation()
        {
            //Instance for the User class/model
            User user = new User();

            //Setting all basic properties
            user.Id = 100;
            user.Username = "manager100";
            user.PasswordHash = "managerpass";
            user.Role = "Manager";

            //expected
            Assert.Equal(100, user.Id);
            Assert.Equal("manager100", user.Username);
            Assert.Equal("managerpass", user.PasswordHash);
            Assert.Equal("Manager", user.Role);
        }
    }
}