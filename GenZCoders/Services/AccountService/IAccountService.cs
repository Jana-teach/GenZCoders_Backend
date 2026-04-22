using GenZCoders.DTOs.AccountsDto;

namespace GenZCoders.Services.AccountService
{
    public interface IAccountService
    {
        public Task<List<InstructorDto>> GetCoInstructorsAsync();
    }
}
