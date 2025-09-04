namespace ClaimSystem.Models
{
    public class Claim
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; }
        public int HoursWorked { get; set; }
        public decimal HourlyRate { get; set; }
        public string Status { get; set; }
        public string DocumentPath { get; set; }
    }
}
