using System.ComponentModel.DataAnnotations;

namespace GenZCoders.DTOs.CourseRoundAssignmentSubmissionDto;

/// <summary>
/// Partial update: only non-null fields are applied. Omitted or null JSON values leave the stored value unchanged.
/// </summary>
public class PatchCourseRoundAssignmentSubmissionRequestDto
{
    [Range(typeof(decimal), "0", "1000000")]
    public decimal? Grade { get; set; }

    public long? StatusId { get; set; }

    [MaxLength(8000)]
    public string? Feedback { get; set; }
}
