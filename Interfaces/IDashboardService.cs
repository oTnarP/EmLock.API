using System.Collections.Generic;
using System.Threading.Tasks;
using EmLock.API.Dtos;
using EmLock.API.Models.DTOs;

namespace EmLock.API.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetOverviewAsync();
        Task<List<MonthlyEmiStatDto>> GetMonthlyEmiStatsAsync();
        Task<List<TopCustomerDto>> GetTopPayingCustomersAsync(int count = 5);
        Task<object> GetDashboardForRoleAsync(string role, int userId);
        Task<object> GetAdminDashboardAsync();
        Task<object> GetShopkeeperDashboardAsync(int shopkeeperId);
        Task<object> GetDealerDashboardAsync(int dealerId);

    }
}
