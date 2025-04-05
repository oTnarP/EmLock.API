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

    }
}
