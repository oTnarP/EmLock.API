using EmLock.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public interface IWithdrawalService
    {
        Task<bool> RequestWithdrawalAsync(int dealerId, decimal amount);
        Task<List<WithdrawalRequest>> GetRequestsByDealerAsync(int dealerId);
        Task<List<WithdrawalRequest>> GetAllRequestsAsync();
        Task<bool> ApproveWithdrawalAsync(int requestId);
        Task<bool> RejectWithdrawalAsync(int requestId);
        Task<bool> ApproveRequestAsync(int requestId);
        Task<bool> RejectRequestAsync(int requestId);

    }
}
