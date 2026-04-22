using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GenZCoders.Repos.CourseRoundRepo
{
    public class CourseRoundRepo : ICourseRoundRepo
    {
        private readonly SchoolDbContext _context;

        public CourseRoundRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseRound>> GetAllAsync()
            => await _context.CourseRounds
            .Include(s=> s.Status)
            .Include(x=> x.CourseRoundInstructors).ThenInclude(c=> c.Instructor)
            .Include(r => r.GroupedRounds)
            .ToListAsync();

        public async Task<CourseRound?> GetByIdAsync(long id)
            => await _context.CourseRounds
            .Include(r => r.Course)
            .Include(r => r.Status)
            .Include(r => r.CourseRoundInstructors).ThenInclude(x => x.Instructor)
            .Include(r => r.CourseMaterials).ThenInclude(cm => cm.Week)
            .Include(r => r.GroupedRounds)
            .FirstOrDefaultAsync(r => r.Id == id);

        public async Task AddAsync(CourseRound round)
            => await _context.CourseRounds.AddAsync(round);

        public void Update(CourseRound round)
            => _context.CourseRounds.Update(round);

        public void Remove(CourseRound round)
            => _context.CourseRounds.Remove(round);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }

}
