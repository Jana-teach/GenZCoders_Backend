using System;
using System.Collections.Generic;

namespace GenZCoders.Models;

public partial class CourseRoundAssignmentSubmission
{
    public long Id { get; set; }

    public long AssignmentId { get; set; }

    public long StudentId { get; set; }

    public string SubmissionLink { get; set; } = null!;

    public DateTime SubmittedAt { get; set; }

    public decimal? Grade { get; set; }

    public string? Feedback { get; set; }

    public long? StatusId { get; set; }
    public virtual Account Student { get; set; } = null!;
    public virtual Status? Status { get; set; }
    public virtual CourseRoundAssignment Assignment { get; set; } = null!;
}
