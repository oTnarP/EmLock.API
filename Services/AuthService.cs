using EmLock.API.Data;
using EmLock.API.Models;
using EmLock.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EmLock.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public AuthService(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<User> Register(UserRegisterDto dto)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                FullName = dto.FullName,
                Role = dto.Role,
                Phone = dto.Phone,
                Email = dto.Email,
                PasswordHash = passwordHash,
                DealerId = dto.DealerId
            };

            if (dto.Role == "Shopkeeper")
            {
                user.LicenseStartDate = (dto.LicenseStartDate ?? DateTime.UtcNow).ToUniversalTime();
                user.LicenseEndDate = (dto.LicenseEndDate ?? DateTime.UtcNow.AddMonths(1)).ToUniversalTime();
                user.IsLicenseActive = true;
            }

            if (dto.Role == "Dealer")
            {
                var dealer = new Dealer
                {
                    Name = dto.FullName,
                    Phone = dto.Phone,
                    Email = dto.Email
                };

                _context.Dealers.Add(dealer);
                await _context.SaveChangesAsync();

                user.DealerId = dealer.Id;
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<object> Login(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
                throw new Exception("User not found");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            // If 2FA is enabled, return early and don't issue token
            if (user.Is2FAEnabled && !string.IsNullOrEmpty(user.TwoFactorSecretKey))
            {
                return new
                {
                    requires2FA = true,
                    userId = user.Id,
                    email = user.Email
                };
            }

            // Otherwise, issue JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }


        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
