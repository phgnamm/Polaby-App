using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.IngredientSearchModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/ingredientSearch")]
    [ApiController]
    public class IngredientSearchController : ControllerBase
    {
        private readonly IIngredientSearchService _ingredientSearchService;

        public IngredientSearchController(IIngredientSearchService ingredientSearchService)
        {
            _ingredientSearchService = ingredientSearchService;
        }


        [HttpPost()]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateIngredientSearch([FromBody] IngredientSearchtImportModel ingredient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _ingredientSearchService.AddIngredientSearchWithNutrients(ingredient);
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
        public async Task<IActionResult> GetAllIngredientByFilter([FromQuery] IngredientSearchFilterModel ingredientFilterModel)
        {
            try
            {
                var result = await _ingredientSearchService.GetAllIngredient(ingredientFilterModel);

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
        public async Task<IActionResult> UpdateIngredient(Guid id, [FromBody] IngredientSearchUpdateModel ingredientUpdateModel)
        {
            try
            {
                var result = await _ingredientSearchService.UpdateIngredient(id, ingredientUpdateModel);
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
                var result = await _ingredientSearchService.DeleteIngredient(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _ingredientSearchService .GetById(id);
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
