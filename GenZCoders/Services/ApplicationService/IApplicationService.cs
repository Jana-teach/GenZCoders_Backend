using GenZCoders.DTOs.ApplicationDto;
using GenZCoders.DTOs.ExamsDto;

namespace GenZCoders.Services.ApplicationService
{
    public interface IApplicationService
    {
        Task<ApplicationDto> CreateAsync(CreateApplicationDto dto);
        Task<ApplicationDto?> GetByIdAsync(long id);
        Task<List<ApplicationDto>> GetAllAsync();
        Task<bool> DeleteAsync(long id);
        Task<bool> PatchStatusAsync(long id, PatchApplicationStatusDto dto);
        Task<bool> PatchCourseRoundAsync(long id, PatchApplicationCourseRoundDto dto);

        // NEW: Get exam questions for a course round
        Task<List<ExamQuestionDto>> GetExamQuestionsAsync(long courseRoundId);

        // NEW: Submit exam answers separately
        Task<bool> SubmitExamAnswersAsync(long accountId, long courseRoundId, List<ExamAnswerItemDto> answers);
    }
}