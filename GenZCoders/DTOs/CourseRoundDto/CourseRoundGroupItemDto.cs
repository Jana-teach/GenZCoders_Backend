namespace GenZCoders.DTOs.CourseRoundDto
{
    public class CourseRoundGroupItemDto
    {
        public long Id { get; set; }
        public decimal RoundNumber { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? Price { get; set; }
    }
}
