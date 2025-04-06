// Controllers/NotificationsController.cs
using EmLock.API.Services;
using EmLock.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmLock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Shopkeeper,Admin")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("log")]
        public async Task<IActionResult> LogNotification([FromBody] Notification notification)
        {
            await _notificationService.LogNotificationAsync(notification);
            return Ok(new { message = "Notification logged successfully." });
        }
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,Shopkeeper")]
        public async Task<ActionResult<List<Notification>>> GetByUser(int userId)
        {
            var logs = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(logs);
        }
        [HttpPost("queue")]
        [Authorize] // or [AllowAnonymous] for now if needed
        public async Task<IActionResult> QueueNotification([FromBody] Notification dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DeviceToken))
                return BadRequest(new { message = "Device token is required." });

            dto.Status = "Queued";
            dto.CreatedAt = DateTime.UtcNow;
            dto.UserId = dto.UserId == 0 ? int.Parse(User.FindFirst("UserId")?.Value ?? "0") : dto.UserId;

            await _notificationService.QueueNotificationAsync(
                dto.UserId,
                dto.DeviceToken,
                dto.Title,
                dto.Message,
                dto.Type
            );

            return Ok(new { message = "Notification queued and logged." });
        }

    }
}
