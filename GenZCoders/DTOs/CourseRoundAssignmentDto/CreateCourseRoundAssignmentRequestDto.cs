using System.ComponentModel.DataAnnotations;

namespace GenZCoders.DTOs.CourseRoundAssignmentDto;

public class CreateCourseRoundAssignmentRequestDto
{
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = null!;

    [MaxLength(4000)]
    public string? Description { get; set; }

    [MaxLength(2000)]
    public string? AssignmentLink { get; set; }

    [Required]
    public DateTime Deadline { get; set; }

    [Range(typeof(decimal), "0", "1000000")]
    public decimal TotalGrade { get; set; }

    [Required]
    public long CourseRoundId { get; set; }

    [Required]
    public long InstructorId { get; set; }

    public long? CourseMaterialId { get; set; }

    public long? StatusId { get; set; }
}
