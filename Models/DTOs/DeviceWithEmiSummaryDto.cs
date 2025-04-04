public class DeviceWithEmiSummaryDto
{
    public string IMEI { get; set; }
    public string Model { get; set; }
    public string OwnerName { get; set; }
    public string OwnerPhone { get; set; }

    // EMI summary
    public int TotalEmis { get; set; }
    public decimal TotalAmount { get; set; }
    public int PaidCount { get; set; }
    public int UnpaidCount { get; set; }
    public DateTime? NextDueDate { get; set; }
    public bool IsLocked { get; set; }
}
