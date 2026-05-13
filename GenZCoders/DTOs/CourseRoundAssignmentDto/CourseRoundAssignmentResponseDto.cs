namespace GenZCoders.DTOs.CourseRoundAssignmentDto;

public class CourseRoundAssignmentResponseDto
{
    public long Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? AssignmentLink { get; set; }
    public DateTime Deadline { get; set; }
    public decimal TotalGrade { get; set; }
    public long CourseRoundId { get; set; }
    public long InstructorId { get; set; }
    public string? InstructorName { get; set; }
    public long? CourseMaterialId { get; set; }
    public long? StatusId { get; set; }
    public string? StatusName { get; set; }
    public DateTime CreatedAt { get; set; }
}
