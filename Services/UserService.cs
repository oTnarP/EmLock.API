// UserService.cs
using EmLock.API.Data;
using EmLock.API.Services;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly DataContext _context;
    public UserService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<object>> GetShopkeepersWithLicenseInfoAsync()
    {
        return await _context.Users
            .Where(u => u.Role == "Shopkeeper")
            .Select(u => new
            {
                u.Id,
                u.FullName,
                u.Email,
                u.Phone,
                u.LicenseStartDate,
                u.LicenseEndDate,
                u.IsLicenseActive,
                u.IsCurrentlyLicensed
            })
            .ToListAsync<object>();
    }
}
