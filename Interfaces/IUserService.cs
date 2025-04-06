using EmLock.API.Models;

namespace EmLock.API.Interfaces
{
    public interface IUserService
    {
        Task<List<object>> GetShopkeepersWithLicenseInfoAsync();
        Task<bool> ReactivateUserAsync(int id);

    }


}
