using Microsoft.AspNetCore.Mvc;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.CommunityPostModels;

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
        public async Task<IActionResult> Create(CommunityPostCreateModel communityPostCreateModel)
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
    }
}
