using EmLock.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("balance")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<ActionResult> GetBalance()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null) return Unauthorized();

            int dealerId = int.Parse(userIdClaim);
            var wallet = await _walletService.GetWalletByDealerIdAsync(dealerId);

            return Ok(new
            {
                dealerId,
                balance = wallet?.Balance ?? 0
            });
        }

        [HttpGet("transactions")]
        [Authorize(Roles = "Dealer,Admin")]
        public async Task<ActionResult> GetTransactions()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null) return Unauthorized();

            int dealerId = int.Parse(userIdClaim);
            var transactions = await _walletService.GetWalletTransactionsAsync(dealerId);
            return Ok(transactions);
        }
    }

}
