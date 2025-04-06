using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("shopkeepers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllShopkeepers()
        {
            var users = await _userService.GetShopkeepersWithLicenseInfoAsync();
            return Ok(users);
        }
        [HttpPost("reactivate/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReactivateUser(int id)
        {
            var success = await _userService.ReactivateUserAsync(id);
            if (!success) return NotFound(new { message = "User not found or already active." });

            return Ok(new { message = "User reactivated successfully." });
        }

    }
}
