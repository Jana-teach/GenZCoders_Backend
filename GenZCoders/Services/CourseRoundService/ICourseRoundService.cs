using GenZCoders.DTOs.CourseRoundDto;

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
    }

}
