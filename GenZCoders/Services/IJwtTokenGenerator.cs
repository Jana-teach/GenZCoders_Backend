namespace GenZCoders.Services
{
    public interface IJwtTokenGenerator
    {
       public string GenerateToken(long accountId, string email, long roleId, string roleName, string businessEntity);
    }
}
