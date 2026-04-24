using GenZCoders.Models;

namespace GenZCoders.Repos.CourseMaterialRepo
{
    public interface ICourseMaterialRepo
    {
       public Task<List<CourseMaterial>> GetAllAsync();
       public Task<CourseMaterial?> GetByIdAsync(long id);
       public Task AddAsync(CourseMaterial material);
       public Task UpdateAsync(CourseMaterial material);
       public Task DeleteAsync(CourseMaterial material);
    }
}
