using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Models.ScheduleModels;
using Polaby.Services.Services;

namespace Polaby.API.Controllers
{
    [Route("api/v1/schedules")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost()]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> Create([FromBody] ScheduleCreateModel scheduleCreateModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _scheduleService.Create(scheduleCreateModel);
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

        [HttpPut("{id}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ScheduleUpdateModel scheduleUpdateModel)
        {
            try
            {
                var result = await _scheduleService.Update(id, scheduleUpdateModel);

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

        [HttpDelete("{id}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _scheduleService.Delete(id);
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
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> GetScheduleByFilter([FromQuery] ScheduleFilterModel scheduleFilterModel)
        {
            try
            {
                var result = await _scheduleService.GetAllSchedules(scheduleFilterModel);
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
