using GenZCoders.DTOs.ApplicationDto;

namespace GenZCoders.Services.ApplicationService
{
    public interface IApplicationService
    {
        Task<ApplicationDto> CreateAsync(long accountId, CreateApplicationDto dto);
        Task<ApplicationDto?> GetByIdAsync(long id);
        Task<List<ApplicationDto>> GetAllAsync();
        Task<bool> DeleteAsync(long id);
        Task<bool> PatchStatusAsync(long id, PatchApplicationStatusDto dto);
        Task<bool> PatchCourseRoundAsync(long id, PatchApplicationCourseRoundDto dto);
    }
}
