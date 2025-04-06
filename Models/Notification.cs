using System;

namespace EmLock.API.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public string? DeviceToken { get; set; } // optional for Firebase targeting
        public string Status { get; set; } = "Queued"; // e.g., Queued, Sent, Failed
        public string Type { get; set; } = "Push";     // e.g., Push, Email, SMS

    }
}
