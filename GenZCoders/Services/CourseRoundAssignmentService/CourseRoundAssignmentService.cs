using GenZCoders.DTOs.CourseRoundAssignmentDto;
using GenZCoders.Models;
using GenZCoders.Repos.CourseRoundAssignmentRepo;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Services.CourseRoundAssignmentService;

public class CourseRoundAssignmentService : ICourseRoundAssignmentService
{
    private readonly SchoolDbContext _context;
    private readonly ICourseRoundAssignmentRepo _repo;

    public CourseRoundAssignmentService(SchoolDbContext context, ICourseRoundAssignmentRepo repo)
    {
        _context = context;
        _repo = repo;
    }

    public async Task<CourseRoundAssignmentResponseDto> CreateAsync(CreateCourseRoundAssignmentRequestDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureCourseRoundExistsAsync(dto.CourseRoundId, cancellationToken);
        await EnsureAccountExistsAsync(dto.InstructorId, cancellationToken);
        if (dto.StatusId.HasValue)
            await EnsureStatusExistsAsync(dto.StatusId.Value, cancellationToken);
        if (dto.CourseMaterialId.HasValue)
            await EnsureCourseMaterialInRoundAsync(dto.CourseMaterialId.Value, dto.CourseRoundId, cancellationToken);

        var entity = new CourseRoundAssignment
        {
            Title = dto.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
            AssignmentLink = string.IsNullOrWhiteSpace(dto.AssignmentLink) ? null : dto.AssignmentLink.Trim(),
            Deadline = dto.Deadline,
            TotalGrade = dto.TotalGrade,
            CourseRoundId = dto.CourseRoundId,
            InstructorId = dto.InstructorId,
            CourseMaterialId = dto.CourseMaterialId,
            StatusId = dto.StatusId,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(entity, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        var created = await _repo.GetByIdAsync(entity.Id, cancellationToken)
            ?? throw new InvalidOperationException("Assignment was created but could not be reloaded.");
        return MapToResponse(created);
    }

    public async Task<CourseRoundAssignmentResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : MapToResponse(entity);
    }

    public async Task<IReadOnlyList<CourseRoundAssignmentResponseDto>> GetAllAsync(long? courseRoundId, CancellationToken cancellationToken = default)
    {
        var list = await _repo.GetAllAsync(courseRoundId, cancellationToken);
        return list.Select(MapToResponse).ToList();
    }

    public async Task<CourseRoundAssignmentResponseDto> UpdateAsync(long id, UpdateCourseRoundAssignmentRequestDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdForUpdateAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Assignment {id} was not found.");

        await EnsureCourseRoundExistsAsync(dto.CourseRoundId, cancellationToken);
        await EnsureAccountExistsAsync(dto.InstructorId, cancellationToken);
        if (dto.StatusId.HasValue)
            await EnsureStatusExistsAsync(dto.StatusId.Value, cancellationToken);
        if (dto.CourseMaterialId.HasValue)
            await EnsureCourseMaterialInRoundAsync(dto.CourseMaterialId.Value, dto.CourseRoundId, cancellationToken);

        entity.Title = dto.Title.Trim();
        entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
        entity.AssignmentLink = string.IsNullOrWhiteSpace(dto.AssignmentLink) ? null : dto.AssignmentLink.Trim();
        entity.Deadline = dto.Deadline;
        entity.TotalGrade = dto.TotalGrade;
        entity.CourseRoundId = dto.CourseRoundId;
        entity.InstructorId = dto.InstructorId;
        entity.CourseMaterialId = dto.CourseMaterialId;
        entity.StatusId = dto.StatusId;

        _repo.Update(entity);
        await _repo.SaveChangesAsync(cancellationToken);

        var updated = await _repo.GetByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException("Assignment could not be reloaded after update.");
        return MapToResponse(updated);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await _repo.GetByIdForUpdateAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Assignment {id} was not found.");

        _repo.Remove(entity);
        await _repo.SaveChangesAsync(cancellationToken);
    }

    private static CourseRoundAssignmentResponseDto MapToResponse(CourseRoundAssignment a)
    {
        return new CourseRoundAssignmentResponseDto
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            AssignmentLink = a.AssignmentLink,
            Deadline = a.Deadline,
            TotalGrade = a.TotalGrade,
            CourseRoundId = a.CourseRoundId,
            InstructorId = a.InstructorId,
            InstructorName = ResolveDisplayName(a.Instructor),
            CourseMaterialId = a.CourseMaterialId,
            StatusId = a.StatusId,
            StatusName = a.Status?.StatusName,
            CreatedAt = a.CreatedAt
        };
    }

    private static string? ResolveDisplayName(Account? account)
    {
        if (account == null) return null;
        if (!string.IsNullOrWhiteSpace(account.FullNameEn)) return account.FullNameEn.Trim();
        return string.IsNullOrWhiteSpace(account.FullNameAr) ? null : account.FullNameAr.Trim();
    }

    private async Task EnsureCourseRoundExistsAsync(long courseRoundId, CancellationToken cancellationToken)
    {
        var exists = await _context.CourseRounds.AnyAsync(cr => cr.Id == courseRoundId, cancellationToken);
        if (!exists)
            throw new ArgumentException($"Course round {courseRoundId} does not exist.");
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

    private async Task EnsureCourseMaterialInRoundAsync(long courseMaterialId, long courseRoundId, CancellationToken cancellationToken)
    {
        var ok = await _context.CourseMaterials.AnyAsync(
            m => m.Id == courseMaterialId && m.CourseRoundId == courseRoundId,
            cancellationToken);
        if (!ok)
            throw new ArgumentException(
                $"Course material {courseMaterialId} was not found for course round {courseRoundId}.");
    }
}
