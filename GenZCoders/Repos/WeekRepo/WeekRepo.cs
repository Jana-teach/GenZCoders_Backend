using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GenZCoders.Repos.WeekRepo
{
    public class WeekRepo : IWeekRepo
    {
        private readonly SchoolDbContext _context;

        public WeekRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<Weeks>> GetAllAsync()
        {
            return await _context.Weeks
                .Where(x => x.BusinessEntityName == "GenZCoders")
                .Include(m=> m.CourseMaterials)
                .ToListAsync();
        }

        public async Task<Weeks?> GetByIdAsync(int id)
        {
            return await _context.Weeks
                 .Where(x => x.BusinessEntityName == "GenZCoders")
                .Include(w => w.CourseMaterials)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task AddAsync(Weeks week)
        {
            _context.Weeks.Add(week);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Weeks week)
        {
            _context.Weeks.Update(week);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Weeks week)
        {
            _context.Weeks.Remove(week);
            await _context.SaveChangesAsync();
        }
    }
}
