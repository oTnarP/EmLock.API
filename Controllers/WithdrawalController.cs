using EmLock.API.Models;
using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WithdrawalController : ControllerBase
    {
        private readonly IWithdrawalService _withdrawalService;

        public WithdrawalController(IWithdrawalService withdrawalService)
        {
            _withdrawalService = withdrawalService;
        }

        // For Dealer/Admin - Request a withdrawal
        [HttpPost("request")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> RequestWithdrawal([FromBody] decimal amount)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            int dealerId = int.Parse(userId);
            var success = await _withdrawalService.RequestWithdrawalAsync(dealerId, amount);
            if (!success) return BadRequest(new { message = "Insufficient balance or wallet not found." });

            return Ok(new { message = "Withdrawal request submitted." });
        }

        // For Dealer/Admin - View my withdrawal requests
        [HttpGet("my-requests")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<IActionResult> GetMyRequests()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            int dealerId = int.Parse(userId);
            var requests = await _withdrawalService.GetRequestsByDealerAsync(dealerId);
            return Ok(requests);
        }

        // For Admin - View all requests
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _withdrawalService.GetAllRequestsAsync();
            return Ok(requests);
        }

        // For Admin - Approve a request
        [HttpPut("approve/{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int requestId)
        {
            var success = await _withdrawalService.ApproveRequestAsync(requestId);
            if (!success) return BadRequest(new { message = "Failed to approve request." });

            return Ok(new { message = "Withdrawal request approved." });
        }

        // For Admin - Reject a request
        [HttpPut("reject/{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int requestId)
        {
            var success = await _withdrawalService.RejectRequestAsync(requestId);
            if (!success) return BadRequest(new { message = "Failed to reject request." });

            return Ok(new { message = "Withdrawal request rejected." });
        }
    }
}
