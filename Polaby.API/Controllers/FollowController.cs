using Microsoft.AspNetCore.Mvc;
using Polaby.API.Helper;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommunityPostLikeModels;
using Polaby.Services.Models.FollowModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/follows")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followService;
        private readonly OneSignalPushNotificationService _oneSignalPushNotificationService;

        public FollowController(IFollowService followService, OneSignalPushNotificationService oneSignalPushNotificationService)
        {
            _followService = followService;
            _oneSignalPushNotificationService = oneSignalPushNotificationService;
        }

        [HttpPost()]
        public async Task<IActionResult> Follow([FromBody] FollowModel followModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _followService.Follow(followModel);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> Unfollow([FromBody] FollowModel followModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _followService.Unfollow(followModel);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
