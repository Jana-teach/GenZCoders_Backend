using GenZCoders.DTOs.CourseDto;
using GenZCoders.DTOs.MediaDto;
using GenZCoders.Models;
using GenZCoders.Repos.CourseRepo;
using GenZCoders.Repos.MediaRepo;

namespace GenZCoders.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepo _repo;
        private readonly IMediaRepository _mediaRepo;

        public CourseService(ICourseRepo repo, IMediaRepository mediaRepo)
        {
            _repo = repo;
            _mediaRepo = mediaRepo;
        }

        public async Task<List<CourseDto>> GetAllAsync()
        {
            var courses = await _repo.GetAllAsync();

            var courseIds = courses.Select(c => c.Id).ToList();

            var medias = await _mediaRepo.GetByOwnerAsync("Course", courseIds);

            return courses.Select(c =>
            {
                var media = medias.FirstOrDefault(m => m.TableId == c.Id);

                return new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    LevelStatus = c.LevelStatus.StatusName,
                    DurationHours = c.DurationHours,

                    Media = media != null
                        ? new MediaForCourseDto
                        {
                            FilePath = media.FilePath
                        }
                        : null
                };
            }).ToList();
        }


        public async Task<CourseDto?> GetByIdAsync(long id)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null) return null;

            var media = await _mediaRepo.GetByOwnerAsync("Course", id);

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                LevelStatus = course.LevelStatus.StatusName,
                DurationHours = course.DurationHours,

                Media = media.Any()
                    ? new MediaForCourseDto
                    {
                        FilePath = media.First().FilePath
                    }
                    : null
            };
        }


        public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
        {
            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                LevelStatusId = dto.LevelStatusId,
                DurationHours = dto.DurationHours
            };

            await _repo.AddAsync(course);
            await _repo.SaveChangesAsync();

            return await GetByIdAsync(course.Id);
        }

        public async Task<bool> UpdateAsync(long id, UpdateCourseDto dto)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null) return false;

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.LevelStatusId = dto.LevelStatusId;
            course.DurationHours = dto.DurationHours;

            _repo.Update(course);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> PatchAsync(long id, PatchCourseDto dto)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null) return false;

            if (dto.Title != null)
                course.Title = dto.Title;

            if (dto.Description != null)
                course.Description = dto.Description;

            if (dto.LevelStatusId.HasValue)
                course.LevelStatusId = dto.LevelStatusId.Value;

            if (dto.DurationHours.HasValue)
                course.DurationHours = dto.DurationHours.Value;

            _repo.Update(course);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var course = await _repo.GetByIdAsync(id);
            if (course == null) return false;

            _repo.Remove(course);
            return await _repo.SaveChangesAsync();
        }
    }

}
