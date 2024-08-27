using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.HealthModels;
using Polaby.Services.Models.ResponseModels;
using Polaby.Services.Services;

namespace Polaby.API.Controllers
{
    [Route("api/v1/healths")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly IHealthService _healthService;

        public HealthController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpPost]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> AddHealthAsync([FromBody] List<HealthCreateModel> healthModels)
        {
            if (healthModels == null || healthModels.Count == 0)
            {
                return BadRequest(new ResponseModel
                {
                    Status = false,
                    Message = "No health records provided!"
                });
            }

            var result = await _healthService.AddHealthAsync(healthModels);
            if (result.Status)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllHealthAsync([FromQuery] HealthFilterModel filterModel)
        {
            var healthList = await _healthService.GetAllHealthAsync(filterModel);
            if (healthList != null)
            {
                return Ok(healthList);
            }

            return NotFound(new ResponseModel
            {
                Status = false,
                Message = "No health records found!"
            });
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateHealthAsync(Guid id, [FromBody] HealthUpdateModel updateModel)
        {
            if (updateModel == null)
            {
                return BadRequest(new ResponseModel
                {
                    Status = false,
                    Message = "Invalid update model!"
                });
            }

            var result = await _healthService.UpdateHealthAsync(id, updateModel);
            if (result.Status)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteHealthAsync(Guid id)
        {
            var result = await _healthService.DeleteHealthAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }

            return NotFound(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _healthService.GetById(id);
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
