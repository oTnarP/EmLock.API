using EmLock.API.Data;
using EmLock.API.Models;
using EmLock.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EmLock.API.Services
{
    public class EmiService : IEmiService
    {
        private readonly DataContext _context;

        public EmiService(DataContext context)
        {
            _context = context;
        }

        public async Task<EmiSchedule> AddEmiAsync(EmiScheduleDto dto)
        {
            var emi = new EmiSchedule
            {
                DeviceId = dto.DeviceId,
                Amount = dto.Amount,
                DueDate = DateTime.SpecifyKind(dto.DueDate, DateTimeKind.Utc),
                IsPaid = false
            };

            _context.EmiSchedules.Add(emi);
            await _context.SaveChangesAsync();
            return emi;
        }

        public async Task<List<EmiSchedule>> GetEmisByDeviceIdAsync(int deviceId)
        {
            return await _context.EmiSchedules
                .Where(e => e.DeviceId == deviceId)
                .OrderBy(e => e.DueDate)
                .ToListAsync();
        }
        public async Task<bool> MarkAsPaidAsync(int emiId)
        {
            var emi = await _context.EmiSchedules
                .Include(e => e.Device)
                    .ThenInclude(d => d.User)
                        .ThenInclude(u => u.Dealer)
                .FirstOrDefaultAsync(e => e.Id == emiId);

            if (emi == null || emi.IsPaid) return false;

            emi.IsPaid = true;

            // Log EMI payment
            var emiLog = new EmiLog
            {
                EmiScheduleId = emi.Id,
                AmountPaid = emi.Amount,
                PaidAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            _context.EmiLogs.Add(emiLog);

            // Revenue sharing logic
            decimal dealerShare = 0;
            decimal adminShare = 0;

            var dealer = emi.Device?.User?.Dealer;

            if (dealer != null)
            {
                dealerShare = emi.Amount * 0.30m;
                adminShare = emi.Amount - dealerShare;

                // Dealer wallet
                var dealerWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == dealer.Id);
                if (dealerWallet == null)
                {
                    dealerWallet = new Wallet { DealerId = dealer.Id, Balance = 0 };
                    _context.Wallets.Add(dealerWallet);
                }
                dealerWallet.Balance += dealerShare;
            }
            else
            {
                adminShare = emi.Amount;
            }

            // Admin wallet (DealerId = 1 is reserved for super admin)
            var adminWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == 1);
            if (adminWallet == null)
            {
                adminWallet = new Wallet { DealerId = 1, Balance = 0 };
                _context.Wallets.Add(adminWallet);
            }
            adminWallet.Balance += adminShare;

            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<List<EmiSchedule>> GetOverdueEmisAsync()
        {
            return await _context.EmiSchedules
                .Where(e => !e.IsPaid && e.DueDate < DateTime.UtcNow)
                .ToListAsync();
        }
        public async Task<int> AutoLockOverdueDevicesAsync()
        {
            var today = DateTime.UtcNow;

            // Get all overdue and unpaid EMIs
            var overdueEmis = await _context.EmiSchedules
                .Include(e => e.Device)
                .Where(e => !e.IsPaid && e.DueDate < today && !e.Device.IsLocked)
                .ToListAsync();

            foreach (var emi in overdueEmis)
            {
                emi.Device.IsLocked = true;
            }

            await _context.SaveChangesAsync();
            return overdueEmis.Count;
        }
        public async Task<List<EmiLog>> GetLogsByEmiScheduleIdAsync(int emiScheduleId)
        {
            return await _context.EmiLogs
                .Where(log => log.EmiScheduleId == emiScheduleId)
                .OrderByDescending(log => log.PaidAt)
                .ToListAsync();
        }
        public async Task<List<EmiSchedule>> FilterEmisAsync(EmiFilterDto filter)
        {
            var query = _context.EmiSchedules.AsQueryable();

            if (filter.IsPaid.HasValue)
                query = query.Where(e => e.IsPaid == filter.IsPaid);

            if (filter.FromDate.HasValue)
                query = query.Where(e => e.DueDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(e => e.DueDate <= filter.ToDate.Value);

            if (filter.DeviceId.HasValue)
                query = query.Where(e => e.DeviceId == filter.DeviceId.Value);

            if (filter.UserId.HasValue)
                query = query.Where(e => e.Device.UserId == filter.UserId.Value);

            return await query
                .Include(e => e.Device)
                .OrderByDescending(e => e.DueDate)
                .ToListAsync();
        }

    }
}
