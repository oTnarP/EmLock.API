using System.Collections.Generic;
using System.Threading.Tasks;
using EmLock.API.Dtos;
using EmLock.API.Models.DTOs;

namespace EmLock.API.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetOverviewAsync();
        Task<List<MonthlyEmiStatDto>> GetMonthlyEmiStatsAsync();
        Task<List<TopCustomerDto>> GetTopPayingCustomersAsync(int count = 5);

    }
}
