using GenZCoders.DTOs.CourseRoundInstructor;

namespace GenZCoders.Services.CourseRoundInstructorService
{
    public interface ICourseRoundInstructorService
    {
       public Task AssignInstructorsAsync(AssignInstructorsDto dto);
       public Task<List<InstructorCourseRoundDto>> GetInstructorCourseRoundsAsync(long instructorId);
    }

}
