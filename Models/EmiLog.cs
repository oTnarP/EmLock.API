using System;

namespace EmLock.API.Models
{
    public class EmiLog
    {
        public int Id { get; set; }
        public int EmiScheduleId { get; set; }
        public EmiSchedule EmiSchedule { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow; // when this EMI was paid
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;   // when this log was recorded
    }
}
