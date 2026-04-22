namespace GenZCoders.DTOs.CourseRoundInstructor
{
    public class AssignInstructorsDto
    {
        public long CourseRoundId { get; set; }
        public List<long> InstructorIds { get; set; } = new();
    }
}
