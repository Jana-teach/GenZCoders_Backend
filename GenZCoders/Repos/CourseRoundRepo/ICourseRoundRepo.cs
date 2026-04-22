using GenZCoders.Models;

namespace GenZCoders.Repos.CourseRoundRepo
{
    public interface ICourseRoundRepo
    {
        Task<CourseRound?> GetByIdAsync(long id);
        Task<List<CourseRound>> GetAllAsync();
        Task AddAsync(CourseRound round);
        void Update(CourseRound round);
        void Remove(CourseRound round);
        Task<bool> SaveChangesAsync();
    }

}
