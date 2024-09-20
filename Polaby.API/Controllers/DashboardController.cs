using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polaby.Services.Interfaces;
using Polaby.Services.Models.DashboardModels;

namespace Polaby.API.Controllers
{
    [Route("api/v1/dashboards")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAdminDashboard([FromQuery] DashboardFilterModel dashboardFilterModel)
        {
            try
            {
                var result = await _dashboardService.GetAdminDashboard(dashboardFilterModel);
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
