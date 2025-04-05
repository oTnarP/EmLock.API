using System;

namespace EmLock.API.Models
{
    public class EmiLog
    {
        public int Id { get; set; }
        public int EmiScheduleId { get; set; }
        public EmiSchedule EmiSchedule { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;  // 🆕 Add this
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // 🆕 And this
    }
}
