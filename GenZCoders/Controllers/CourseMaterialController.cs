using GenZCoders.DTOs.CourseMaterialDto;
using GenZCoders.Models;
using GenZCoders.Models;
using GenZCoders.Services.CourseMaterialService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseMaterialController : ControllerBase
    {
        private readonly ICourseMaterialService _service;

        public CourseMaterialController(ICourseMaterialService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseMaterial>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseMaterial>> GetById(long id)
        {
            var material = await _service.GetByIdAsync(id);
            if (material == null) return NotFound();
            return Ok(material);
        }

        [HttpPost]
        public async Task<ActionResult<CourseMaterial>> Create(CreateCourseMaterialDto dto)
        {
            var material = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = material.Id }, material);
        }

        [HttpPost("zoom")]
        public async Task<ActionResult<CourseMaterial>> CreateZoom(CreateZoomCourseMaterialDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    return BadRequest(new { message = "Meeting topic (Title) is required" });
                if (dto.DurationMinutes < 15 || dto.DurationMinutes > 300)
                    return BadRequest(new { message = "Duration must be between 15 and 300 minutes" });

                var material = await _service.CreateZoomMaterialAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = material.Id }, material);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message ?? "Failed to create Zoom meeting" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, UpdateCourseMaterialDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(long id, PatchCourseMaterialDto dto)
        {
            var updated = await _service.PatchAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}

