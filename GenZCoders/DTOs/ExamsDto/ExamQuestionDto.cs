namespace GenZCoders.DTOs.ExamsDto
{
    public class ExamQuestionDto
    {
        public long Id { get; set; }
        public string QuestionTitle { get; set; } = null!;
        public string Choice1 { get; set; } = null!;
        public string Choice2 { get; set; } = null!;
        public string Choice3 { get; set; } = null!;
        public string Choice4 { get; set; } = null!;

    }
}
