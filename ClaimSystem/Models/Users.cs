namespace ClaimSystem.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public static class Users
    {
        public static readonly Dictionary<string, User> Accounts = new()
        {
            {
                "lecturer",
                new User {
                    Username = "lecturer",
                    Password = "1234",
                    Role = "Lecturer"
                }
            },
            {
                "coordinator",
                new User {
                    Username = "coordinator",
                    Password = "5678",
                    Role = "Coordinator"
                }
            },
            {
                "manager",
                new User {
                    Username = "manager",
                    Password = "9999",
                    Role = "Manager"
                }
            }
        };
    }
}