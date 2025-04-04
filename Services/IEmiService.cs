using EmLock.API.Models;
using EmLock.API.Models.DTOs;

namespace EmLock.API.Services
{
    public interface IEmiService
    {
        Task<EmiSchedule> AddEmiAsync(EmiScheduleDto dto);
        Task<List<EmiSchedule>> GetEmisByDeviceIdAsync(int deviceId);
    }
}
