namespace GenZCoders.DTOs.CourseDto
{
    public class CreateCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long LevelStatusId { get; set; }
        public long DurationHours { get; set; }
    }

}
