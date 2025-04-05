using EmLock.API.Dtos;
using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Shopkeeper,Dealer")]
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
        [HttpGet("top-customers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTopCustomers()
        {
            var topCustomers = await _dashboardService.GetTopPayingCustomersAsync();
            return Ok(topCustomers);
        }
        [HttpGet("role-based")]
        [Authorize]
        public async Task<IActionResult> GetDashboardForRole()
        {
            // STEP 1: Read role from JWT claims
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            // STEP 2: Role-based logic
            if (role == "Dealer")
            {
                // Dealer-specific dashboard logic
                var dealerStats = await _dashboardService.GetDealerDashboardAsync(userId);
                return Ok(dealerStats);
            }
            else if (role == "Shopkeeper")
            {
                var shopkeeperStats = await _dashboardService.GetShopkeeperDashboardAsync(userId);
                return Ok(shopkeeperStats);
            }
            else if (role == "Admin")
            {
                var adminStats = await _dashboardService.GetAdminDashboardAsync();
                return Ok(adminStats);
            }

            return Forbid(); // Role doesn't match expected
        }


    }
}
