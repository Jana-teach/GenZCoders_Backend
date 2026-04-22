using GenZCoders.DTOs.CourseRoundInstructor;
using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.CourseRoundInstructorRepo
{
    public class CourseRoundInstructorRepository : ICourseRoundInstructorRepository
    {
        private readonly SchoolDbContext _context;

        public CourseRoundInstructorRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task RemoveByCourseRoundAsync(long courseRoundId)
        {
            var existing = await _context.CourseRoundInstructors
                .Where(x => x.CourseRoundId == courseRoundId)
                .ToListAsync();

            _context.CourseRoundInstructors.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<CourseRoundInstructor> entities)
        {
            await _context.CourseRoundInstructors.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<List<InstructorCourseRoundDto>> GetByInstructorAsync(long instructorId)
        {
            return await _context.CourseRoundInstructors
                .Where(x => x.InstructorId == instructorId)
                .Include(x => x.CourseRound)
                    .ThenInclude(cr => cr.Course)
                .Select(x => new InstructorCourseRoundDto
                {
                    CourseRoundId = x.CourseRound.Id,
                    CourseName = x.CourseRound.Course.Title,
                    RoundNumber = x.CourseRound.RoundNumber,
                    StartDate = x.CourseRound.StartDate,
                    EndDate = x.CourseRound.EndDate,
                    Price = x.CourseRound.Price
                })
                .ToListAsync();
        }
    }

}
