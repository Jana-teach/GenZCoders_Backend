using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.ExamRepo
{
    public class StudentExamAnswerRepo : IStudentExamAnswerRepo
    {
        private readonly SchoolDbContext _context;
        private readonly DbSet<StudentExamAnswer> _dbSet;

        public StudentExamAnswerRepo(SchoolDbContext context)
        {
            _context = context;
            _dbSet = context.StudentExamAnswers;
        }

        public async Task<IEnumerable<StudentExamAnswer>> GetByAccountIdAsync(long accountId)
        {
            return await _dbSet
                .Where(x => x.AccountId == accountId)
                .Include(x => x.ExamQuestion)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentExamAnswer>> GetByAccountAndQuestionBankAsync(long accountId, long questionBankId)
        {
            return await _dbSet
                .Where(x => x.AccountId == accountId && x.QuestionbankId == questionBankId)
                .Include(x => x.ExamQuestion)
                .ToListAsync();
        }

        public async Task AddAsync(StudentExamAnswer entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<StudentExamAnswer> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}