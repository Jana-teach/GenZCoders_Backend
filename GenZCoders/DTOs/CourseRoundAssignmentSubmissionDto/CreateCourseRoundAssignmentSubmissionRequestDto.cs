using System.ComponentModel.DataAnnotations;

namespace GenZCoders.DTOs.CourseRoundAssignmentSubmissionDto;

public class CreateCourseRoundAssignmentSubmissionRequestDto
{
    [Required]
    public long AssignmentId { get; set; }

    [Required]
    public long StudentId { get; set; }

    [Required]
    [MaxLength(2000)]
    public string SubmissionLink { get; set; } = null!;
}
