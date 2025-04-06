using EmLock.API.Data;
using EmLock.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly DataContext _context;

        public NotificationService(DataContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task LogNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        // 🔔 New: Queue Firebase Push Notification (to be picked by background worker)
        public async Task QueueNotificationAsync(int? userId, string title, string message, string? type, string? deviceToken = null)
        {
            var notif = new Notification
            {
                UserId = userId ?? 0,
                Title = title,
                Message = message,
                DeviceToken = deviceToken,
                CreatedAt = DateTime.UtcNow,
                Status = "Queued",
                Type = type ?? "Push"
            };


            _context.Notifications.Add(notif);
            await _context.SaveChangesAsync();
        }
    }
}
