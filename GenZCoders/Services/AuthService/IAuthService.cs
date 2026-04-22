using GenZCoders.DTOs.AuthDto;

namespace GenZCoders.Services.AuthService
{
    public interface IAuthService
    {
       public Task<AuthResponseDto> SignupAsync(SignupRequestDto dto);
       public Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
       public Task<AuthenticatedUserDto> GetCurrentUserAsync(long accountId);
    }
}
