using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GenZCoders.Repos.CourseRepo
{
    public class CourseRepo : ICourseRepo
    {
        private readonly SchoolDbContext _context;

        public CourseRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllAsync()
            => await _context.Courses.Include(s=> s.LevelStatus).ToListAsync();

        public async Task<Course?> GetByIdAsync(long id)
            => await _context.Courses.Include(s=> s.LevelStatus).FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Course course)
            => await _context.Courses.AddAsync(course);

        public void Update(Course course)
            => _context.Courses.Update(course);

        public void Remove(Course course)
            => _context.Courses.Remove(course);

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() > 0;
    }

}
