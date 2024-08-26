using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.SafeFoodModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/safefoods")]
    [ApiController]
    public class SafeFoodController : ControllerBase
    {
        private readonly ISafeFoodService _safeFoodService;

        public SafeFoodController(ISafeFoodService safeFoodService)
        {
            _safeFoodService = safeFoodService;
        }

        [HttpPost]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateSafeFood([FromBody] SafeFoodCreateModel createModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _safeFoodService.AddSafeFood(createModel);
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

        [HttpGet]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllSafeFoods([FromQuery] SafeFoodFilterModel filterModel)
        {
            try
            {
                var result = await _safeFoodService.GetAllSafeFoods(filterModel);
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

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateSafeFood(Guid id, [FromBody] SafeFoodCreateModel updateModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _safeFoodService.UpdateSafeFood(id, updateModel);
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

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteSafeFood(Guid id)
        {
            try
            {
                var result = await _safeFoodService.DeleteSafeFood(id);
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
