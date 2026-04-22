using GenZCoders.Models;

namespace GenZCoders.Repos.AuthRepo
{
    public interface IAccountRepo
    {
       public Task AddAsync(Account account);
       public Task<Account?> GetByIdAsync(long id);
       public Task<Account?> GetByEmailAsync(string email);
       public Task<Account?> GetByNationalIdAsync(string nationalId);
    }
}
