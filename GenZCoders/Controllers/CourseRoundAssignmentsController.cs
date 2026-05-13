using GenZCoders.DTOs.CourseRoundAssignmentDto;
using GenZCoders.DTOs.CourseRoundAssignmentSubmissionDto;
using GenZCoders.Services.CourseRoundAssignmentService;
using GenZCoders.Services.CourseRoundAssignmentSubmissionService;
using Microsoft.AspNetCore.Mvc;

namespace GenZCoders.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseRoundAssignmentsController : ControllerBase
{
    private readonly ICourseRoundAssignmentService _assignmentService;
    private readonly ICourseRoundAssignmentSubmissionService _submissionService;

    public CourseRoundAssignmentsController(
        ICourseRoundAssignmentService assignmentService,
        ICourseRoundAssignmentSubmissionService submissionService)
    {
        _assignmentService = assignmentService;
        _submissionService = submissionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] long? courseRoundId, CancellationToken cancellationToken)
    {
        var items = await _assignmentService.GetAllAsync(courseRoundId, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        var item = await _assignmentService.GetByIdAsync(id, cancellationToken);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseRoundAssignmentRequestDto dto, CancellationToken cancellationToken)
    {
        var created = await _assignmentService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateCourseRoundAssignmentRequestDto dto, CancellationToken cancellationToken)
    {
        var updated = await _assignmentService.UpdateAsync(id, dto, cancellationToken);
        return Ok(updated);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _assignmentService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>Lists submissions for an assignment (includes student display name).</summary>
    [HttpGet("{id:long}/submissions")]
    public async Task<IActionResult> GetSubmissionsForAssignment(long id, CancellationToken cancellationToken)
    {
        var items = await _submissionService.GetByAssignmentIdAsync(id, cancellationToken);
        return Ok(items);
    }
}
