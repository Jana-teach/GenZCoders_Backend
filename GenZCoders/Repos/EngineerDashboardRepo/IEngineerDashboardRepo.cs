namespace GenZCoders.Repos.EngineerDashboardRepo
{
    public interface IEngineerDashboardRepo
    {
       public Task<long> GetTotalCoursesAsync();
       public Task<long> GetTotalCourseRoundsAsync();
       public Task<long> GetApplicationsCountByStatusAsync(long statusId);
       public Task<List<(long CourseId, string Title, long ApplicationCount)>> GetTopCoursesAsync(int top = 3);
    }
}
