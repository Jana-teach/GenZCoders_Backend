namespace GenZCoders.DTOs.CourseRoundDto
{
    public class CourseRoundDto
    {
        public long Id { get; set; }
        public long? CourseId { get; set; }
        public decimal? RoundNumber { get; set; }
        public long InstructorId { get; set; }
        public string? InstructorName { get; set; }


        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? CourseRoundGroupId { get; set; }

        public long? MinStudents { get; set; }
        public long? MaxStudents { get; set; }

        public decimal? Price { get; set; }

        public string Status { get; set; }
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

        public List<CourseRoundGroupItemDto> Groups { get; set; } = new();

    }

}
