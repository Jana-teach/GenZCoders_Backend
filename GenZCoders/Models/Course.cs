using System;
using System.Collections.Generic;

namespace GenZCoders.Models;

public partial class Course
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long LevelStatusId { get; set; }

    public long DurationHours { get; set; }
    public virtual Status LevelStatus { get; set; }

    public virtual ICollection<CourseRound> CourseRounds { get; set; } = new List<CourseRound>();
}
