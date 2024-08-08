using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.IngredientModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/ingredients")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredeintService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredeintService = ingredientService;
        }


        [HttpPost()]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMeal(List<IngredientImportModel> ingredients)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _ingredeintService.AddRangeIngredient(ingredients);
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
        public async Task<IActionResult> GetAllIngredientByFilter([FromQuery] IngredientFilterModel ingredientFilterModel)
        {
            try
            {
                var result = await _ingredeintService.GetAllIngredient(ingredientFilterModel);

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
        public async Task<IActionResult> UpdateIngredient(Guid id, [FromBody] IngredientUpdateModel ingredientUpdateModel)
        {
            try
            {
                var result = await _ingredeintService.UpdateIngredient(id, ingredientUpdateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteIngredient(Guid id)
        {
            try
            {
                var result = await _ingredeintService.DeleteIngredient(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
