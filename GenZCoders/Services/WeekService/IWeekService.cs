using GenZCoders.DTOs.WeekDto;
using GenZCoders.Models;

namespace GenZCoders.Services.WeekService
{
    public interface IWeekService
    {
       public Task<List<WeekDto>> GetAllAsync();
       public Task<WeekDto?> GetByIdAsync(int id);
       public Task<WeekDto> CreateAsync(CreateWeekDto dto);
       public Task<bool> UpdateAsync(int id, UpdateWeekDto dto);
       public Task<bool> DeleteAsync(int id);
    }
}
