namespace EmLock.API.Models.DTOs
{
    public class EmiFilterDto
    {
        public bool? IsPaid { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DeviceId { get; set; }
        public int? UserId { get; set; }
    }
}
