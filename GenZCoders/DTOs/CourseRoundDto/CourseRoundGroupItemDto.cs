namespace GenZCoders.DTOs.CourseRoundDto
{
    public class CourseRoundGroupItemDto
    {
        public long Id { get; set; }
        public decimal? RoundNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Price { get; set; }
    }
}
