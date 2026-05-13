using GenZCoders.Models;

namespace GenZCoders.Repos.CourseRoundAssignmentSubmissionRepo;

public interface ICourseRoundAssignmentSubmissionRepo
{
    Task<CourseRoundAssignmentSubmission?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<CourseRoundAssignmentSubmission?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);
    Task<List<CourseRoundAssignmentSubmission>> GetByAssignmentIdAsync(long assignmentId, CancellationToken cancellationToken = default);
    Task<List<CourseRoundAssignmentSubmission>> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default);
    Task<bool> ExistsForAssignmentAndStudentAsync(long assignmentId, long studentId, CancellationToken cancellationToken = default);
    Task AddAsync(CourseRoundAssignmentSubmission entity, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
