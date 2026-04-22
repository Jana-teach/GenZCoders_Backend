namespace GenZCoders.DTOs.CourseMaterialDto
{
    public class CreateCourseMaterialDto
    {
        public long CourseRoundId { get; set; }
        public long CreatedByAccountId { get; set; }
        public long WeekId { get; set; }
        public long? ParentMaterialId { get; set; }
        public long MaterialTypeStatusId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? MeetingId { get; set; }
        public string? MeetingPassword { get; set; }
    }
}
