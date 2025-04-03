using EmLock.API.Models;
using EmLock.API.Models.DTOs;
using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}
