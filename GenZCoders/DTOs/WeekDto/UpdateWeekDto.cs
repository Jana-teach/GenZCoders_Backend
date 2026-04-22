namespace GenZCoders.DTOs.WeekDto
{
    public class UpdateWeekDto
    {
        public string? WeekTitle { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        // Optional: update default material

    }

}
