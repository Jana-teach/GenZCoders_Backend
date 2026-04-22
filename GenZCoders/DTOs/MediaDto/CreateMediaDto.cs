namespace GenZCoders.DTOs.MediaDto
{
    public class MediaCreateDto
    {
        public string TableName { get; set; } = null!; // "Application" or "Course"
        public long TableId { get; set; }              // ApplicationId or CourseId
        public string FilePath { get; set; } = null!;
    }
}
