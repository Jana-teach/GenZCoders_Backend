using GenZCoders.DTOs.AccountsDto;
using GenZCoders.Repos.AccountRoleRepo;

namespace GenZCoders.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRoleRepo _repo;

        public AccountService(IAccountRoleRepo repo)
        {
            _repo = repo;
        }

        public async Task<List<InstructorDto>> GetCoInstructorsAsync()
        {
            var accounts = await _repo.GetCoInstructorsAsync("GenZCoders");

            return accounts.Select(a => new InstructorDto
            {
                Id = a.Id,
                FullName = a.FullNameEn,
                Email = a.Email
            }).ToList();
        }
    }
}
