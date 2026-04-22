namespace GenZCoders.DTOs.WeekDto
{
    public class WeekDto
    {
        public long Id { get; set; }
        public string? WeekTitle { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        // Only include child materials, no navigation back to Week
        public List<CourseMaterialDto> CourseMaterials { get; set; } = new();
    }
}
