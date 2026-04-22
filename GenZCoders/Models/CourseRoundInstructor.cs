using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenZCoders.Models;

public partial class CourseRoundInstructor
{
    public long Id { get; set; }

    public long CourseRoundId { get; set; }
    [Column("InstructorAccountId")]
    public long InstructorId { get; set; }

    public DateTime AssignedDate { get; set; }

    public virtual CourseRound CourseRound { get; set; } = null!;
    [ForeignKey(nameof(InstructorId))]
    public virtual Account Instructor { get; set; } = null!;
}
