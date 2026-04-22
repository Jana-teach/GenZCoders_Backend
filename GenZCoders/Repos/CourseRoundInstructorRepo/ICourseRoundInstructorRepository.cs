using GenZCoders.DTOs.CourseRoundInstructor;
using GenZCoders.Models;

namespace GenZCoders.Repos.CourseRoundInstructorRepo
{
    public interface ICourseRoundInstructorRepository
    {
       public Task RemoveByCourseRoundAsync(long courseRoundId);
       public Task AddRangeAsync(List<CourseRoundInstructor> entities);
       public Task<List<InstructorCourseRoundDto>> GetByInstructorAsync(long instructorId);
    }
}
