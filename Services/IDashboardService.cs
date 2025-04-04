using System.Collections.Generic;
using System.Threading.Tasks;
using EmLock.API.Dtos;

namespace EmLock.API.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetOverviewAsync();
        Task<List<MonthlyEmiStatDto>> GetMonthlyEmiStatsAsync();

    }
}
