using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Polaby.API.Helper;
using Polaby.Services.Common;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommentLikeModels;
using Polaby.Services.Models.FollowModels;
using Polaby.Services.Notification;

namespace Polaby.API.Controllers
{
    [Route("api/v1/comment-likes")]
    [ApiController]
    public class CommentLikeController : ControllerBase
    {
        private readonly ICommentLikeService _commentLikeService;
        private readonly IConfiguration _configuration;
        

        public CommentLikeController(ICommentLikeService commentLikeService, IConfiguration configuration)
        {
            _commentLikeService = commentLikeService;
            _configuration = configuration;
        }

        [HttpPost()]
        public async Task<IActionResult> Like([FromBody] CommentLikeModel commentLikeModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _commentLikeService.Like(commentLikeModel);
                if (result.Status)
                {
                    //CreateNotificationModel notification = new()
                    //{
                    //    Title = "Thích",
                    //    Content = result.Data.UserName + "đã thích bài viết của bạn.",
                    //    PlayerIds = new List<string>
                    //        {
                    //            "bfb806da-3075-48a1-acdd-699e68ee84d5",
                    //            "978d3302-874d-4d99-ac5d-89fd05efc0cf",
                    //            "4dded946-4396-475d-8887-159e7a9ec43e",
                    //            "a59f9d40-89c8-405a-abcc-cc366a78c567"
                    //        }
                    //};
                    //Guid appId = Guid.Parse(_configuration.GetSection(AppSettingKey.OneSignalAppId).Value);
                    //string restKey = _configuration.GetSection(AppSettingKey.OneSignalRestKey).Value;
                    //await OneSignalPushNotificationHelper.OneSignalPushNotification(notification, appId, restKey);

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
        public async Task<IActionResult> Unlike([FromBody] CommentLikeModel commentLikeModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _commentLikeService.Unlike(commentLikeModel);
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
