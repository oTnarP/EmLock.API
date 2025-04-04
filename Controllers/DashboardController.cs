using EmLock.API.Dtos;
using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Shopkeeper")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            var summary = await _dashboardService.GetOverviewAsync();
            return Ok(summary);
        }
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _dashboardService.GetOverviewAsync();
            return Ok(result);
        }
        [HttpGet("monthly-emi-stats")]
        public async Task<ActionResult<List<MonthlyEmiStatDto>>> GetMonthlyEmiStats()
        {
            var stats = await _dashboardService.GetMonthlyEmiStatsAsync();
            return Ok(stats);
        }

    }
}
