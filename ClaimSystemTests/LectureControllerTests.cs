using ClaimSystem.Models;

namespace ClaimSystemTests
{
    public class LectureControllerTests
    {
        [Fact]
        public void LectureUserProperties()
        {
            //Instance for the User class/model
            User user = new User();

            //Setting values for lecture
            user.Username = "lecturer";
            user.Role = "Lecturer";

            //expected
            Assert.Equal("lecturer", user.Username);
            Assert.Equal("Lecturer", user.Role);
        }

        [Fact]
        public void LectureClaimProperties()
        {
            //Instance for the Claim class/model
            Claim claim = new Claim();

            //Setting values for lecture
            claim.LecturerName = "Dr. Smith";
            claim.Module = "Computer Science";

            //expected
            Assert.Equal("Dr. Smith", claim.LecturerName);
            Assert.Equal("Computer Science", claim.Module);
        }
    }
}