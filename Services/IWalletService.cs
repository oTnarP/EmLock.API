using EmLock.API.Models;
using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public interface IWalletService
    {
        Task AddEarningsAsync(int dealerId, int adminId, decimal totalAmount, float dealerPercentage);
        Task<Wallet> GetWalletByDealerIdAsync(int dealerId);
        Task<List<WalletTransaction>> GetWalletTransactionsAsync(int dealerId);
        Task SaveChangesAsync();

    }
}
