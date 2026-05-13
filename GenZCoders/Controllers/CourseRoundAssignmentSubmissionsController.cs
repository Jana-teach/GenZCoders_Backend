using GenZCoders.DTOs.CourseRoundAssignmentSubmissionDto;
using GenZCoders.Services.CourseRoundAssignmentSubmissionService;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseRoundAssignmentSubmissionsController : ControllerBase
{
    private readonly ICourseRoundAssignmentSubmissionService _service;

    public CourseRoundAssignmentSubmissionsController(ICourseRoundAssignmentSubmissionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseRoundAssignmentSubmissionRequestDto dto, CancellationToken cancellationToken)
    {
        var created = await _service.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var item = await _service.GetByIdAsync(id, cancellationToken);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet("by-assignment/{assignmentId:long}")]
    public async Task<IActionResult> GetByAssignment(long assignmentId, CancellationToken cancellationToken)
    {
        var items = await _service.GetByAssignmentIdAsync(assignmentId, cancellationToken);
        return Ok(items);
    }

    [HttpGet("by-student/{studentId:long}")]
    public async Task<IActionResult> GetByStudent(long studentId, CancellationToken cancellationToken)
    {
        var items = await _service.GetByStudentIdAsync(studentId, cancellationToken);
        return Ok(items);
    }

    [HttpPatch("{id:long}")]
    public async Task<IActionResult> Patch(long id, [FromBody] PatchCourseRoundAssignmentSubmissionRequestDto dto, CancellationToken cancellationToken)
    {
        await _service.PatchAsync(id, dto, cancellationToken);
        return NoContent();
    }
}
