using GenZCoders.DTOs.CourseRoundAssignmentSubmissionDto;

namespace GenZCoders.Services.CourseRoundAssignmentSubmissionService;

public interface ICourseRoundAssignmentSubmissionService
{
    Task<CourseRoundAssignmentSubmissionResponseDto> CreateAsync(
        CreateCourseRoundAssignmentSubmissionRequestDto dto,
        CancellationToken cancellationToken = default);

    Task<CourseRoundAssignmentSubmissionResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CourseRoundAssignmentSubmissionResponseDto>> GetByAssignmentIdAsync(
        long assignmentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CourseRoundAssignmentSubmissionResponseDto>> GetByStudentIdAsync(
        long studentId,
        CancellationToken cancellationToken = default);

    Task PatchAsync(long id, PatchCourseRoundAssignmentSubmissionRequestDto dto, CancellationToken cancellationToken = default);
}
