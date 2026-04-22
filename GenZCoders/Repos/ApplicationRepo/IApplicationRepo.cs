using GenZCoders.Models;

namespace GenZCoders.Repos.ApplicationRepo
{
    public interface IApplicationRepo
    {
        Task<Application?> GetByIdAsync(long id);
        Task<List<Application>> GetAllAsync();
        Task AddAsync(Application application);
        void Remove(Application application);
        Task<bool> SaveChangesAsync();
    }

}
