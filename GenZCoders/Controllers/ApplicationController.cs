using GenZCoders.DTOs.ApplicationDto;
using GenZCoders.DTOs.ExamsDto;
using GenZCoders.Services.ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _service;

        public ApplicationController(IApplicationService service)
        {
            _service = service;
        }

        [HttpGet("exam-questions/{courseRoundId:long}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ExamQuestionDto>>> GetExamQuestions(long courseRoundId)
        {
            var questions = await _service.GetExamQuestionsAsync(courseRoundId);

            if (!questions.Any())
                return NotFound(new { message = "No questions found for this course round" });

            return Ok(questions);
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationDto>> Create([FromBody] CreateApplicationDto dto)
        {
            try
            {
                //var accountId = GetCurrentAccountId();
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{applicationId:long}/exam-answers")]
        public async Task<ActionResult> SubmitExamAnswers(long applicationId, [FromBody] List<ExamAnswerItemDto> answers)
        {
            try
            {
                var accountId = GetCurrentAccountId();
                var app = await _service.GetByIdAsync(applicationId);

                if (app == null) return NotFound(new { message = "Application not found" });
                if (app.AccountId != accountId) return Forbid();

                var success = await _service.SubmitExamAnswersAsync(accountId, app.CourseRoundId, answers);

                return success
                    ? Ok(new { message = "Answers submitted successfully" })
                    : BadRequest(new { message = "Failed to submit answers" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ApplicationDto>> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationDto>>> GetAll()
        {
            var results = await _service.GetAllAsync();
            return Ok(results);
        }

        [HttpDelete("{id:long}")]
        public async Task<ActionResult> Delete(long id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/status")]
        public async Task<ActionResult> PatchStatus(long id, [FromBody] PatchApplicationStatusDto dto)
        {
            var success = await _service.PatchStatusAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/course-round")]
        public async Task<ActionResult> PatchCourseRound(long id, [FromBody] PatchApplicationCourseRoundDto dto)
        {
            var success = await _service.PatchCourseRoundAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        private long GetCurrentAccountId()
        {
            var userIdClaim = User.FindFirst("AccountId")?.Value
                ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            return long.Parse(userIdClaim!);
        }
    }
}