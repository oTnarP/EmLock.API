using EmLock.API.Data;
using EmLock.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
                await _context.SaveChangesAsync();
            }
            dealerWallet.Balance += dealerAmount;

            _context.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = dealerWallet.Id,
                Type = "Credit",
                Amount = dealerAmount,
                Description = "Dealer share from EMI payment",
                Timestamp = DateTime.UtcNow
            });

            var adminWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == adminId);
            if (adminWallet == null)
            {
                adminWallet = new Wallet { DealerId = adminId, Balance = 0 };
                _context.Wallets.Add(adminWallet);
                await _context.SaveChangesAsync();
            }
            adminWallet.Balance += adminAmount;

            _context.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = adminWallet.Id,
                Type = "Credit",
                Amount = adminAmount,
                Description = "Admin share from EMI payment",
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task DistributeEarningsAsync(int shopkeeperId, decimal emiAmount)
        {
            const decimal dealerPercentage = 0.30m;
            const decimal adminPercentage = 0.70m;

            var shopkeeper = await _context.Users
                .Include(u => u.Devices)
                .FirstOrDefaultAsync(u => u.Id == shopkeeperId && u.Role == "Shopkeeper");

            if (shopkeeper == null) return;

            var dealer = await _context.Users.FirstOrDefaultAsync(u => u.Id == shopkeeper.DealerId && u.Role == "Dealer");
            if (dealer == null) return;

            var dealerWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == dealer.Id);
            if (dealerWallet == null)
            {
                dealerWallet = new Wallet { DealerId = dealer.Id, Balance = 0 };
                _context.Wallets.Add(dealerWallet);
                await _context.SaveChangesAsync();
            }

            var dealerAmount = emiAmount * dealerPercentage;
            dealerWallet.Balance += dealerAmount;

            _context.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = dealerWallet.Id,
                Type = "Credit",
                Amount = dealerAmount,
                Description = "Dealer share from EMI payment",
                Timestamp = DateTime.UtcNow
            });

            var adminWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == 1); // assuming admin = dealerId 1
            if (adminWallet == null)
            {
                adminWallet = new Wallet { DealerId = 1, Balance = 0 };
                _context.Wallets.Add(adminWallet);
                await _context.SaveChangesAsync();
            }

            var adminAmount = emiAmount * adminPercentage;
            adminWallet.Balance += adminAmount;

            _context.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = adminWallet.Id,
                Type = "Credit",
                Amount = adminAmount,
                Description = "Admin share from EMI payment",
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task<Wallet> GetWalletByDealerIdAsync(int dealerId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == dealerId);
        }

        public async Task<List<WalletTransaction>> GetWalletTransactionsAsync(int dealerId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == dealerId);
            if (wallet == null) return new List<WalletTransaction>();

            return await _context.WalletTransactions
                .Where(t => t.WalletId == wallet.Id)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();
        }
    }
}
