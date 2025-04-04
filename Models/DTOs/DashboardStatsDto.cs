namespace EmLock.API.Models.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalDevices { get; set; }
        public int LockedDevices { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalDueAmount { get; set; }
    }

}
