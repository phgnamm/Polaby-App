using Microsoft.AspNetCore.Mvc;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.EmotionModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/emotions")]
    [ApiController]
    public class EmotionController : ControllerBase
    {
        private readonly IEmotionService _emotionService;
        public EmotionController(IEmotionService emotionService)
        {
            _emotionService = emotionService;
        }
        [HttpPost]
        //[Authorize("User")]
        public async Task<IActionResult> AddEmotion([FromBody] EmotionRequestModel model)
        {
            var result = await _emotionService.AddEmotionAsync(model);
            if (!result.Status)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(result);
        }
        [HttpDelete("{id}")]
        //[Authorize("User")]
        public async Task<IActionResult> DeleteEmotion(Guid id)
        {
            var result = await _emotionService.DeleteEmotionAsync(id);
            if (!result.Status)
            {
                return BadRequest(new { result.Message });
            }

            return Ok(result);
        }
    }
}
