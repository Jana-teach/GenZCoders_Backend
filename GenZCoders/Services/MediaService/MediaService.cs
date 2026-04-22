using GenZCoders.DTOs.MediaDto;
using GenZCoders.Models;
using GenZCoders.Repos.MediaRepo;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;

namespace GenZCoders.Services.MediaService
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepo;
        private readonly SchoolDbContext _context;
        private readonly ILogger<MediaService> _logger;

        public MediaService(IMediaRepository mediaRepo, SchoolDbContext context, ILogger<MediaService> logger)
        {
            _mediaRepo = mediaRepo;
            _context = context;
            _logger = logger;
        }

        public async Task AddMediaAsync(MediaCreateDto dto)
        {
            var tableName = (dto.TableName ?? string.Empty).Trim();

            _logger.LogInformation("AddMediaAsync: TableName={TableName}, TableId={TableId}, FilePath={FilePath}", 
                tableName, dto.TableId, dto.FilePath);

            // Validate owner existence
            bool ownerExists;

            if (string.Equals(tableName, "Application", StringComparison.OrdinalIgnoreCase))
            {
                ownerExists = await _context.Applications.AnyAsync(a => a.Id == dto.TableId);
                _logger.LogInformation("Checking Application existence for Id={TableId}: {Exists}", dto.TableId, ownerExists);
                tableName = "Application";
            }
            else if (string.Equals(tableName, "Course", StringComparison.OrdinalIgnoreCase))
            {
                ownerExists = await _context.Courses.AnyAsync(c => c.Id == dto.TableId);
                tableName = "Course";
            }
            else
            {
                _logger.LogWarning("AddMediaAsync: Unknown table name: {TableName}", tableName);
                ownerExists = false;
            }

            if (!ownerExists)
            {
                _logger.LogError("AddMediaAsync failed: Owner not found. TableName={TableName}, TableId={TableId}", 
                    tableName, dto.TableId);
                throw new ArgumentException($"Invalid TableName or TableId. Table '{tableName}' with ID {dto.TableId} does not exist.");
            }

            var media = new Media
            {
                TableName = tableName,
                TableId = dto.TableId,
                FilePath = dto.FilePath
            };

            await _mediaRepo.AddAsync(media);
            await _mediaRepo.SaveAsync();
        }

        public async Task<List<MediaDto>> GetMediasAsync(string tableName, long tableId)
        {
            var medias = await _mediaRepo.GetByOwnerAsync(tableName, tableId);

            return medias.Select(m => new MediaDto
            {
                Id = m.Id,
                FilePath = m.FilePath
            }).ToList();
        }

        public async Task DeleteMediaAsync(long mediaId)
        {
            var media = await _mediaRepo.GetByIdAsync(mediaId)
                ?? throw new KeyNotFoundException("Media not found");

            _mediaRepo.Delete(media);
            await _mediaRepo.SaveAsync();
        }
    }

}
