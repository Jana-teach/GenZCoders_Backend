using GenZCoders.DTOs.CourseRoundAssignmentSubmissionDto;
using GenZCoders.Models;
using GenZCoders.Repos.CourseRoundAssignmentSubmissionRepo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime;

namespace GenZCoders.Services.CourseRoundAssignmentSubmissionService;

public class CourseRoundAssignmentSubmissionService : ICourseRoundAssignmentSubmissionService
{
    private readonly SchoolDbContext _context;
    private readonly ICourseRoundAssignmentSubmissionRepo _repo;

    public CourseRoundAssignmentSubmissionService(SchoolDbContext context, ICourseRoundAssignmentSubmissionRepo repo)
    {
        _context = context;
        _repo = repo;
    }

    private const int late = 67;
    private const int submitted = 65;
    public async Task<CourseRoundAssignmentSubmissionResponseDto> CreateAsync(
        CreateCourseRoundAssignmentSubmissionRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var assignment = await _context.CourseRoundAssignments.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == dto.AssignmentId, cancellationToken)
            ?? throw new ArgumentException($"Assignment {dto.AssignmentId} was not found.");

        await EnsureAccountExistsAsync(dto.StudentId, cancellationToken);

        //if (assignment.Deadline < DateTime.UtcNow)
        //    throw new InvalidOperationException("The assignment deadline has passed; new submissions are not accepted.");

        var duplicate = await _repo.ExistsForAssignmentAndStudentAsync(dto.AssignmentId, dto.StudentId, cancellationToken);
        if (duplicate)
            throw new InvalidOperationException("This student has already submitted for this assignment.");
        var now = DateTime.UtcNow;
        var isLate = assignment.Deadline < now;

        var entity = new CourseRoundAssignmentSubmission
        {
            AssignmentId = dto.AssignmentId,
            StudentId = dto.StudentId,
            SubmissionLink = dto.SubmissionLink.Trim(),
            SubmittedAt = DateTime.UtcNow,
            StatusId = isLate ? late : submitted
        };

        await _repo.AddAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        var created = await _repo.GetByIdAsync(entity.Id, cancellationToken)
            ?? throw new InvalidOperationException("Submission was created but could not be reloaded.");
        return MapToResponse(created);
    }

    public async Task<CourseRoundAssignmentSubmissionResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToResponse(entity);
    }

    public async Task<IReadOnlyList<CourseRoundAssignmentSubmissionResponseDto>> GetByAssignmentIdAsync(
        long assignmentId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _context.CourseRoundAssignments.AnyAsync(a => a.Id == assignmentId, cancellationToken);
        if (!exists)
            throw new KeyNotFoundException($"Assignment {assignmentId} was not found.");

        var list = await _repo.GetByAssignmentIdAsync(assignmentId, cancellationToken);
        return list.Select(MapToResponse).ToList();
    }

    public async Task<IReadOnlyList<CourseRoundAssignmentSubmissionResponseDto>> GetByStudentIdAsync(
        long studentId,
        CancellationToken cancellationToken = default)
    {
        await EnsureAccountExistsAsync(studentId, cancellationToken);
        var list = await _repo.GetByStudentIdAsync(studentId, cancellationToken);
        return list.Select(MapToResponse).ToList();
    }

    public async Task PatchAsync(long id, PatchCourseRoundAssignmentSubmissionRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (!dto.Grade.HasValue && !dto.StatusId.HasValue && dto.Feedback is null)
            throw new ArgumentException("Provide at least one of: grade, statusId, or feedback.");

        var entity = await _repo.GetByIdForUpdateAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Submission {id} was not found.");

        if (dto.StatusId.HasValue)
        {
            if (dto.StatusId.Value <= 0)
                throw new ArgumentException("StatusId must be a positive value when supplied.");
            await EnsureStatusExistsAsync(dto.StatusId.Value, cancellationToken);
            entity.StatusId = dto.StatusId;
        }

        if (dto.Grade.HasValue)
        {
            var max = entity.Assignment?.TotalGrade ?? await _context.CourseRoundAssignments
                .Where(a => a.Id == entity.AssignmentId)
                .Select(a => a.TotalGrade)
                .FirstAsync(cancellationToken);
            if (dto.Grade.Value < 0 || dto.Grade.Value > max)
                throw new ArgumentException($"Grade must be between 0 and the assignment total ({max}).");
            entity.Grade = dto.Grade.Value;
        }

        if (dto.Feedback is not null)
            entity.Feedback = dto.Feedback;

        await _repo.SaveChangesAsync(cancellationToken);
    }

    private static CourseRoundAssignmentSubmissionResponseDto MapToResponse(CourseRoundAssignmentSubmission s)
    {
        return new CourseRoundAssignmentSubmissionResponseDto
        {
            Id = s.Id,
            AssignmentId = s.AssignmentId,
            AssignmentTitle = s.Assignment?.Title,
            StudentId = s.StudentId,
            StudentName = ResolveDisplayName(s.Student) ?? string.Empty,
            SubmissionLink = s.SubmissionLink,
            SubmittedAt = s.SubmittedAt,
            Grade = s.Grade,
            Feedback = s.Feedback,
            StatusId = s.StatusId,
            //Status = s.Status.StatusName,
            StatusName = s.Status?.StatusName
        };
    }

    private static string? ResolveDisplayName(Account? account)
    {
        if (account == null) return null;
        if (!string.IsNullOrWhiteSpace(account.FullNameEn)) return account.FullNameEn.Trim();
        return string.IsNullOrWhiteSpace(account.FullNameAr) ? null : account.FullNameAr.Trim();
    }

    private async Task EnsureAccountExistsAsync(long accountId, CancellationToken cancellationToken)
    {
        var exists = await _context.Accounts.AnyAsync(a => a.Id == accountId, cancellationToken);
        if (!exists)
            throw new ArgumentException($"Account {accountId} does not exist.");
    }

    private async Task EnsureStatusExistsAsync(long statusId, CancellationToken cancellationToken)
    {
        var exists = await _context.Statuses.AnyAsync(s => s.Id == statusId, cancellationToken);
        if (!exists)
            throw new ArgumentException($"Status {statusId} does not exist.");
    }
}
