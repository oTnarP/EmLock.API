using EmLock.API.Dtos;
using EmLock.API.Models.DTOs;
using System.Threading.Tasks;

namespace EmLock.API.Interfaces
{
    public interface IAnalyticsService
    {
        Task<AnalyticsDto> GetBasicAnalyticsAsync();
    }
}
