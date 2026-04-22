using GenZCoders.DTOs.CourseRoundStudentDto;
using GenZCoders.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseRoundStudentController : ControllerBase
    {
        private const long PaidStatusId = 42;
        private const long AcceptedStatusId = 17;

        private readonly SchoolDbContext _context;

        public CourseRoundStudentController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet("course-round/{courseRoundId:long}")]
        public async Task<IActionResult> GetByCourseRoundId(long courseRoundId)
        {
            if (courseRoundId <= 0) return BadRequest(new { message = "courseRoundId must be greater than 0." });

            var apps = await _context.Applications
                .Include(a => a.Account)
                .Where(a => a.CourseRoundId == courseRoundId && (a.StatusId == PaidStatusId || a.StatusId == AcceptedStatusId))
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();

            var result = apps.Select(a => new CourseRoundStudentDto
            {
                Id = a.Id,
                CourseRoundId = a.CourseRoundId,
                StudentId = a.AccountId,
                StudentName = a.Account != null ? (a.Account.FullNameEn ?? a.Account.FullNameAr ?? string.Empty) : string.Empty,
                StudentEmail = a.Account != null ? a.Account.Email : string.Empty,
                AssignedAt = a.ApplicationDate.ToString("O"),
                CompletedAt = null,
                IsActive = a.StatusId == PaidStatusId,
            }).ToList();

            return Ok(result);
        }

        [HttpGet("student/{studentId:long}")]
        public async Task<IActionResult> GetByStudentId(long studentId)
        {
            if (studentId <= 0) return BadRequest(new { message = "studentId must be greater than 0." });

            var apps = await _context.Applications
                .Include(a => a.Account)
                .Where(a => a.AccountId == studentId && (a.StatusId == PaidStatusId || a.StatusId == AcceptedStatusId))
                .OrderByDescending(a => a.ApplicationDate)
                .ToListAsync();

            var result = apps.Select(a => new CourseRoundStudentDto
            {
                Id = a.Id,
                CourseRoundId = a.CourseRoundId,
                StudentId = a.AccountId,
                StudentName = a.Account != null ? (a.Account.FullNameEn ?? a.Account.FullNameAr ?? string.Empty) : string.Empty,
                StudentEmail = a.Account != null ? a.Account.Email : string.Empty,
                AssignedAt = a.ApplicationDate.ToString("O"),
                CompletedAt = null,
                IsActive = a.StatusId == PaidStatusId,
            }).ToList();

            return Ok(result);
        }

        [HttpPost("course-round/{courseRoundId:long}/assign")]
        public async Task<IActionResult> AssignStudent(long courseRoundId, [FromBody] AssignStudentRequest payload)
        {
            if (courseRoundId <= 0) return BadRequest(new { message = "courseRoundId must be greater than 0." });
            if (payload is null) return BadRequest(new { message = "Request body is required." });
            if (payload.StudentId <= 0) return BadRequest(new { message = "studentId must be greater than 0." });

            var app = await _context.Applications
                .Include(a => a.Account)
                .FirstOrDefaultAsync(a => a.CourseRoundId == courseRoundId && a.AccountId == payload.StudentId);

            if (app == null) return NotFound(new { message = "Application not found for this student and course round." });

            app.StatusId = PaidStatusId;
            await _context.SaveChangesAsync();

            var dto = new CourseRoundStudentDto
            {
                Id = app.Id,
                CourseRoundId = app.CourseRoundId,
                StudentId = app.AccountId,
                StudentName = app.Account != null ? (app.Account.FullNameEn ?? app.Account.FullNameAr ?? string.Empty) : string.Empty,
                StudentEmail = app.Account != null ? app.Account.Email : string.Empty,
                AssignedAt = app.ApplicationDate.ToString("O"),
                CompletedAt = null,
                IsActive = true,
            };

            return Ok(dto);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> UnassignStudent(long id)
        {
            if (id <= 0) return BadRequest(new { message = "id must be greater than 0." });

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Id == id);
            if (app == null) return NotFound(new { message = "Assignment not found." });

            app.StatusId = AcceptedStatusId;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
