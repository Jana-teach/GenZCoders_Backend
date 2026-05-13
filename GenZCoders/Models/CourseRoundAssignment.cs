using System;
using System.Collections.Generic;

namespace GenZCoders.Models;

public partial class CourseRoundAssignment
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? AssignmentLink { get; set; }

    public DateTime Deadline { get; set; }

    public decimal TotalGrade { get; set; }

    public long CourseRoundId { get; set; }

    public long InstructorId { get; set; }

    public long? CourseMaterialId { get; set; }

    public long? StatusId { get; set; }

    public DateTime CreatedAt { get; set; }
    public virtual Account? Instructor { get; set; }
    public virtual CourseMaterial? CourseMaterial { get; set; }
    public virtual Status? Status { get; set; }
    public virtual CourseRound? CourseRound { get; set; }
    public virtual ICollection<CourseRoundAssignmentSubmission> CourseRoundAssignmentSubmissions { get; set; } = new List<CourseRoundAssignmentSubmission>();
}
