using GenZCoders.Models;

namespace GenZCoders.Repos.LoginRepo
{
    public interface ILoginRepo
    {
       public Task<Login?> GetByEmailAsync(string email);
       public Task<bool> ExistsByEmailAsync(string email);
       public Task AddAsync(Login login);
    }
}
