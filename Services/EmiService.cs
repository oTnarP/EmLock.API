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
            var emi = await _context.EmiSchedules.FindAsync(emiId);
            if (emi == null || emi.IsPaid)
                return false;

            emi.IsPaid = true;
            emi.PaymentDate = DateTime.UtcNow;

            var log = new EmiLog
            {
                EmiScheduleId = emi.Id,
                AmountPaid = emi.Amount,
                PaymentDate = emi.PaymentDate.Value,
                CreatedAt = DateTime.UtcNow
            };

            _context.EmiLogs.Add(log);
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
                .OrderByDescending(log => log.PaymentDate)
                .ToListAsync();
        }

    }
}
