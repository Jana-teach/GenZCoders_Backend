using GenZCoders.DTOs.AuthDto;
using GenZCoders.Models;
using GenZCoders.Repos.AccountRoleRepo;
using GenZCoders.Repos.AuthRepo;
using GenZCoders.Repos.LoginRepo;
using GenZCoders.Repos.StudentExtensionRepo;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepo _accountRepo;
        private readonly ILoginRepo _loginRepo;
        private readonly IAccountRoleRepo _accountRoleRepo;
        private readonly IJwtTokenGenerator _jwt;
        private readonly IStudentExtensionRepo _studentExtensionRepo;
        private readonly SchoolDbContext _db;

        public AuthService(
            IAccountRepo accountRepo,
            ILoginRepo loginRepo,
            IAccountRoleRepo accountRoleRepo,
            IStudentExtensionRepo studentExtensionRepo,
            IJwtTokenGenerator jwt,
            SchoolDbContext db)
        {
            _accountRepo = accountRepo;
            _loginRepo = loginRepo;
            _accountRoleRepo = accountRoleRepo;
            _studentExtensionRepo = studentExtensionRepo;
            _jwt = jwt;
            _db = db;
        }

        public async Task<AuthResponseDto> SignupAsync(SignupRequestDto dto)
        {
            var normalizedEmail = dto.Email.Trim().ToLowerInvariant();
            var normalizedNationalId = dto.NationalId.Trim();

            if (await _loginRepo.ExistsByEmailAsync(normalizedEmail) || await _accountRepo.GetByEmailAsync(normalizedEmail) is not null)
                throw new InvalidOperationException("An account with this email already exists.");

            if (await _accountRepo.GetByNationalIdAsync(normalizedNationalId) is not null)
                throw new InvalidOperationException("An account with this national ID already exists.");

            var role = await _db.Roles
                .AsNoTracking()
                .Where(r => r.BusinessEntity == "GenZCoders")
                .OrderBy(r => r.Id)
                .FirstOrDefaultAsync(r => r.RoleName == "Student")
                ?? await _db.Roles
                    .AsNoTracking()
                    .Where(r => r.BusinessEntity == "GenZCoders")
                    .OrderBy(r => r.Id)
                    .FirstOrDefaultAsync();

            if (role == null)
                throw new InvalidOperationException("Roles are not configured in the database.");

            var account = new Account
            {
                FullNameEn = dto.FullNameEn.Trim(),
                FullNameAr = dto.FullNameAr.Trim(),
                NationalId = normalizedNationalId,
                Phone = dto.Phone?.Trim(),
                IsActive = true,
                StatusId = 1,
                Email = normalizedEmail,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                RoleId = role.Id
            };

            await _accountRepo.AddAsync(account);

            var login = new Login
            {
                AccountId = account.Id,
                Email = normalizedEmail,
                PasswordHash = PasswordHasher.Hash(dto.Password),
                StatusId = 1
            };

            await _loginRepo.AddAsync(login);

            await _accountRoleRepo.AddAsync(new AccountRole
            {
                AccountId = account.Id,
                RoleId = role.Id,
                BusinessEntityName = "GenZCoders"
            });

            var studentExtension = new StudentExtension
            {
                AccountId = account.Id,
                IsLeader = false,
                ClassId = null,
                Macaddress = null,
                StatusId = 1,
                EducationalLevelStatusId = dto.EducationalLevelId
            };

            await _studentExtensionRepo.AddAsync(studentExtension);

            return BuildAuthResponse(account.Id, normalizedEmail, role.Id, role.RoleName, "GenZCoders");
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var normalizedEmail = dto.Email.Trim().ToLowerInvariant();
            var login = await _loginRepo.GetByEmailAsync(normalizedEmail);

            if (login == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (login.StatusId != 1 || !login.Account.IsActive)
                throw new UnauthorizedAccessException("Account inactive.");

            if (!PasswordHasher.Verify(dto.Password, login.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            var role = login.Account.AccountRoles
                .Select(ar => ar.Role)
                .FirstOrDefault(r => r!.BusinessEntity == "GenZCoders");

            if (role == null)
                throw new UnauthorizedAccessException("Unauthorized business entity.");

            return BuildAuthResponse(
                login.AccountId,
                login.Email,
                role.Id,
                role.RoleName,
                role.BusinessEntity!
            );
        }

        public async Task<AuthenticatedUserDto> GetCurrentUserAsync(long accountId)
        {
            var account = await _accountRepo.GetByIdAsync(accountId)
                ?? throw new KeyNotFoundException("Account not found.");

            if (!account.IsActive)
                throw new UnauthorizedAccessException("Account inactive.");

            var login = await _loginRepo.GetByEmailAsync(account.Email)
                ?? throw new KeyNotFoundException("Login information not found.");

            var role = login.Account.AccountRoles
                .Select(ar => ar.Role)
                .FirstOrDefault(r => r!.BusinessEntity == "GenZCoders");

            if (role == null)
                throw new UnauthorizedAccessException("Unauthorized business entity.");

            return new AuthenticatedUserDto
            {
                AccountId = login.AccountId,
                Email = login.Email,
                RoleId = role.Id,
                RoleName = role.RoleName,
                BusinessEntity = role.BusinessEntity!
            };
        }

        private AuthResponseDto BuildAuthResponse(long accountId, string email, long roleId, string roleName, string businessEntity)
        {
            var expiresAtUtc = DateTime.UtcNow.AddHours(2);
            var token = _jwt.GenerateToken(accountId, email, roleId, roleName, businessEntity);

            return new AuthResponseDto
            {
                AccessToken = token,
                Token = token,
                ExpiresAtUtc = expiresAtUtc,
                AccountId = accountId,
                Email = email,
                RoleId = roleId,
                RoleName = roleName,
                BusinessEntity = businessEntity
            };
        }
    }
}
