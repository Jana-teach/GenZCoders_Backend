using GenZCoders.DTOs.ApplicationDto;
using GenZCoders.Services.ApplicationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers
{
    [ApiController]
    [Route("api/applications")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;

        public ApplicationController(IApplicationService service)
        {
            _service = service;
        }

        /// <param name="accountId">Student's account ID (from auth). Optional if from JWT.</param>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromQuery] long? accountId,
            [FromBody] CreateApplicationDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Request body is required." });
            if (!accountId.HasValue || accountId.Value <= 0)
                return BadRequest(new { message = "accountId is required and must be a positive number." });
            var result = await _service.CreateAsync(accountId.Value, dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] long? accountId)
        {
            var items = await _service.GetAllAsync();
            if (accountId.HasValue)
            {
                items = items.Where(x => x.AccountId == accountId.Value).ToList();
            }
            return Ok(items);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPatch("{id:long}/status")]
        public async Task<IActionResult> PatchStatus(
            long id,
            [FromBody] PatchApplicationStatusDto dto)
        {
            var updated = await _service.PatchStatusAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id:long}/course-round")]
        public async Task<IActionResult> PatchCourseRound(
            long id,
            [FromBody] PatchApplicationCourseRoundDto dto)
        {
            try
            {
                var updated = await _service.PatchCourseRoundAsync(id, dto);
                if (!updated) return NotFound();
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }

}
