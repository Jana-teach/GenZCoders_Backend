using GenZCoders.DTOs.CourseRoundInstructor;
using GenZCoders.Models;
using GenZCoders.Repos.CourseRoundInstructorRepo;

namespace GenZCoders.Services.CourseRoundInstructorService
{
    public class CourseRoundInstructorService : ICourseRoundInstructorService
    {
        private readonly ICourseRoundInstructorRepository _repository;

        public CourseRoundInstructorService(ICourseRoundInstructorRepository repository)
        {
            _repository = repository;
        }

        public async Task AssignInstructorsAsync(AssignInstructorsDto dto)
        {
            await _repository.RemoveByCourseRoundAsync(dto.CourseRoundId);

            var entities = dto.InstructorIds.Select(id => new CourseRoundInstructor
            {
                CourseRoundId = dto.CourseRoundId,
                InstructorId = id,
                AssignedDate = DateTime.UtcNow
            }).ToList();

            await _repository.AddRangeAsync(entities);
        }

        public async Task<List<InstructorCourseRoundDto>> GetInstructorCourseRoundsAsync(long instructorId)
        {
            return await _repository.GetByInstructorAsync(instructorId);
        }
    }

}
