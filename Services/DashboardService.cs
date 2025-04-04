using EmLock.API.Data;
using Microsoft.EntityFrameworkCore;
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

        public async Task<object> GetOverviewAsync()
        {
            var totalDevices = await _context.Devices.CountAsync();
            var lockedDevices = await _context.Devices.CountAsync(d => d.IsLocked);
            var activeEmis = await _context.EmiSchedules.CountAsync();
            var overdueEmis = await _context.EmiSchedules.CountAsync(e => !e.IsPaid && e.DueDate < DateTime.UtcNow);
            var customers = await _context.Users.CountAsync(u => u.Role == "Customer");

            return new
            {
                totalDevices,
                lockedDevices,
                activeEmis,
                overdueEmis,
                customers
            };
        }
    }
}
