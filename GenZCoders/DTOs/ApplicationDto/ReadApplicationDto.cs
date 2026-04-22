namespace GenZCoders.DTOs.ApplicationDto
{
    public class ApplicationDto
    {
        public long Id { get; set; }
        public long CourseRoundId { get; set; }
        public long AccountId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }

        public string? FullNameEn { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

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

        public MediaForApplicationDto? Media { get; set; }

    }

}
