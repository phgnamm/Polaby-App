using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<IActionResult> CreateEmotion([FromBody] EmotionRequestModel model)
        {
            var result = await _emotionService.CreateEmotionAsync(model);
            if (result.Status)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmotionsByFilter([FromQuery] EmotionFilterModel filterModel)
        {
            var result = await _emotionService.GetEmotionsByFilterAsync(filterModel);

            var metadata = new
            {
                result.PageSize,
                result.CurrentPage,
                result.TotalPages,
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmotion(Guid id)
        {
            var result = await _emotionService.DeleteEmotionAsync(id);
            if (result.Status)
            {
                return Ok(result.Message);
            }

            return NotFound(result.Message);
        }
    }
}
