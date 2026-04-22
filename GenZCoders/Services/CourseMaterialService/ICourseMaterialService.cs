using GenZCoders.DTOs.CourseMaterialDto;
using GenZCoders.Models.GenZCoders.Models;

namespace GenZCoders.Services.CourseMaterialService
{
    public interface ICourseMaterialService
    {
       public Task<List<ReadCourseMaterialDto>> GetAllAsync();
       public Task<ReadCourseMaterialDto?> GetByIdAsync(long id);
       public Task<ReadCourseMaterialDto> CreateAsync(CreateCourseMaterialDto dto);
       public Task<ReadCourseMaterialDto> CreateZoomMaterialAsync(CreateZoomCourseMaterialDto dto);
       public Task<bool> UpdateAsync(long id, UpdateCourseMaterialDto dto);
       public Task<bool> PatchAsync(long id, PatchCourseMaterialDto dto);
       public Task<bool> DeleteAsync(long id);
    }
}
