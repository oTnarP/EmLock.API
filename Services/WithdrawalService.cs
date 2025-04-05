using EmLock.API.Data;
using EmLock.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmLock.API.Services
{
    public class WithdrawalService : IWithdrawalService
    {
        private readonly DataContext _context;

        public WithdrawalService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> RequestWithdrawalAsync(int dealerId, decimal amount)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.DealerId == dealerId);
            if (wallet == null || wallet.Balance < amount) return false;

            var request = new WithdrawalRequest
            {
                DealerId = dealerId,
                Amount = amount,
                Status = "Pending",
                RequestedAt = DateTime.UtcNow
            };

            _context.WithdrawalRequests.Add(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<WithdrawalRequest>> GetRequestsByDealerAsync(int dealerId)
        {
            return await _context.WithdrawalRequests
                .Where(r => r.DealerId == dealerId)
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();
        }

        public async Task<List<WithdrawalRequest>> GetAllRequestsAsync()
        {
            return await _context.WithdrawalRequests
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();
        }

        public async Task<bool> ApproveWithdrawalAsync(int requestId)
        {
            var request = await _context.WithdrawalRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null || request.Status != "Pending") return false;

            request.Status = "Approved";
            request.ResolvedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectWithdrawalAsync(int requestId)
        {
            var request = await _context.WithdrawalRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null || request.Status != "Pending") return false;

            request.Status = "Rejected";
            request.ResolvedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ApproveRequestAsync(int requestId)
        {
            var request = await _context.WithdrawalRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null || request.Status != "Pending") return false;

            request.Status = "Approved";
            request.ResolvedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectRequestAsync(int requestId)
        {
            var request = await _context.WithdrawalRequests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null || request.Status != "Pending") return false;

            request.Status = "Rejected";
            request.ResolvedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
