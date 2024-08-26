using Microsoft.AspNetCore.Mvc;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommentLikeModels;
using Polaby.Services.Models.FollowModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/comment-likes")]
    [ApiController]
    public class CommentLikeController : ControllerBase
    {
        private readonly ICommentLikeService _commentLikeService;

        public CommentLikeController(ICommentLikeService commentLikeService)
        {
            _commentLikeService = commentLikeService;
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
