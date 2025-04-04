using System;

namespace EmLock.API.Models
{
    public class DeviceActionLog
    {
        public int Id { get; set; }
        public string IMEI { get; set; }
        public string ActionType { get; set; } // "Lock" or "Unlock"
        public string PerformedBy { get; set; } // Username, role, or "System"
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
