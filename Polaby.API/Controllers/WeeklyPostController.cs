using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.WeeklyPostModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/weekly-posts")]
    [ApiController]
    public class WeeklyPostController : ControllerBase
    {
        private readonly IWeeklyPostService _weeklyPostService;

        public WeeklyPostController(IWeeklyPostService weeklyPostService)
        {
            _weeklyPostService = weeklyPostService;
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddWeeklyPost([FromBody] List<WeeklyPostCreateModel> weeklyPostCreateModels)
        {
            try
            {
                var result = await _weeklyPostService.AddWeeklyPosts(weeklyPostCreateModels);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // [Authorize(Roles = "Admin, User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeeklyPost(Guid id)
        {
            try
            {
                var result = await _weeklyPostService.GetWeeklyPost(id);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // [Authorize(Roles = "Admin, User")]
        [HttpGet("weeks/{week}")]
        public async Task<IActionResult> GetWeeklyPost(int week)
        {
            try
            {
                var result = await _weeklyPostService.GetWeeklyPost(week);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllReports([FromQuery] WeeklyPostFilterModel weeklyPostFilterModel)
        {
            try
            {
                var result = await _weeklyPostService.GetAllWeeklyPosts(weeklyPostFilterModel);
                var metadata = new
                {
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(Guid id, [FromBody] WeeklyPostUpdateModel weeklyPostUpdateModel)
        {
            try
            {
                var result = await _weeklyPostService.UpdateWeeklyPost(id, weeklyPostUpdateModel);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(Guid id)
        {
            try
            {
                var result = await _weeklyPostService.DeleteWeeklyPosts(id);
                if (result.Status)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}