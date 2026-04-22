using System;

namespace GenZCoders.DTOs.CourseMaterialDto
{
    public class CreateZoomCourseMaterialDto
    {
        public long CourseRoundId { get; set; }
        public long CreatedByAccountId { get; set; }
        public long WeekId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTimeUtc { get; set; }
        public int DurationMinutes { get; set; }
    }
}

