using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.CourseRoundAssignmentRepo;

public class CourseRoundAssignmentRepo : ICourseRoundAssignmentRepo
{
    private readonly SchoolDbContext _context;

    public CourseRoundAssignmentRepo(SchoolDbContext context)
    {
        _context = context;
    }

    public async Task<CourseRoundAssignment?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.CourseRoundAssignments
            .AsNoTracking()
            .Include(a => a.CourseRound)
            .Include(a => a.Instructor)
            .Include(a => a.CourseMaterial)
            .Include(a => a.Status)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<CourseRoundAssignment?> GetByIdForUpdateAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.CourseRoundAssignments
            .Include(a => a.CourseRound)
            .Include(a => a.Instructor)
            .Include(a => a.CourseMaterial)
            .Include(a => a.Status)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<List<CourseRoundAssignment>> GetAllAsync(long? courseRoundId, CancellationToken cancellationToken = default)
    {
        var query = _context.CourseRoundAssignments
            .AsNoTracking()
            .Include(a => a.CourseRound)
            .Include(a => a.Instructor)
            .Include(a => a.CourseMaterial)
            .Include(a => a.Status)
            .AsQueryable();

        if (courseRoundId.HasValue)
            query = query.Where(a => a.CourseRoundId == courseRoundId.Value);

        return await query
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(CourseRoundAssignment entity, CancellationToken cancellationToken = default)
    {
        await _context.CourseRoundAssignments.AddAsync(entity, cancellationToken);
    }

    public void Update(CourseRoundAssignment entity)
    {
        _context.CourseRoundAssignments.Update(entity);
    }

    public void Remove(CourseRoundAssignment entity)
    {
        _context.CourseRoundAssignments.Remove(entity);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
