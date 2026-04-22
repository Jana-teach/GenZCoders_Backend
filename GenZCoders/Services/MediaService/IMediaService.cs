using GenZCoders.DTOs.MediaDto;

namespace GenZCoders.Services.MediaService
{
    public interface IMediaService
    {
       public Task AddMediaAsync(MediaCreateDto dto);
       public Task<List<MediaDto>> GetMediasAsync(string tableName, long tableId);
       public Task DeleteMediaAsync(long mediaId);
    }
}
