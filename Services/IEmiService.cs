using EmLock.API.Models;
using EmLock.API.Models.DTOs;

namespace EmLock.API.Services
{
    public interface IEmiService
    {
        Task<EmiSchedule> AddEmiAsync(EmiScheduleDto dto);
        Task<List<EmiSchedule>> GetEmisByDeviceIdAsync(int deviceId);
        Task<bool> MarkAsPaidAsync(int emiId);
        Task<List<EmiSchedule>> GetOverdueEmisAsync();
        Task<int> AutoLockOverdueDevicesAsync();
        Task<List<EmiLog>> GetLogsByEmiScheduleIdAsync(int emiScheduleId);

    }
}
