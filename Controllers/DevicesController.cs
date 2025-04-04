using EmLock.API.Models;
using EmLock.API.Models.DTOs;
using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmLock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    public DevicesController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Device>> AddDevice(DeviceDto dto)
    {
        // 🔹 Get logged-in user ID from JWT token claims
        var userId = User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token");

        var userIdClaim = User.FindFirst("UserId");
        if (userIdClaim == null) return Unauthorized("User ID not found in token");

        dto.UserId = int.Parse(userIdClaim.Value); 


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

    [HttpGet("{imei}/status")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<object>> GetDeviceLockStatus(string imei)
    {
        var device = await _deviceService.GetDeviceByImeiAsync(imei);
        if (device == null) return NotFound("Device not found");

        return Ok(new
        {
            imei = device.IMEI,
            isLocked = device.IsLocked
        });
    }
    [HttpPut("{imei}")]
    [Authorize(Roles = "Admin,Shopkeeper")]
    public async Task<ActionResult<Device>> UpdateDevice(string imei, DeviceDto dto)
    {
        var updated = await _deviceService.UpdateDeviceAsync(imei, dto);
        if (updated == null) return NotFound("Device not found");
        return Ok(updated);
    }
    [HttpGet("my")]
    [Authorize(Roles = "Customer,Shopkeeper")]
    public async Task<ActionResult<List<Device>>> GetMyDevices()
    {
        var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        if (userIdClaim == null)
            return Unauthorized("User email not found in token.");

        var userEmail = userIdClaim.Value;
        var devices = await _deviceService.GetDevicesByEmailAsync(userEmail);
        return Ok(devices);
    }
    [HttpGet("logs/{imei}")]
    [Authorize(Roles = "Shopkeeper,Admin")]
    public async Task<ActionResult<IEnumerable<DeviceActionLog>>> GetLogsByIMEI(string imei)
    {
        var logs = await _deviceService.GetLogsByIMEIAsync(imei);
        return Ok(logs);
    }



}
