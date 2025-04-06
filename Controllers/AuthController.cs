using EmLock.API.Data;
using EmLock.API.Helpers;
using EmLock.API.Models.DTOs;
using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly DataContext _context;
        public AuthController(IAuthService authService, DataContext context)
        {
            _authService = authService;
            _context = context;
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

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> VerifyTwoFactor([FromBody] TwoFactorDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && u.Is2FAEnabled);
            if (user == null || string.IsNullOrEmpty(user.TwoFactorSecretKey))
                return BadRequest(new { message = "2FA is not enabled for this user." });

            bool isValid = TwoFactorHelper.VerifyCode(user.TwoFactorSecretKey, dto.Code);
            if (!isValid) return Unauthorized(new { message = "Invalid 2FA code." });

            // If valid, you can generate and return a real JWT or token
            return Ok(new { message = "2FA verified successfully." });
        }

    }
}
