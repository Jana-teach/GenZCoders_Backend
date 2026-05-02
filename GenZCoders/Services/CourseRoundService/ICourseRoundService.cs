using GenZCoders.DTOs.CourseRoundDto;
using GenZCoders.DTOs.ExamsDto;

namespace GenZCoders.Services.CourseRoundService
{
    public interface ICourseRoundService
    {
        Task<List<CourseRoundDto>> GetAllAsync();
        Task<CourseRoundDetailsDto?> GetByIdAsync(long id);
        Task<CourseRoundDetailsDto> CreateAsync(CreateCourseRoundDto dto);
        Task<bool> UpdateAsync(long id, UpdateCourseRoundDto dto);
        Task<bool> PatchAsync(long id, PatchCourseRoundDto dto);
        Task<bool> DeleteAsync(long id);

        // NEW: Separate endpoint to manage exam questions
        Task<bool> AddExamQuestionsAsync(long courseRoundId, List<CreateExamQuestionDto> questions);
        Task<bool> RemoveExamQuestionsAsync(long courseRoundId, List<long> questionIds);
    }
}