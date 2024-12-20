using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.AccountModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddAccounts([FromBody] List<AccountRegisterModel> accountRegisterModels)
        {
            try
            {
                var result = await _accountService.AddAccounts(accountRegisterModels);
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
        public async Task<IActionResult> GetAccount(Guid id)
        {
            try
            {
                var result = await _accountService.GetAccount(id);
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
        public async Task<IActionResult> GetAllAccounts([FromQuery] AccountFilterModel accountFilterModel)
        {
            try
            {
                var result = await _accountService.GetAllAccounts(accountFilterModel);
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

        // [Authorize(Roles = "User")]
        [HttpPut("{id}/user")]
        public async Task<IActionResult> UpdateAccountUser(Guid id,
            [FromBody] AccountUserUpdateModel accountUserUpdateModel)
        {
            try
            {
                var result = await _accountService.UpdateAccountUser(id, accountUserUpdateModel);
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

        // [Authorize(Roles = "Expert")]
        [HttpPut("{id}/expert")]
        public async Task<IActionResult> UpdateAccountExpert(Guid id,
            [FromBody] AccountExpertUpdateModel accountExpertUpdateModel)
        {
            try
            {
                var result = await _accountService.UpdateAccountExpert(id, accountExpertUpdateModel);
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(Guid id,
            [FromBody] AccountUpdateModel accountUpdateModel)
        {
            try
            {
                var result = await _accountService.UpdateAccount(id, accountUpdateModel);
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try
            {
                var result = await _accountService.DeleteAccount(id);
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
        [HttpPut("{id}/restore")]
        public async Task<IActionResult> RestoreAccount(Guid id)
        {
            try
            {
                var result = await _accountService.RestoreAccount(id);
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

        [HttpPost("{id}/check-password")]
        public async Task<IActionResult> CheckPassword(Guid id, [FromBody]AccountCheckPasswordModel accountCheckPasswordModel)
        {
            try
            {
                var result = await _accountService.CheckPassword(id, accountCheckPasswordModel);
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
        
        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] AccountCreatePaymentModel model)
        {
            try
            {
                var result = await _accountService.CreatePayment(model);
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
        
        // [Authorize(Roles = "User")]
        [HttpPost("subscription")]
        public async Task<IActionResult> UpdateSubscription(Guid id, [FromBody]AccountUpdateSubscriptionModel model)
        {
            try
            {
                var result = await _accountService.UpdateSubscription(model);
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