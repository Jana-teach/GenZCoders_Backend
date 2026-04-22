using GenZCoders.DTOs.DashBoardsDto;

namespace GenZCoders.Services.EngineerDashboardService
{
    public interface IEngineerDashboardService
    {
        Task<EngineerDashboardDto> GetDashboardAsync();
    }
}
