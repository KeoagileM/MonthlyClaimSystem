namespace ClaimSystem.Models
{
    public class HrReports
    {
        public DateTime ReportDate { get; set; } = DateTime.Now;
        public List<Claim> ApprovedClaims { get; set; } = new List<Claim>();
        public decimal TotalAmount { get; set; }
        public int TotalClaims { get; set; }
        public int TotalLecturers { get; set; }
    }

    public class LecturerUpdateModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
    }
}
