using EmLock.API.Models.DTOs;
using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var user = await _authService.Register(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var token = await _authService.Login(dto);
            return Ok(new { token });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminEndpoint()
        {
            return Ok("You are an admin!");
        }

        [Authorize(Roles = "Shopkeeper")]
        [HttpGet("shopkeeper-only")]
        public IActionResult ShopkeeperEndpoint()
        {
            return Ok("You are a shopkeeper!");
        }

        [Authorize]
        [HttpGet("general")]
        public IActionResult GeneralUserEndpoint()
        {
            return Ok("You're logged in!");
        }


    }
}
