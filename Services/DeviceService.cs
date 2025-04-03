using EmLock.API.Data;
using EmLock.API.Models;
using EmLock.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EmLock.API.Services;

public class DeviceService : IDeviceService
{
    private readonly DataContext _context;
    public DeviceService(DataContext context)
    {
        _context = context;
    }

    public async Task<Device> AddDeviceAsync(DeviceDto dto)
    {
        var device = new Device
        {
            IMEI = dto.IMEI,
            Model = dto.Model,
            OwnerName = dto.OwnerName,
            OwnerPhone = dto.OwnerPhone
        };

        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        return device;
    }

    public async Task<List<Device>> GetAllDevicesAsync()
    {
        return await _context.Devices.ToListAsync();
    }
    public async Task<Device?> GetDeviceByImeiAsync(string imei)
    {
        return await _context.Devices.FirstOrDefaultAsync(d => d.IMEI == imei);
    }

}
