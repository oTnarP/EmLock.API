namespace EmLock.API.Models.DTOs
{
    public class EmiScheduleDto
    {
        public int DeviceId { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
    }
}
