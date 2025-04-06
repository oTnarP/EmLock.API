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
        [HttpPost("freeze/{dealerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FreezeWallet(int dealerId, [FromBody] string adminNote)
        {
            var wallet = await _walletService.GetWalletByDealerIdAsync(dealerId);
            if (wallet == null) return NotFound("Wallet not found");

            wallet.IsFrozen = true;
            wallet.AdminNote = adminNote;
            await _walletService.SaveChangesAsync();

            return Ok(new { message = "Wallet frozen", dealerId });
        }

        [HttpPost("unfreeze/{dealerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnfreezeWallet(int dealerId)
        {
            var wallet = await _walletService.GetWalletByDealerIdAsync(dealerId);
            if (wallet == null) return NotFound("Wallet not found");

            wallet.IsFrozen = false;
            wallet.AdminNote = null;
            await _walletService.SaveChangesAsync();

            return Ok(new { message = "Wallet unfrozen", dealerId });
        }

    }

}
