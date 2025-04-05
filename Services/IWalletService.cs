using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public interface IWalletService
    {
        Task AddEarningsAsync(int dealerId, int adminId, decimal totalAmount, float dealerPercentage);
    }
}
