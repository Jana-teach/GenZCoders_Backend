using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.CourseRoundAssignmentSubmissionRepo;

public class CourseRoundAssignmentSubmissionRepo : ICourseRoundAssignmentSubmissionRepo
{
    private readonly SchoolDbContext _context;

    public CourseRoundAssignmentSubmissionRepo(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<CourseRoundAssignmentSubmission?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.CourseRoundAssignmentSubmissions
            .AsNoTracking()
            .Include(s => s.Student)
            .Include(s => s.Assignment)
            .Include(s => s.Status)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<CourseRoundAssignmentSubmission?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.CourseRoundAssignmentSubmissions
            .Include(s => s.Student)
            .Include(s => s.Assignment)
            .Include(s => s.Status)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<List<CourseRoundAssignmentSubmission>> GetByAssignmentIdAsync(long assignmentId, CancellationToken cancellationToken = default)
    {
        return await _context.CourseRoundAssignmentSubmissions
            .AsNoTracking()
            .Include(s => s.Student)
            .Include(s => s.Status)
            .Where(s => s.AssignmentId == assignmentId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CourseRoundAssignmentSubmission>> GetByStudentIdAsync(long studentId, CancellationToken cancellationToken = default)
    {
        return await _context.CourseRoundAssignmentSubmissions
            .AsNoTracking()
            .Include(s => s.Assignment)
            .Include(s => s.Status)
            .Where(s => s.StudentId == studentId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsForAssignmentAndStudentAsync(long assignmentId, long studentId, CancellationToken cancellationToken = default)
    {
        return _context.CourseRoundAssignmentSubmissions
            .AnyAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId, cancellationToken);
    }

    public async Task AddAsync(CourseRoundAssignmentSubmission entity, CancellationToken cancellationToken = default)
    {
        await _context.CourseRoundAssignmentSubmissions.AddAsync(entity, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
