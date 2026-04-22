using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GenZCoders.Repos.MediaRepo
{
    public class MediaRepository : IMediaRepository
    {
        private readonly SchoolDbContext _context;

        public MediaRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<Media>> GetByOwnerAsync(string tableName, long tableId)
        {
            var tn = tableName?.ToLower() ?? string.Empty;
            return await _context.Media
                .Where(m => (m.TableName != null && m.TableName.ToLower() == tn) && m.TableId == tableId)
                .ToListAsync();
        }
        public async Task<List<Media>> GetByOwnerAsync(string tableName, List<long> tableIds)
        {
            var tn = tableName?.ToLower() ?? string.Empty;
            return await _context.Media
                .Where(m => (m.TableName != null && m.TableName.ToLower() == tn) && tableIds.Contains(m.TableId))
                .ToListAsync();
        }

        public async Task<Media?> GetByIdAsync(long id)
        {
            return await _context.Media.FindAsync(id);
        }

        public async Task AddAsync(Media media)
        {
            await _context.Media.AddAsync(media);
        }

        public void Delete(Media media)
        {
            _context.Media.Remove(media);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
