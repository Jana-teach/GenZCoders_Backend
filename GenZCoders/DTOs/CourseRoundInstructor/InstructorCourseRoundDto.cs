namespace GenZCoders.DTOs.CourseRoundInstructor
{
    public class InstructorCourseRoundDto
    {
        public long CourseRoundId { get; set; }
        public string CourseName { get; set; } = null!;
        public decimal? RoundNumber { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? Price { get; set; }
    }
}
