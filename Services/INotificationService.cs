using EmLock.API.Models;
using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public interface INotificationService
    {
        Task LogNotificationAsync(Notification notification);
        Task<List<Notification>> GetNotificationsByUserIdAsync(int userId);

    }
}
