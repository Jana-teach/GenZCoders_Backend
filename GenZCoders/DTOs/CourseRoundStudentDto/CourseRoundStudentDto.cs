namespace GenZCoders.DTOs.CourseRoundStudentDto
{
    public class CourseRoundStudentDto
    {
        public long Id { get; set; }
        public long CourseRoundId { get; set; }
        public long StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string AssignedAt { get; set; } = string.Empty;
        public string? CompletedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
