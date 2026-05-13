namespace GenZCoders.DTOs.CourseRoundAssignmentSubmissionDto;

public class CourseRoundAssignmentSubmissionResponseDto
{
    public long Id { get; set; }
    public long AssignmentId { get; set; }
    public string? AssignmentTitle { get; set; }
    public long StudentId { get; set; }
    public string StudentName { get; set; } = null!;
    public string SubmissionLink { get; set; } = null!;
    public DateTime SubmittedAt { get; set; }
    public decimal? Grade { get; set; }
    public string? Feedback { get; set; }
    public long? StatusId { get; set; }
    public string Status {  get; set; }
    public string? StatusName { get; set; }
}
