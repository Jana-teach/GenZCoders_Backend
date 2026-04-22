using GenZCoders.DTOs.ApplicationDto;
using GenZCoders.DTOs.MediaDto;
using GenZCoders.Models;
using GenZCoders.Repos.ApplicationRepo;
using GenZCoders.Repos.CourseRoundRepo;
using GenZCoders.Repos.MediaRepo;

namespace GenZCoders.Services.ApplicationService
{
    public class ApplicationService : IApplicationService
    {
        private const long DefaultStatusId = 14;

        private readonly IApplicationRepo _repo;
        private readonly IMediaRepository _mediaRepo;
        private readonly ICourseRoundRepo _courseRoundRepo;


        public ApplicationService(IApplicationRepo repo , IMediaRepository mediaRepo , ICourseRoundRepo courseRoundRepo)
        {
            _repo = repo;
            _mediaRepo = mediaRepo;
            _courseRoundRepo = courseRoundRepo;
        }

        public async Task<ApplicationDto> CreateAsync(long accountId, CreateApplicationDto dto)
        {
            var courseRound = await _courseRoundRepo.GetByIdAsync(dto.CourseRoundId)
                ?? throw new Exception("Course round not found");

            long statusId;

            if (courseRound.AutomatedWorkFlowJump == 15)
                statusId = 17; // Accepted
            else
                statusId = DefaultStatusId; // Pending

            var application = new Application
            {
                CourseRoundId = dto.CourseRoundId,
                AccountId = accountId,
                ApplicationDate = DateTime.UtcNow,
                StatusId = statusId,

                Answer1 = dto.Answer1,
                Answer2 = dto.Answer2,
                Answer3 = dto.Answer3,
                Answer4 = dto.Answer4,
                Answer5 = dto.Answer5,
                Answer6 = dto.Answer6,
                Answer7 = dto.Answer7,
                Answer8 = dto.Answer8,
                Answer9 = dto.Answer9,
                Answer10 = dto.Answer10
            };

            await _repo.AddAsync(application);
            await _repo.SaveChangesAsync();

            return await GetByIdAsync(application.Id)!;
        }


        public async Task<ApplicationDto?> GetByIdAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            var media = await _mediaRepo.GetByOwnerAsync("Application", id);

            // 🔹 Automatically update status to Paid (42) if media exists
            if (media.Any() && entity.StatusId != 42)
            {
                entity.StatusId = 42;
                await _repo.SaveChangesAsync();
            }

            var dto = MapToDto(entity);

            dto.Media = media.Any()
                ? new MediaForApplicationDto
                {
                    FilePath = media.First().FilePath
                }
                : null;

            return dto;
        }
        public async Task<List<ApplicationDto>> GetAllAsync()
        {
            var apps = await _repo.GetAllAsync();
            var appIds = apps.Select(a => a.Id).ToList();
            var medias = await _mediaRepo.GetByOwnerAsync("Application", appIds);

            foreach (var app in apps)
            {
                var media = medias.FirstOrDefault(m => m.TableId == app.Id);
                if (media != null && app.StatusId != 42)
                {
                    app.StatusId = 42; // update to Paid
                }
            }

            await _repo.SaveChangesAsync(); // save all changes

            return apps.Select(a =>
            {
                var dto = MapToDto(a);
                var media = medias.FirstOrDefault(m => m.TableId == a.Id);
                if (media != null)
                {
                    dto.Media = new MediaForApplicationDto
                    {
                        FilePath = media.FilePath
                    };
                }
                return dto;
            }).ToList();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            _repo.Remove(entity);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> PatchStatusAsync(long id, PatchApplicationStatusDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            entity.StatusId = dto.StatusId;
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> PatchCourseRoundAsync(long id, PatchApplicationCourseRoundDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            var roundExists = await _courseRoundRepo.GetByIdAsync(dto.CourseRoundId) != null;
            if (!roundExists)
                throw new ArgumentException("Course round not found");

            entity.CourseRoundId = dto.CourseRoundId;
            return await _repo.SaveChangesAsync();
        }

        private static ApplicationDto MapToDto(Application a)
        {
            return new ApplicationDto
            {
                Id = a.Id,
                CourseRoundId = a.CourseRoundId,
                AccountId = a.AccountId,
                ApplicationDate = a.ApplicationDate,
                Status = a.Status?.StatusName ?? "Unknown",  // safe fallback

                FullNameEn = a.Account?.FullNameEn,
                Email = a.Account?.Email,
                Phone = a.Account?.Phone,

                Answer1 = a.Answer1,
                Answer2 = a.Answer2,
                Answer3 = a.Answer3,
                Answer4 = a.Answer4,
                Answer5 = a.Answer5,
                Answer6 = a.Answer6,
                Answer7 = a.Answer7,
                Answer8 = a.Answer8,
                Answer9 = a.Answer9,
                Answer10 = a.Answer10
            };
        }

    }

}
