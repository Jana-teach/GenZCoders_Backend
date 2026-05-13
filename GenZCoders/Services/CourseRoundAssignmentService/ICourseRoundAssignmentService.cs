using GenZCoders.DTOs.CourseRoundAssignmentDto;

namespace GenZCoders.Services.CourseRoundAssignmentService;

public interface ICourseRoundAssignmentService
{
    Task<CourseRoundAssignmentResponseDto> CreateAsync(CreateCourseRoundAssignmentRequestDto dto, CancellationToken cancellationToken = default);
    Task<CourseRoundAssignmentResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CourseRoundAssignmentResponseDto>> GetAllAsync(long? courseRoundId, CancellationToken cancellationToken = default);
    Task<CourseRoundAssignmentResponseDto> UpdateAsync(long id, UpdateCourseRoundAssignmentRequestDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
