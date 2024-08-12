using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.MenuModels;
using Polaby.Services.Models.UserMenuModels;
using Polaby.Services.Services;

namespace Polaby.API.Controllers
{
    [Route("api/v1/menus")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }


        [HttpPost()]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMenu([FromBody] List<MenuImportModel> menus)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _menuService.AddRangeMenu(menus);
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
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMenuByFilter([FromQuery] MenuFilterModel menuFilterModel)
        {
            try
            {
                var result = await _menuService.GetAllMenu(menuFilterModel);

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
        public async Task<IActionResult> UpdateMenu(Guid id, [FromBody] MenuUpdateModel menuUpdateModel)
        {
            try
            {
                var result = await _menuService.UpdateMenu(id, menuUpdateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMenu(Guid id)
        {
            try
            {
                var result = await _menuService.DeleteMenu(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("menu-meals")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMenuMeal(List<MenuMealCreateModel> menuMeals)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _menuService.AddRangeMenuMeal(menuMeals);
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

        [HttpDelete("menu-meals/{menuId}/{mealId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMenuMeal(Guid menuId, Guid mealId)
        {
            try
            {
                var result = await _menuService.DeleteMenuMeal(menuId, mealId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("user-menus")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> CreateUserMenu(List<UserMenuMCreateModel> models)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _menuService.AddRangeUserMenu(models);
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

        [HttpDelete("user-menus/{userId}/{menuId}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteUserMenu(Guid userId, Guid menuId)
        {
            try
            {
                var result = await _menuService.DeleteUserMenu(userId,menuId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("recommendations")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> GetMenuRecommendations([FromQuery] MenuRecommentFilterModel model)
        {
            try
            {
                var result = await _menuService.GetMenuRecommendations(model);
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
