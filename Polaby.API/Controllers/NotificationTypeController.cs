using Microsoft.AspNetCore.Mvc;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.NotificationTypeModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/notification-types")]
    [ApiController]
    public class NotificationTypeController : ControllerBase
    {
        private readonly INotificationTypeService _notificationTypeService;

        public NotificationTypeController(INotificationTypeService notificationTypeService)
        {
            _notificationTypeService = notificationTypeService;
        }

        [HttpPut()]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] NotificationTypeModel notificationTypeModel)
        {
            try
            {
                var result = await _notificationTypeService.Update(notificationTypeModel);

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
    }
}
