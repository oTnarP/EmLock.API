using EmLock.API.Models;
using EmLock.API.Models.DTOs;

namespace EmLock.API.Services;

public interface IDeviceService
{
    Task<Device> AddDeviceAsync(DeviceDto dto);
    Task<List<Device>> GetAllDevicesAsync();
    Task<Device?> GetDeviceByImeiAsync(string imei);
    Task<bool> SetDeviceLockStateAsync(string imei, bool isLocked);
    Task<Device?> UpdateDeviceAsync(string imei, DeviceDto dto);
    Task<List<Device>> GetDevicesByEmailAsync(string email);
    Task<List<DeviceActionLog>> GetLogsByIMEIAsync(string imei);

}
