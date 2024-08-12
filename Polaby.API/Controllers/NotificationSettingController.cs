using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.NotificationModels;
using Polaby.Services.Models.NotificationSettingModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/notification-settings")]
    [ApiController]
    public class NotificationSettingController : ControllerBase
    {
        private readonly INotificationSettingService _notificationSettingService;

        public NotificationSettingController(INotificationSettingService notificationSettingService)
        {
            _notificationSettingService = notificationSettingService;
        }

        [HttpPut()]
        //[Authorize(Roles = "User, Expert")]
        public async Task<IActionResult> Update([FromBody] NotificationSettingUpdateModel notificationSettingUpdateModel)
        {
            try
            {
                var result = await _notificationSettingService.Update(notificationSettingUpdateModel);

                if (result.Status)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        //[Authorize(Roles = "User, Expert")]
        public async Task<IActionResult> GetNotificationSettingByFilter([FromQuery] NotificationSettingFilterModel notificationFilterModel)
        {
            try
            {
                var result = await _notificationSettingService.GetAllNotificationSettings(notificationFilterModel);
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
