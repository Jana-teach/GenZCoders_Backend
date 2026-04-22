using GenZCoders.Models;

namespace GenZCoders.Repos.AccountRoleRepo
{
    public interface IAccountRoleRepo
    {
       public Task AddAsync(AccountRole accountRole);
       public Task<List<Account>> GetCoInstructorsAsync(string businessEntityName);
    }
}
