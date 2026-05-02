using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.ExamRepo
{
    public class ExamQuestionBankRepo : IExamQuestionBankRepo
    {
        private readonly SchoolDbContext _context;
        private readonly DbSet<ExamQuestionBank> _dbSet;

        public ExamQuestionBankRepo(SchoolDbContext context)
        {
            _context = context;
            _dbSet = context.ExamQuestionBanks;
        }

        public async Task<IEnumerable<ExamQuestionBank>> GetByCourseRoundIdAsync(long courseRoundId)
        {
            return await _dbSet
                .Where(x => x.CourseRoundId == courseRoundId)
                .ToListAsync();
        }

        public async Task<ExamQuestionBank?> FindAsync(Func<ExamQuestionBank, bool> predicate)
        {
            return await Task.FromResult(_dbSet.AsEnumerable().FirstOrDefault(predicate));
        }

        public async Task AddAsync(ExamQuestionBank entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<ExamQuestionBank> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        // NEW: Remove methods
        public async Task RemoveAsync(ExamQuestionBank entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task RemoveRangeAsync(IEnumerable<ExamQuestionBank> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}