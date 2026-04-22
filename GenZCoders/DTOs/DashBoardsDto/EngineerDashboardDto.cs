namespace GenZCoders.DTOs.DashBoardsDto
{
    public class EngineerDashboardDto
    {
        public long TotalCourses { get; set; }
        public long TotalCourseRounds { get; set; }
        public long TotalAcceptedApplications { get; set; }
        public long TotalRejectedApplications { get; set; }
        public long TotalPendingApplications { get; set; }
        public List<TopCourseDto> TopCourses { get; set; } = new();
    }
    public class TopCourseDto
    {
        public long CourseId { get; set; }
        public string CourseTitle { get; set; } = null!;
        public long ApplicationCount { get; set; }
    }
}
