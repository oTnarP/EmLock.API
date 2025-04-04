namespace EmLock.API.Models.DTOs
{
    public class DeviceDto
    {
        public string IMEI { get; set; }
        public string Model { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public int UserId { get; set; }

    }
}
