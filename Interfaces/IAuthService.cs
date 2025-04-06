using EmLock.API.Models;
using EmLock.API.Models.DTOs;

namespace EmLock.API.Interfaces
{
    public interface IAuthService
    {
        Task<User> Register(UserRegisterDto dto);
        Task<object> Login(UserLoginDto dto);

    }
}
