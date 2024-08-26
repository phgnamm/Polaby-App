using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.ExpertRegistrationModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/expert-registrations")]
    [ApiController]
    public class ExpertRegistrationController : ControllerBase
    {
        private readonly IExpertRegistrationService _expertRegistrationService;

        public ExpertRegistrationController(IExpertRegistrationService expertRegistrationService)
        {
            _expertRegistrationService = expertRegistrationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpertRegistration(
            [FromBody] ExpertRegistrationCreateModel expertRegistrationCreateModel)
        {
            try
            {
                var result = await _expertRegistrationService.CreateExpertRegistration(expertRegistrationCreateModel);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpertRegistration(Guid id)
        {
            try
            {
                var result = await _expertRegistrationService.GetExpertRegistration(id);
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

        [HttpGet("emails/{email}")]
        public async Task<IActionResult> GetExpertRegistration(string email)
        {
            try
            {
                var result = await _expertRegistrationService.GetExpertRegistration(email);
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

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllExpertRegistrations(
            [FromQuery] ExpertRegistrationFilterModel expertRegistrationFilterModel)
        {
            try
            {
                var result = await _expertRegistrationService.GetAllExpertRegistration(expertRegistrationFilterModel);
                var metadata = new
                {
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                };

                Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpertRegistration(Guid id,
            [FromBody] ExpertRegistrationUpdateModel expertRegistrationUpdateModel)
        {
            try
            {
                var result =
                    await _expertRegistrationService.UpdateExpertRegistration(id, expertRegistrationUpdateModel);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpertRegistration(Guid id)
        {
            try
            {
                var result = await _expertRegistrationService.DeleteExpertRegistration(id);
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

        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateExpertRegistrationStatus(Guid id,
            [FromBody] ExpertRegistrationUpdateStatusModel expertRegistrationUpdateStatusModel)
        {
            try
            {
                var result =
                    await _expertRegistrationService.UpdateExpertRegistrationStatus(id,
                        expertRegistrationUpdateStatusModel);
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