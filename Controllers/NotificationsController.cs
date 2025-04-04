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
    }
}
