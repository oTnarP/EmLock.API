namespace EmLock.API.Dtos;

public class OverdueCustomerDto
{
    public string IMEI { get; set; }
    public string Model { get; set; }
    public string OwnerName { get; set; }
    public string OwnerPhone { get; set; }
    public decimal TotalDueAmount { get; set; }
    public DateTime? NextDueDate { get; set; }
}
