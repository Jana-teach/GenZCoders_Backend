using GenZCoders.DTOs.CourseRoundInstructor;
using GenZCoders.Services.CourseRoundInstructorService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers
{
    [ApiController]
    [Route("api/course-round-instructors")]
    public class CourseRoundInstructorController : ControllerBase
    {
        private readonly ICourseRoundInstructorService _service;

        public CourseRoundInstructorController(ICourseRoundInstructorService service)
        {
            _service = service;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignInstructors([FromBody] AssignInstructorsDto dto)
        {
            if (!dto.InstructorIds.Any())
                return BadRequest("InstructorIds is required.");

            await _service.AssignInstructorsAsync(dto);
            return Ok(new { message = "Instructors assigned successfully." });
        }


        [HttpGet("instructor/{instructorId}")]
        public async Task<IActionResult> GetInstructorCourseRounds(long instructorId)
        {
            var result = await _service.GetInstructorCourseRoundsAsync(instructorId);
            return Ok(result);
        }
    }

}
