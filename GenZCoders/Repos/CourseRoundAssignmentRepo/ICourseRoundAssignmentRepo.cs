using GenZCoders.Models;

namespace GenZCoders.Repos.CourseRoundAssignmentRepo;

public interface ICourseRoundAssignmentRepo
{
    Task<CourseRoundAssignment?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<CourseRoundAssignment?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default);
    Task<List<CourseRoundAssignment>> GetAllAsync(long? courseRoundId, CancellationToken cancellationToken = default);
    Task AddAsync(CourseRoundAssignment entity, CancellationToken cancellationToken = default);
    void Update(CourseRoundAssignment entity);
    void Remove(CourseRoundAssignment entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
