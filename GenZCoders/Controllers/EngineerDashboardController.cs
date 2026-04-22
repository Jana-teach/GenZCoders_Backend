using GenZCoders.DTOs.DashBoardsDto;
using GenZCoders.Services.EngineerDashboardService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineerDashboardController : ControllerBase
    {
        private readonly IEngineerDashboardService _service;

        public EngineerDashboardController(IEngineerDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<EngineerDashboardDto>> GetDashboard()
        {
            var dashboard = await _service.GetDashboardAsync();
            return Ok(dashboard);   
        }
    }
}
