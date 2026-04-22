using GenZCoders.DTOs.AccountsDto;
using GenZCoders.Services.AccountService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorsController : ControllerBase
    {
        private readonly IAccountService _service;

        public InstructorsController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet("co-instructors")]
        public async Task<ActionResult<List<InstructorDto>>> GetCoInstructors()
        {
            var instructors = await _service.GetCoInstructorsAsync();
            return Ok(instructors);
        }
    }
}
