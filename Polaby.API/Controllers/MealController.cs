using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.MealModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/meals")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }


        [HttpPost()]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMeal([FromBody] List<MealImportModel> meals)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _mealService.AddRangeMeal(meals);
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
        //[Authorize(Roles = "Admin,User, Expert")]
        public async Task<IActionResult> GetAllMenuByFilter([FromQuery] MealFilterModel mealFilterModel)
        {
            try
            {
                var result = await _mealService.GetAllMeal(mealFilterModel);

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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMeal(Guid id, [FromBody] MealUpdateModel mealUpdateModel)
        {
            try
            {
                var result = await _mealService.UpdateMeal(id, mealUpdateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMeal(Guid id)
        {
            try
            {
                var result = await _mealService.DeleteMeal(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
