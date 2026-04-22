using System.ComponentModel.DataAnnotations;

namespace GenZCoders.DTOs.AuthDto
{
    public class SignupRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string FullNameEn { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string FullNameAr { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{14}$")]
        public string NationalId { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\+?\d{10,15}$")]
        public string? Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;

        [Range(1, long.MaxValue)]
        public long EducationalLevelId { get; set; }
    }
}
