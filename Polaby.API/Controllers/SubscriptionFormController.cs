using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.SubscriptionFormModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/subscription-forms")]
    [ApiController]
    public class SubscriptionFormController : ControllerBase
    {
        readonly ISubscriptionFormService _subscriptionFormService;

        public SubscriptionFormController(ISubscriptionFormService subscriptionFormService)
        {
            _subscriptionFormService = subscriptionFormService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromForm] SubscriptionFormCreateModel model)
        {
            try
            {
                var result = await _subscriptionFormService.CreateSubscriptionForm(model);
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