namespace GenZCoders.DTOs.WeekDto
{
    public class CreateWeekDto
    {
        public string? WeekTitle { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        // For the auto-created material
        public long AccountId { get; set; }
        public long CourseRoundId { get; set; }
    }

}
