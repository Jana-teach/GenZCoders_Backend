namespace GenZCoders.DTOs.CourseRoundInstructor
{
    public class InstructorCourseRoundDto
    {
        public long CourseRoundId { get; set; }
        public string CourseName { get; set; } = null!;
        public decimal? RoundNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Price { get; set; }
    }
}
