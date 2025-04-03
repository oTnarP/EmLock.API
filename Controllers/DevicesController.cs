using EmLock.API.Models;
using EmLock.API.Models.DTOs;
using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmLock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "ShopkeeperOnly")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    public async Task<ActionResult<Device>> AddDevice(DeviceDto dto)
    {
        var added = await _deviceService.AddDeviceAsync(dto);
        return Ok(added);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Shopkeeper")]
    public async Task<ActionResult<List<Device>>> GetAllDevices()
    {
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }

    [HttpGet("{imei}")]
    [Authorize(Roles = "Admin,Shopkeeper")]
    public async Task<ActionResult<Device>> GetDeviceByImei(string imei)
    {
        var device = await _deviceService.GetDeviceByImeiAsync(imei);
        if (device == null) return NotFound("Device not found");

        return Ok(device);
    }
    [HttpPost("{imei}/lock")]
    [Authorize(Roles = "Admin,Shopkeeper")]
    public async Task<ActionResult> LockDevice(string imei)
    {
        var updated = await _deviceService.SetDeviceLockStateAsync(imei, true);
        if (!updated) return NotFound("Device not found");
        return Ok("Device locked successfully");
    }

    [HttpPost("{imei}/unlock")]
    [Authorize(Roles = "Admin,Shopkeeper")]
    public async Task<ActionResult> UnlockDevice(string imei)
    {
        var updated = await _deviceService.SetDeviceLockStateAsync(imei, false);
        if (!updated) return NotFound("Device not found");
        return Ok("Device unlocked successfully");
    }
    
}
