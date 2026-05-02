using GenZCoders.Models;

namespace GenZCoders.Repos.ExamRepo
{
    public interface IExamQuestionBankRepo
    {
        Task<IEnumerable<ExamQuestionBank>> GetByCourseRoundIdAsync(long courseRoundId);
        Task<ExamQuestionBank?> FindAsync(Func<ExamQuestionBank, bool> predicate);
        Task AddAsync(ExamQuestionBank entity);
        Task AddRangeAsync(IEnumerable<ExamQuestionBank> entities);
        Task RemoveAsync(ExamQuestionBank entity);        // NEW
        Task RemoveRangeAsync(IEnumerable<ExamQuestionBank> entities);  // NEW
        Task SaveChangesAsync();
    }
}