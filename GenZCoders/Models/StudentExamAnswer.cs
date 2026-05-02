using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenZCoders.Models;

[Table("StudentExamAnswer")]

public partial class StudentExamAnswer
{
    public long Id { get; set; }

    public long AccountId { get; set; }

    public long? ExamQuestionId { get; set; }  // <-- Matches DB column name

    public string ChoosedAnswer { get; set; } = null!;

    public bool Score { get; set; }

    public long? QuestionbankId { get; set; }  // <-- New column

    public long? ExamDetailsId { get; set; }     // <-- New column (ExamDetailsID)

    public virtual Account Account { get; set; } = null!;

    public virtual ExamQuestion? ExamQuestion { get; set; }  // Nav to ExamQuestion
}