namespace GenZCoders.DTOs.ExamsDto
{
    public class StudentExamAnswerDto
    {
        public long? QuestionId { get; set; }
        public string? QuestionTitle { get; set; }
        public string ChoosedAnswer { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
