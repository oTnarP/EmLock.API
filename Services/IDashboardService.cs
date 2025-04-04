using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public interface IDashboardService
    {
        Task<object> GetOverviewAsync();
    }
}
