using GenZCoders.Models;

namespace GenZCoders.Repos.MediaRepo
{
    public interface IMediaRepository
    {
       public Task<List<Media>> GetByOwnerAsync(string tableName, long tableId);
        Task<List<Media>> GetByOwnerAsync(string tableName, List<long> tableIds);

        public Task<Media?> GetByIdAsync(long id);
       public Task AddAsync(Media media);
       public void Delete(Media media);
       public Task SaveAsync();
    }

}
