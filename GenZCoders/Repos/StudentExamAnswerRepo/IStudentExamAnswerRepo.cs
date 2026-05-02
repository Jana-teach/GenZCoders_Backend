using GenZCoders.Models;

namespace GenZCoders.Repos.ExamRepo
{
    public interface IStudentExamAnswerRepo
    {
        Task<IEnumerable<StudentExamAnswer>> GetByAccountIdAsync(long accountId);
        Task<IEnumerable<StudentExamAnswer>> GetByAccountAndQuestionBankAsync(long accountId, long questionBankId);
        Task AddAsync(StudentExamAnswer entity);
        Task AddRangeAsync(IEnumerable<StudentExamAnswer> entities);
        Task SaveChangesAsync();
    }
}