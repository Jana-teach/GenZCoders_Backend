using GenZCoders.DTOs.AuthDto;
using GenZCoders.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GenZCoders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequestDto dto)
        {
            var result = await _authService.SignupAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var accountIdClaim = User.FindFirstValue("AccountId");
            if (!long.TryParse(accountIdClaim, out var accountId))
                return Unauthorized(new { message = "Invalid authentication token." });

            var user = await _authService.GetCurrentUserAsync(accountId);
            return Ok(user);
        }
    }
}
