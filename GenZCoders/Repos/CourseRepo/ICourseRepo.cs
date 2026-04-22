using GenZCoders.Models;

namespace GenZCoders.Repos.CourseRepo
{
    public interface ICourseRepo
    {
      public  Task<Course?> GetByIdAsync(long id);
       public Task<List<Course>> GetAllAsync();
       public Task AddAsync(Course course);
       public void Update(Course course);
       public void Remove(Course course);
       public Task<bool> SaveChangesAsync();
    }

}
