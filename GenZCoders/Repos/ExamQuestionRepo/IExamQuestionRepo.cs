using GenZCoders.Models;

namespace GenZCoders.Repos.ExamRepo
{
    public interface IExamQuestionRepo
    {
        Task<IEnumerable<ExamQuestion>> GetByIdsAsync(IEnumerable<long> questionIds);
        Task<ExamQuestion?> GetByIdAsync(long id);

        // NEW: Add these methods
        Task AddAsync(ExamQuestion entity);
        Task AddRangeAsync(IEnumerable<ExamQuestion> entities);
        Task SaveChangesAsync();
    }
}