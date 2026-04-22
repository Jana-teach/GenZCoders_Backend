using GenZCoders.Models;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Repos.LoginRepo
{
    public class LoginRepo : ILoginRepo
    {
        private readonly SchoolDbContext _context;

        public LoginRepo(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<Login?> GetByEmailAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLower();
            return await _context.Logins
                .Include(l => l.Account)
                    .ThenInclude(a => a.AccountRoles)
                        .ThenInclude(ar => ar.Role)
                .FirstOrDefaultAsync(l => l.Email.ToLower() == normalizedEmail);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var normalizedEmail = email.Trim().ToLower();
            return await _context.Logins.AnyAsync(l => l.Email.ToLower() == normalizedEmail);
        }

        public async Task AddAsync(Login login)
        {
            _context.Logins.Add(login);
            await _context.SaveChangesAsync();
        }
    }
}
