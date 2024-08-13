using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.NotificationModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        //[Authorize(Roles = "User, Expert")]
        public async Task<IActionResult> GetNotificationByFilter([FromQuery] NotificationFilterModel notificationModel)
        {
            try
            {
                var result = await _notificationService.GetAllNotifications(notificationModel);
                var metadata = new
                {
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
