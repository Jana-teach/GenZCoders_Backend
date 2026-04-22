using GenZCoders.DTOs.WeekDto;
using GenZCoders.Services.WeekService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeekController : ControllerBase
    {
        private readonly IWeekService _service;

        public WeekController(IWeekService service)
        {
            _service = service;
        }

        // GET: api/Week
        [HttpGet]
        public async Task<ActionResult<List<WeekDto>>> GetAll()
        {
            var weeks = await _service.GetAllAsync();
            return Ok(weeks);
        }

        // GET: api/Week/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeekDto>> GetById(int id)
        {
            var week = await _service.GetByIdAsync(id);
            if (week == null) return NotFound();
            return Ok(week);
        }

        // POST: api/Week
        [HttpPost]
        public async Task<ActionResult<WeekDto>> Create([FromBody] CreateWeekDto dto)
        {
            if (dto == null) return BadRequest();

            var week = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = week.Id }, week);
        }

        // PUT: api/Week/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWeekDto dto)
        {
            if (dto == null) return BadRequest();

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        // DELETE: api/Week/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
