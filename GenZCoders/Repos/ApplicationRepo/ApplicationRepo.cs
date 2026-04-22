using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GenZCoders.Repos.ApplicationRepo
{
    public class ApplicationRepo : IApplicationRepo
    {
        private readonly SchoolDbContext _context;

        public ApplicationRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetByIdAsync(long id)
        {
            return await _context.Applications
                .Include(a => a.Account)
                .Include(s => s.Status)
                .Include(a => a.CourseRound)
                    .ThenInclude(cr => cr.Course)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Application>> GetAllAsync()
        {
            return await _context.Applications
                .Include(a => a.Account)
                .Include(s => s.Status)
                .Include(a => a.CourseRound)
                .ThenInclude(cr => cr.Course)
                .ToListAsync();
        }

        public async Task AddAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
        }

        public void Remove(Application application)
        {
            _context.Applications.Remove(application);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
