using EmLock.API.Data;
using EmLock.API.Dtos;
using EmLock.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly DataContext _context;

        public DashboardService(DataContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetOverviewAsync()
        {
            var totalDevices = await _context.Devices.CountAsync();
            var lockedDevices = await _context.Devices.CountAsync(d => d.IsLocked);
            var activeCustomers = await _context.Users.CountAsync(u => u.Role == "Customer");

            var totalPaid = await _context.EmiSchedules
                .Where(e => e.IsPaid)
                .SumAsync(e => e.Amount);

            var totalDue = await _context.EmiSchedules
                .Where(e => !e.IsPaid)
                .SumAsync(e => e.Amount);

            return new DashboardSummaryDto
            {
                TotalDevices = totalDevices,
                LockedDevices = lockedDevices,
                ActiveCustomers = activeCustomers,
                TotalPaidAmount = totalPaid,
                TotalDueAmount = totalDue
            };
        }
        public async Task<List<MonthlyEmiStatDto>> GetMonthlyEmiStatsAsync()
        {
            var rawData = await _context.EmiSchedules
                .GroupBy(e => new { e.DueDate.Year, e.DueDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalPaid = g.Where(e => e.IsPaid).Sum(e => e.Amount),
                    TotalDue = g.Where(e => !e.IsPaid).Sum(e => e.Amount)
                })
                .ToListAsync();

            var result = rawData
                .Select(g => new MonthlyEmiStatDto
                {
                    Month = $"{g.Year}-{g.Month:D2}",
                    TotalPaid = g.TotalPaid,
                    TotalDue = g.TotalDue
                })
                .OrderBy(g => g.Month)
                .ToList();

            return result;
        }

        public async Task<List<TopCustomerDto>> GetTopPayingCustomersAsync(int count = 5)
        {
            return await _context.Users
                .Where(u => u.Role == "Customer")
                .Select(u => new TopCustomerDto
                {
                    CustomerId = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    TotalPaid = u.Devices
                        .SelectMany(d => d.EmiSchedules)
                        .Where(e => e.IsPaid)
                        .Sum(e => (decimal?)e.Amount) ?? 0
                })
                .OrderByDescending(c => c.TotalPaid)
                .Take(count)
                .ToListAsync();
        }
        public async Task<object> GetDashboardForRoleAsync(string role, int userId)
        {
            if (role == "Admin")
            {
                return await GetOverviewAsync();
            }
            else if (role == "Dealer")
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == userId);
                var transactions = await _context.WalletTransactions
                    .Where(t => t.DealerId == userId)
                    .OrderByDescending(t => t.Timestamp)
                    .Take(5)
                    .ToListAsync();

                return new
                {
                    dealerId = userId,
                    walletBalance = wallet?.Balance ?? 0,
                    recentTransactions = transactions
                };
            }
            else if (role == "Shopkeeper")
            {
                var myDevices = await _context.Devices
                    .Where(d => d.UserId == userId)
                    .Include(d => d.EmiSchedules)
                    .ToListAsync();

                var totalDevices = myDevices.Count;
                var locked = myDevices.Count(d => d.IsLocked);
                var totalEmis = myDevices.SelectMany(d => d.EmiSchedules).Count();
                var dueEmis = myDevices.SelectMany(d => d.EmiSchedules).Count(e => !e.IsPaid);

                return new
                {
                    totalDevices,
                    lockedDevices = locked,
                    totalEmis,
                    dueEmis
                };
            }

            return new { message = "Role not supported" };
        }
        public async Task<object> GetAdminDashboardAsync()
        {
            var totalDevices = await _context.Devices.CountAsync();
            var lockedDevices = await _context.Devices.CountAsync(d => d.IsLocked);
            var totalUsers = await _context.Users.CountAsync();
            var totalEmi = await _context.EmiSchedules.SumAsync(e => e.Amount);
            var paidEmi = await _context.EmiSchedules.Where(e => e.IsPaid).SumAsync(e => e.Amount);

            return new
            {
                Role = "Admin",
                TotalDevices = totalDevices,
                LockedDevices = lockedDevices,
                TotalUsers = totalUsers,
                TotalEmi = totalEmi,
                PaidEmi = paidEmi
            };
        }

        public async Task<object> GetShopkeeperDashboardAsync(int shopkeeperId)
        {
            var devices = await _context.Devices
                .Where(d => d.UserId == shopkeeperId)
                .Select(d => d.Id)
                .ToListAsync();

            var totalEmi = await _context.EmiSchedules
                .Where(e => devices.Contains(e.DeviceId))
                .SumAsync(e => e.Amount);

            var paidEmi = await _context.EmiSchedules
                .Where(e => devices.Contains(e.DeviceId) && e.IsPaid)
                .SumAsync(e => e.Amount);

            return new
            {
                Role = "Shopkeeper",
                ShopkeeperId = shopkeeperId,
                TotalDevices = devices.Count,
                TotalEmi = totalEmi,
                PaidEmi = paidEmi
            };
        }

        public async Task<object> GetDealerDashboardAsync(int dealerId)
        {
            var shopkeepers = await _context.Users
                .Where(u => u.DealerId == dealerId && u.Role == "Shopkeeper")
                .Select(u => u.Id)
                .ToListAsync();

            var devices = await _context.Devices
                .Where(d => shopkeepers.Contains(d.UserId))
                .Select(d => d.Id)
                .ToListAsync();

            var totalEmi = await _context.EmiSchedules
                .Where(e => devices.Contains(e.DeviceId))
                .SumAsync(e => e.Amount);

            var paidEmi = await _context.EmiSchedules
                .Where(e => devices.Contains(e.DeviceId) && e.IsPaid)
                .SumAsync(e => e.Amount);

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == dealerId);

            return new
            {
                Role = "Dealer",
                DealerId = dealerId,
                ShopkeeperCount = shopkeepers.Count,
                DeviceCount = devices.Count,
                TotalEmi = totalEmi,
                PaidEmi = paidEmi,
                WalletBalance = wallet?.Balance ?? 0
            };
        }

    }
}
