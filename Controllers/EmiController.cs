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

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<List<EmiSchedule>>> GetEmisByDeviceId(int deviceId)
        {
            var emis = await _emiService.GetEmisByDeviceIdAsync(deviceId);
            return Ok(emis);
        }
    }
}
