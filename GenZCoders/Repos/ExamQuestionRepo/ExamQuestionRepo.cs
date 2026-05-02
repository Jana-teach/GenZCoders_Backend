using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.ExamRepo
{
    public class ExamQuestionRepo : IExamQuestionRepo
    {
        private readonly SchoolDbContext _context;
        private readonly DbSet<ExamQuestion> _dbSet;

        public ExamQuestionRepo(SchoolDbContext context)
        {
            _context = context;
            _dbSet = context.ExamQuestions;
        }

        public async Task<IEnumerable<ExamQuestion>> GetByIdsAsync(IEnumerable<long> questionIds)
        {
            return await _dbSet
                .Where(x => questionIds.Contains(x.Id))
                .ToListAsync();
        }

        public async Task<ExamQuestion?> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        // NEW: Add methods
        public async Task AddAsync(ExamQuestion entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<ExamQuestion> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}