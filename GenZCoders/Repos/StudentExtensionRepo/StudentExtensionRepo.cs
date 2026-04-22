using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.StudentExtensionRepo
{
    public class StudentExtensionRepo : IStudentExtensionRepo
    {
        private readonly SchoolDbContext _context;

        public StudentExtensionRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(StudentExtension entity)
        {
            _context.StudentExtensions.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<StudentExtension?> GetByAccountIdAsync(long accountId)
        {
            return await _context.StudentExtensions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task<bool> ExistsAsync(long accountId)
        {
            return await _context.StudentExtensions
                .AnyAsync(x => x.AccountId == accountId);
        }
    }
}
