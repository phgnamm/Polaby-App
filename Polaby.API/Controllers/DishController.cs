﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.DishModels;
using Polaby.Services.Models.MenuModels;
using Polaby.Services.Services;

namespace Polaby.API.Controllers
{
    [Route("api/v1/dishes")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }


        [HttpPost()]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDish([FromBody] List<DishImportModel> dishes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _dishService.AddRangeDish(dishes);
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

        [HttpPost("dish-ingredients")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDishIngredient(DishIngredientCreateModel dishIngredient)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _dishService.AddDishIngredient(dishIngredient);
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
        public async Task<IActionResult> GetAllDishByFilter([FromQuery] DishFilterModel dishFilterModel)
        {
            try
            {
                var result = await _dishService.GetAllDish(dishFilterModel);

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
        public async Task<IActionResult> UpdateDish(Guid id, [FromBody] DishUpdateModel dishUpdateModel)
        {
            try
            {
                var result = await _dishService.UpdateDish(id, dishUpdateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDish(Guid id)
        {
            try
            {
                var result = await _dishService.DeleteDish(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("dish-ingredients/{dishId}/{ingredientId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDishIngredient(Guid dishId, Guid ingredientId)
        {
            try
            {
                var result = await _dishService.DeleteDishIngredient(dishId, ingredientId);
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
                var result = await _dishService.GetById(id);
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
