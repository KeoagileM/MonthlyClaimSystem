namespace ClaimSystem.Models
{
    public static class Users
    {
        public static readonly Dictionary<string, (string Password, string Role)> Accounts =
            new Dictionary<string, (string, string)>
            {
                { "lecturer", ("1234", "Lecturer") },
                { "coordinator", ("5678", "Coordinator") },
                { "manager", ("9999", "Manager") }
            };
    }
}
