using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.EngineerDashboardRepo
{
    public class EngineerDashBoardRepo : IEngineerDashboardRepo
    {
        private readonly SchoolDbContext _context;

        public EngineerDashBoardRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<long> GetTotalCoursesAsync()
        {
            return await _context.Courses.LongCountAsync();
        }

        public async Task<long> GetTotalCourseRoundsAsync()
        {
            return await _context.CourseRounds.LongCountAsync();
        }

        public async Task<long> GetApplicationsCountByStatusAsync(long statusId)
        {
            return await _context.Applications
                .Where(a => a.StatusId == statusId)
                .LongCountAsync();
        }

        public async Task<List<(long CourseId, string Title, long ApplicationCount)>> GetTopCoursesAsync(int top = 3)
        {
            var topCourses = await _context.Courses
                .Select(c => new
                {
                    CourseId = c.Id,
                    Title = c.Title,
                    // Calculate the count at the database level
                    ApplicationCount = (long)c.CourseRounds.SelectMany(cr => cr.Applications).Count()
                })
                .OrderByDescending(c => c.ApplicationCount)
                .Take(top)
                .ToListAsync();

            // Map the anonymous objects from the database result to the Tuple format
            return topCourses
                .Select(c => (c.CourseId, c.Title, c.ApplicationCount))
                .ToList();
        }
    }
}

