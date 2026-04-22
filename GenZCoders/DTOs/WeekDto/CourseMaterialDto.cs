namespace GenZCoders.DTOs.WeekDto
{
    public class CourseMaterialDto
    {
        public long Id { get; set; }
        public long CourseRoundId { get; set; }
        public long CreatedByAccountId { get; set; }

        public long? ParentMaterialId { get; set; }
        public long StatusId { get; set; }
        public long MaterialTypeStatusId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? MeetingId { get; set; }
        public string? MeetingPassword { get; set; }

        // Only include child materials, no parent navigation
        public List<CourseMaterialDto> ChildMaterials { get; set; } = new();
    }
}
