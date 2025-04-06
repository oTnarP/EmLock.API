using EmLock.API.Models;
using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public interface INotificationService
    {
        Task AddNotificationAsync(Notification notification);
        Task LogNotificationAsync(Notification notification);
        Task<List<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task QueueNotificationAsync(int? userId, string title, string message, string? type, string? deviceToken = null);

    }
}
