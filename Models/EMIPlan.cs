namespace EmLock.API.Models
{
    public class EMIPlan
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int DurationInMonths { get; set; }
        public DateTime StartDate { get; set; }
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
}
