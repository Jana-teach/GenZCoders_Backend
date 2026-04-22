namespace GenZCoders.DTOs.AuthDto
{
    public class AuthenticatedUserDto
    {
        public long AccountId { get; set; }
        public string Email { get; set; } = null!;
        public long RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public string BusinessEntity { get; set; } = null!;
    }
}
