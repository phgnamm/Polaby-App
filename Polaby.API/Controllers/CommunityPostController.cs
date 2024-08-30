using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Repositories.Enums;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommunityPostModels;
using Polaby.Services.Services;

namespace Polaby.API.Controllers
{
    [Route("api/v1/community-posts")]
    [ApiController]
    public class CommunityPostController : ControllerBase
    {
        private readonly ICommunityPostService _communityPostService;

        public CommunityPostController(ICommunityPostService communityPostService)
        {
            _communityPostService = communityPostService;
        }

        [HttpPost()]
        //[Authorize(Roles = "User, Expert")]
        public async Task<IActionResult> Create([FromBody] CommunityPostCreateModel communityPostCreateModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _communityPostService.Create(communityPostCreateModel);
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
        //[Authorize(Roles = "User, Expert")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CommunityPostUpdateModel communityPostUpdateModel)
        {
            try
            {
                var result = await _communityPostService.Update(id, communityPostUpdateModel);

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
        //[Authorize(Roles = "Admin, User, Expert")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _communityPostService.Delete(id);
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
        //[Authorize(Roles = "Admin, User, Expert")]
        public async Task<IActionResult> GetProjectByFilter([FromQuery] CommunityPostFilterModel communityFilterModel)
        {
            try
            {
                var result = await _communityPostService.GetAllCommunityPosts(communityFilterModel);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommunityPost(Guid id)
        {
            try
            {
                var result = await _communityPostService.GetById(id);
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
