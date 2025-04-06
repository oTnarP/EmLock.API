using System;

namespace EmLock.API.Models
{
    public class PushNotification
    {
        public int Id { get; set; }
        public int? UserId { get; set; } // optional if linked to a user
        public string Title { get; set; }
        public string Message { get; set; }
        public string TargetToken { get; set; } // FCM device token
        public bool IsSent { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }
    }
}
