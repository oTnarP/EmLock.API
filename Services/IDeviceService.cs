using EmLock.API.Models;
using EmLock.API.Models.DTOs;

namespace EmLock.API.Services;

public interface IDeviceService
{
    Task<Device> AddDeviceAsync(DeviceDto dto);
}
