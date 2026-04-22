using GenZCoders.DTOs.CourseDto;

namespace GenZCoders.Services.CourseService
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(long id);
        Task<CourseDto> CreateAsync(CreateCourseDto dto);
        Task<bool> UpdateAsync(long id, UpdateCourseDto dto);
        Task<bool> PatchAsync(long id, PatchCourseDto dto);
        Task<bool> DeleteAsync(long id);
    }

}
