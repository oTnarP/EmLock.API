using EmLock.API.Data;
using EmLock.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public class WalletService : IWalletService
    {
        private readonly DataContext _context;

        public WalletService(DataContext context)
        {
            _context = context;
        }

        public async Task AddEarningsAsync(int dealerId, int adminId, decimal totalAmount, float dealerPercentage)
        {
            var dealerAmount = totalAmount * (decimal)(dealerPercentage / 100);
            var adminAmount = totalAmount - dealerAmount;

            var dealerWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == dealerId);
            if (dealerWallet == null)
            {
                dealerWallet = new Wallet { DealerId = dealerId, Balance = 0 };
                _context.Wallets.Add(dealerWallet);
            }
            dealerWallet.Balance += dealerAmount;

            var adminWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == adminId);
            if (adminWallet == null)
            {
                adminWallet = new Wallet { DealerId = adminId, Balance = 0 };
                _context.Wallets.Add(adminWallet);
            }
            adminWallet.Balance += adminAmount;

            await _context.SaveChangesAsync();
        }
        public async Task DistributeEarningsAsync(int shopkeeperId, decimal emiAmount)
        {
            const decimal dealerPercentage = 0.30m; // 30%
            const decimal adminPercentage = 0.70m;

            var shopkeeper = await _context.Users.Include(u => u.Devices)
                                                 .FirstOrDefaultAsync(u => u.Id == shopkeeperId && u.Role == "Shopkeeper");

            if (shopkeeper == null) return;

            var dealer = await _context.Users.FirstOrDefaultAsync(u => u.Id == shopkeeper.DealerId && u.Role == "Dealer");
            if (dealer == null) return;

            var dealerWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == dealer.Id);
            if (dealerWallet != null)
            {
                dealerWallet.Balance += emiAmount * dealerPercentage;
            }

            var adminWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == 1); // assuming admin is dealer ID 1
            if (adminWallet != null)
            {
                adminWallet.Balance += emiAmount * adminPercentage;
            }

            await _context.SaveChangesAsync();
        }

    }
}
