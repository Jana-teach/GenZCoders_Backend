using GenZCoders.DTOs.DashBoardsDto;
using GenZCoders.Repos.EngineerDashboardRepo;

namespace GenZCoders.Services.EngineerDashboardService
{
    public class EngineerDashboardService : IEngineerDashboardService
    {
        private readonly IEngineerDashboardRepo _repo;

        // Replace with your actual status IDs
        private const long AcceptedStatusId = 17;
        private const long RejectedStatusId = 18;
        private const long PendingStatusId = 16;

        public EngineerDashboardService(IEngineerDashboardRepo repo)
        {
            _repo = repo;
        }

        public async Task<EngineerDashboardDto> GetDashboardAsync()
        {
            var totalCourses = await _repo.GetTotalCoursesAsync();
            var totalRounds = await _repo.GetTotalCourseRoundsAsync();
            var acceptedApps = await _repo.GetApplicationsCountByStatusAsync(AcceptedStatusId);
            var rejectedApps = await _repo.GetApplicationsCountByStatusAsync(RejectedStatusId);
            var pendingApps = await _repo.GetApplicationsCountByStatusAsync(PendingStatusId);

            var topCoursesRaw = await _repo.GetTopCoursesAsync();
            var topCourses = topCoursesRaw.Select(tc => new TopCourseDto
            {
                CourseId = tc.CourseId,
                CourseTitle = tc.Title,
                ApplicationCount = tc.ApplicationCount
            }).ToList();

            return new EngineerDashboardDto
            {
                TotalCourses = totalCourses,
                TotalCourseRounds = totalRounds,
                TotalAcceptedApplications = acceptedApps,
                TotalRejectedApplications = rejectedApps,
                TotalPendingApplications = pendingApps,
                TopCourses = topCourses
            };
        }
    }
}
