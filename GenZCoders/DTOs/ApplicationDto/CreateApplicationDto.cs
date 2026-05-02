using GenZCoders.DTOs.ExamsDto;

namespace GenZCoders.DTOs.ApplicationDto
{
    public class CreateApplicationDto
    {
        public long CourseRoundId { get; set; }
        public long AccountId { get; set; }

        // Optional legacy
        public string? Answer1 { get; set; }
        public string? Answer2 { get; set; }
        public string? Answer3 { get; set; }
        public string? Answer4 { get; set; }
        public string? Answer5 { get; set; }
        public string? Answer6 { get; set; }
        public string? Answer7 { get; set; }
        public string? Answer8 { get; set; }
        public string? Answer9 { get; set; }
        public string? Answer10 { get; set; }

        // NEW
        public List<ExamAnswerItemDto>? ExamAnswers { get; set; }
    }

}
