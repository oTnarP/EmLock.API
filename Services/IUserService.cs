using EmLock.API.Models;

namespace EmLock.API.Services
{
    public interface IUserService
    {
        Task<List<object>> GetShopkeepersWithLicenseInfoAsync();
    }


}
