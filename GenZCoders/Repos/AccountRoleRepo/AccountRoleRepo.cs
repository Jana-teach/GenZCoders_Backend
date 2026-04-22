using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.AccountRoleRepo
{
    public class AccountRoleRepo : IAccountRoleRepo
    {
        private readonly SchoolDbContext _context;

        public AccountRoleRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AccountRole accountRole)
        {
            _context.AccountRoles.Add(accountRole);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Account>> GetCoInstructorsAsync(string businessEntityName)
        {
            return await _context.AccountRoles
                .Where(ar => ar.BusinessEntityName == businessEntityName &&
                             ar.Role != null &&
                             ar.Role.RoleName == "co-Instructor" &&
                             ar.Account != null)
                .Select(ar => ar.Account!)  
                .ToListAsync();
        }
    }
}
