using ClaimSystem.Models;

namespace ClaimSystemTests
{
    public class LoginTests
    {
        [Fact]
        public void UserCanBeCreated()
        {
            //Instance for the User class/model
            User user = new User();

            //expected
            Assert.NotNull(user);
        }

        [Fact]
        public void UserHasPassword()
        {
            //Instance for the User class/model
            User user = new User();

            //Setting value
            user.PasswordHash = "password123";

            //expected
            Assert.Equal("password123", user.PasswordHash);
        }

        [Fact]
        public void UserHasAllBasicProperties()
        {
            //Instance for the User class/model
            User user = new User();

            //Setting values
            user.Username = "user1";
            user.PasswordHash = "pass1";
            user.Role = "Role1";

            //expected
            Assert.Equal("user1", user.Username);
            Assert.Equal("pass1", user.PasswordHash);
            Assert.Equal("Role1", user.Role);
        }
    }
}