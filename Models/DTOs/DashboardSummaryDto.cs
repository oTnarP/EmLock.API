namespace EmLock.API.Dtos;

public class DashboardSummaryDto
{
    public int TotalDevices { get; set; }
    public int LockedDevices { get; set; }
    public int ActiveCustomers { get; set; }
    public decimal TotalPaidAmount { get; set; }
    public decimal TotalDueAmount { get; set; }
}
