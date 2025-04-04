using EmLock.API.Models;
using EmLock.API.Models.DTOs;
using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Shopkeeper,Admin")]
    public class EmiController : ControllerBase
    {
        private readonly IEmiService _emiService;

        public EmiController(IEmiService emiService)
        {
            _emiService = emiService;
        }

        [HttpPost]
        public async Task<ActionResult<EmiSchedule>> AddEmi(EmiScheduleDto dto)
        {
            var emi = await _emiService.AddEmiAsync(dto);
            return Ok(emi);
        }

        [HttpGet("device/{deviceId}")]
        [Authorize(Roles = "Shopkeeper")]
        public async Task<ActionResult<List<EmiSchedule>>> GetEmisByDevice(int deviceId)
        {
            var emis = await _emiService.GetEmisByDeviceIdAsync(deviceId);
            return Ok(emis);
        }

        [HttpPut("{emiId}/pay")]
        [Authorize(Roles = "Shopkeeper")]
        public async Task<IActionResult> MarkAsPaid(int emiId)
        {
            var success = await _emiService.MarkAsPaidAsync(emiId);
            if (!success) return NotFound();

            return Ok(new { message = "EMI marked as paid." });
        }
        [HttpGet("overdue")]
        [Authorize(Roles = "Shopkeeper,Admin")]
        public async Task<ActionResult<List<EmiSchedule>>> GetOverdueEmis()
        {
            var overdueEmis = await _emiService.GetOverdueEmisAsync();
            return Ok(overdueEmis);
        }
        [HttpPost("auto-lock-overdue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AutoLockOverdueDevices()
        {
            var count = await _emiService.AutoLockOverdueDevicesAsync();
            return Ok(new { message = $"{count} device(s) locked due to overdue EMIs." });
        }

    }
}
