using GenZCoders.Models;
using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.CourseMaterialRepo
{
    public class CourseMaterialRepo : ICourseMaterialRepo
    {
        private readonly SchoolDbContext _context;

        public CourseMaterialRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseMaterial>> GetAllAsync()
        {
            return await _context.CourseMaterials
                .Include(c => c.Week)
                .Include(c => c.CreatedBy)
                .Include(c => c.CourseRound)
                .Include(c => c.Status)
                .Include(c => c.MaterialTypeStatus)
                .ToListAsync();
        }

        public async Task<CourseMaterial?> GetByIdAsync(long id)
        {
            return await _context.CourseMaterials
                .Include(c => c.Week)
                .Include(c => c.CreatedBy)
                .Include(c => c.CourseRound)
                .Include(c => c.Status)
                .Include(c => c.MaterialTypeStatus)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(CourseMaterial material)
        {
            _context.CourseMaterials.Add(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseMaterial material)
        {
            _context.CourseMaterials.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CourseMaterial material)
        {
            _context.CourseMaterials.Remove(material);
            await _context.SaveChangesAsync();
        }
    }
}
