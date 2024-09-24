using Microsoft.AspNetCore.Mvc;
using Polaby.API.Helper;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommunityPostLikeModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/community-post-likes")]
    [ApiController]
    public class CommunityPostLikeController : ControllerBase
    {
        private readonly ICommunityPostLikeService _communityPostLikeService;
        private readonly OneSignalPushNotificationService _oneSignalPushNotificationService;

        public CommunityPostLikeController(ICommunityPostLikeService communityPostLikeService, 
            OneSignalPushNotificationService oneSignalPushNotificationService)
        {
            _communityPostLikeService = communityPostLikeService;
            _oneSignalPushNotificationService = oneSignalPushNotificationService;
        }

        [HttpPost()]
        public async Task<IActionResult> Like([FromBody] CommunityPostLikeModel communityPostLikeModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _communityPostLikeService.Like(communityPostLikeModel);
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
        public async Task<IActionResult> Unlike([FromBody] CommunityPostLikeModel communityPostLikeModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _communityPostLikeService.Unlike(communityPostLikeModel);
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
