using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.RatingModel;

namespace Polaby.API.Controllers
{
    [Route("api/v1/ratings")]
    [ApiController]
    public class RatingController: ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        [HttpPost]
        //[Authorize("User")]
        public async Task<IActionResult> CreateRating([FromBody] CreateRatingModel model)
        {
            var response = await _ratingService.CreateRatingAsync(model);
            if (!response.Status)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }
        [HttpPut]
        //[Authorize("User")]
        public async Task<IActionResult> UpdateRating([FromBody] CreateRatingModel model)
        {
            var response = await _ratingService.UpdateRatingAsync(model);
            if (!response.Status)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpDelete]
        //[Authorize("User")]
        public async Task<IActionResult> DeleteRating([FromQuery] Guid userId, [FromQuery] Guid expertId)
        {
            var response = await _ratingService.DeleteRatingAsync(userId, expertId);
            if (!response.Status)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }
    }

}
