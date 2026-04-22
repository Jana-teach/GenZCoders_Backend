using GenZCoders.Models;

namespace GenZCoders.Repos.StudentExtensionRepo
{
    public interface IStudentExtensionRepo
    {
        Task AddAsync(StudentExtension entity);
        Task<StudentExtension?> GetByAccountIdAsync(long accountId);
        Task<bool> ExistsAsync(long accountId);
    }
}
