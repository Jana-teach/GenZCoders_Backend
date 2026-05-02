using GenZCoders.DTOs.ExamsDto;

namespace GenZCoders.DTOs.ApplicationDto
{
    public class ApplicationDto
    {
        public long Id { get; set; }
        public long CourseRoundId { get; set; }
        public long AccountId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = null!;
        public string? FullNameEn { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        // Legacy
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
        public List<StudentExamAnswerDto>? ExamAnswers { get; set; }
        public MediaForApplicationDto? Media { get; set; }

    }

}
