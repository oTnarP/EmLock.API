namespace EmLock.API.Models;

public class Device
{
    public int Id { get; set; }
    public string IMEI { get; set; }
    public string Model { get; set; }
    public string OwnerName { get; set; }
    public string OwnerPhone { get; set; }
    public bool IsLocked { get; set; } = false;
}
