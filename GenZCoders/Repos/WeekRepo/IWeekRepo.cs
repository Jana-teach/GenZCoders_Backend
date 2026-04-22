using GenZCoders.Models;

namespace GenZCoders.Repos.WeekRepo
{
    public interface IWeekRepo
    {
       public Task<List<Weeks>> GetAllAsync();
       public Task<Weeks?> GetByIdAsync(int id);
       public Task AddAsync(Weeks week);
       public Task UpdateAsync(Weeks week);
       public Task DeleteAsync(Weeks week);
    }
}
