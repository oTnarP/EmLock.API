namespace EmLock.API.Dtos
{
    public class MonthlyEmiStatDto
    {
        public string Month { get; set; }          // Example: "2025-04"
        public decimal TotalPaid { get; set; }     // Total paid EMI amount in that month
        public decimal TotalDue { get; set; }      // Total due EMI amount in that month
    }
}
