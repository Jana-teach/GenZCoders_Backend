using System;
using System.Collections.Generic;

namespace GenZCoders.Models;

public partial class CourseRound
{
    public long Id { get; set; }

    public long CourseId { get; set; }

    public decimal? RoundNumber { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public long? MaxStudents { get; set; }

    public long StatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Question1 { get; set; }

    public string? Question2 { get; set; }

    public string? Question3 { get; set; }

    public string? Question4 { get; set; }

    public string? Question5 { get; set; }

    public string? Question6 { get; set; }

    public string? Question7 { get; set; }

    public string? Question8 { get; set; }

    public string? Question9 { get; set; }

    public string? Question10 { get; set; }

    public long? MinStudents { get; set; }

    public decimal? Price { get; set; }

    public int? AutomatedWorkFlowJump { get; set; }

    public long? CourseRoundGroupId { get; set; }

    public long? TrialStatusID { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual Status? TrialStatus { get; set; }

    public virtual ICollection<CourseMaterial> CourseMaterials { get; set; } = new List<CourseMaterial>();

    public virtual ICollection<CourseRoundInstructor> CourseRoundInstructors { get; set; } = new List<CourseRoundInstructor>();

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual CourseRound? CourseRoundGroup { get; set; }

    public virtual ICollection<CourseRound> GroupedRounds { get; set; } = new List<CourseRound>();
    public virtual ICollection<ExamQuestionBank> ExamQuestionBanks { get; set; } = new List<ExamQuestionBank>();
}