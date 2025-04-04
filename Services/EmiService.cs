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
    }
}
