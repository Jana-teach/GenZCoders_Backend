using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.AuthRepo
{
    public class AccountRepo : IAccountRepo
    {
        private readonly SchoolDbContext _context;

        public AccountRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account?> GetByIdAsync(long id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLower();
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower() == normalizedEmail);
        }

        public async Task<Account?> GetByNationalIdAsync(string nationalId)
        {
            var normalizedNationalId = nationalId.Trim();
            return await _context.Accounts.FirstOrDefaultAsync(a => a.NationalId == normalizedNationalId);
        }
    }
}
