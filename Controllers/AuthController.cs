using EmLock.API.Data;
using EmLock.API.Helpers;
using EmLock.API.Models.DTOs;
using EmLock.API.Services;
using iText.StyledXmlParser.Jsoup.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtpNet;


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

            // 🧼 Avoid circular reference by returning only necessary fields
            return Ok(new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Role,
                user.DealerId
            });
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
        [Authorize]
        [HttpPost("setup-2fa")]
        public IActionResult Setup2FA()
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var secret = KeyGeneration.GenerateRandomKey(20);
            var secretBase32 = Base32Encoding.ToString(secret);
            var label = User.Identity?.Name ?? "user";
            var issuer = "EmLock";

            var otpUrl = TwoFactorHelper.GenerateOtpUrl(label, secretBase32, issuer);

            var user = _context.Users.Find(userId);
            if (user == null) return NotFound();

            user.TwoFactorSecretKey = secretBase32;
            _context.SaveChanges();

            return Ok(new
            {
                secretKey = secretBase32,
                qrCodeUrl = otpUrl
            });
        }

        [HttpPost("enable-2fa")]
        public async Task<IActionResult> Enable2FA([FromBody] TwoFactorVerifyDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null || string.IsNullOrEmpty(user.TwoFactorSecretKey)) return BadRequest("Secret key not found");

            var totp = new Totp(Base32Encoding.ToBytes(user.TwoFactorSecretKey));
            if (!totp.VerifyTotp(dto.Code, out _))
                return BadRequest("Invalid OTP");

            user.Is2FAEnabled = true;
            await _context.SaveChangesAsync();

            return Ok("2FA enabled successfully");
        }

    }
}
