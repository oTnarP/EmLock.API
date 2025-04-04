using System;

namespace EmLock.API.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ➕ Add this:
        public int UserId { get; set; }
    }
}
