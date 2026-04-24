namespace GenZCoders.DTOs.CourseDto
{
    public class CourseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LevelStatus { get; set; }
        public long? DurationHours { get; set; }
        public MediaForCourseDto? Media { get; set; }
    }

}
