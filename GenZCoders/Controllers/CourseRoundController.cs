using GenZCoders.DTOs.CourseRoundDto;
using GenZCoders.Services.CourseRoundService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseRoundController : ControllerBase
    {
        private readonly ICourseRoundService _service;

        public CourseRoundController(ICourseRoundService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var round = await _service.GetByIdAsync(id);
            if (round == null) return NotFound();
            return Ok(round);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseRoundDto dto)
        {
            var round = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = round.Id }, round);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, UpdateCourseRoundDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id:long}")]
        public async Task<IActionResult> Patch(long id, PatchCourseRoundDto dto)
        {
            var updated = await _service.PatchAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
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
