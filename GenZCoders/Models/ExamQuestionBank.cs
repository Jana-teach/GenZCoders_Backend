using System;
using System.Collections.Generic;

namespace GenZCoders.Models;

public partial class ExamQuestionBank
{
    public int Id { get; set; }

    public long? ExamId { get; set; }

    public long? QuestionId { get; set; }

    public long? CourseRoundId { get; set; }

    // Navigation properties
    public virtual CourseRound? CourseRound { get; set; }
}