using ClaimSystem.Models;

namespace ClaimSystemTests
{
    public class UserTests
    {
        [Fact]
        public void UserExists()
        {
            //Instance for the User class/model
            User user = new User();

            //expected - user should not be null
            Assert.NotNull(user);
        }

        [Fact]
        public void UserHasUsername()
        {
            //Instance for the User class/model
            User user = new User();

            //Setting value
            user.Username = "testuser";

            //expected
            Assert.Equal("testuser", user.Username);
        }

        [Fact]
        public void UserHasRole()
        {
            //Instance for the User class/model
            User user = new User();

            //Setting value
            user.Role = "Lecturer";

            //expected
            Assert.Equal("Lecturer", user.Role);
        }
    }
}