﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polaby.Repositories.Enums;
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

        [HttpPost("create")]
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

        [HttpPut("{id}/update")]
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

        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> DeleteProject(Guid id)
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
    }
}
