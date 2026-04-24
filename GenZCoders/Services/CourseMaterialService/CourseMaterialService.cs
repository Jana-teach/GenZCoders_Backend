using GenZCoders.DTOs.CourseMaterialDto;
using GenZCoders.DTOs.WeekDto;
using GenZCoders.Models;
using GenZCoders.Models;
using GenZCoders.Repos.CourseMaterialRepo;
using GenZCoders.Services.Zoom;

namespace GenZCoders.Services.CourseMaterialService
{
    public class CourseMaterialService : ICourseMaterialService
    {
        private readonly SchoolDbContext _context;
        private readonly ICourseMaterialRepo _repo;
        private readonly IZoomService _zoomService;

        public CourseMaterialService(SchoolDbContext context, ICourseMaterialRepo repo, IZoomService zoomService)
        {
            _context = context;
            _repo = repo;
            _zoomService = zoomService;
        }

        // GET ALL
        public async Task<List<ReadCourseMaterialDto>> GetAllAsync()
        {
            var materials = await _repo.GetAllAsync();
            return materials.Select(m => MapToDto(m)).ToList();
        }

        // GET BY ID
        public async Task<ReadCourseMaterialDto?> GetByIdAsync(long id)
        {
            var material = await _repo.GetByIdAsync(id);
            return material == null ? null : MapToDto(material);
        }

        // CREATE
        public async Task<ReadCourseMaterialDto> CreateAsync(CreateCourseMaterialDto dto)
        {
            var material = new CourseMaterial
            {
                CourseRoundId = dto.CourseRoundId,
                CreatedByAccountId = dto.CreatedByAccountId,
                WeekId = dto.WeekId,
                ParentMaterialId = dto.ParentMaterialId,
                StatusId = 1, // default status
                MaterialTypeStatusId = dto.MaterialTypeStatusId,
                Title = dto.Title,
                Description = dto.Description,
                Link = dto.Link,
                MeetingId = dto.MeetingId,
                MeetingPassword = dto.MeetingPassword
            };

            _context.CourseMaterials.Add(material);
            await _context.SaveChangesAsync();

            return MapToDto(material);
        }

        public async Task<ReadCourseMaterialDto> CreateZoomMaterialAsync(CreateZoomCourseMaterialDto dto)
        {
            // Create Zoom meeting via Zoom API
            var zoomMeeting = await _zoomService.CreateMeetingAsync(dto.Title, dto.StartTimeUtc, dto.DurationMinutes);

            // 34 = ZoomLink (material type), 38 = Scheduled (status)
            const long zoomLinkTypeId = 34;
            const long scheduledStatusId = 38;

            var material = new CourseMaterial
            {
                CourseRoundId = dto.CourseRoundId,
                CreatedByAccountId = dto.CreatedByAccountId,
                WeekId = dto.WeekId,
                ParentMaterialId = null,
                StatusId = scheduledStatusId,
                MaterialTypeStatusId = zoomLinkTypeId,
                Title = dto.Title,
                Description = dto.Description,
                Link = zoomMeeting.JoinUrl,
                MeetingId = zoomMeeting.Id,
                MeetingPassword = zoomMeeting.Password
            };

            _context.CourseMaterials.Add(material);
            await _context.SaveChangesAsync();

            return MapToDto(material);
        }

        // UPDATE
        public async Task<bool> UpdateAsync(long id, UpdateCourseMaterialDto dto)
        {
            var material = await _repo.GetByIdAsync(id);
            if (material == null) return false;

            material.Title = dto.Title ?? material.Title;
            material.Description = dto.Description ?? material.Description;
            material.Link = dto.Link ?? material.Link;
            material.MeetingId = dto.MeetingId ?? material.MeetingId;
            material.MeetingPassword = dto.MeetingPassword ?? material.MeetingPassword;
            material.MaterialTypeStatusId = dto.MaterialTypeStatusId ?? material.MaterialTypeStatusId;
            material.ParentMaterialId = dto.ParentMaterialId ?? material.ParentMaterialId;

            await _repo.UpdateAsync(material);
            return true;
        }

        // PATCH (only update StatusId)
        public async Task<bool> UpdateStatusAsync(long id, long statusId)
        {
            var material = await _repo.GetByIdAsync(id);
            if (material == null) return false;

            material.StatusId = statusId;
            await _repo.UpdateAsync(material);
            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(long id)
        {
            var material = await _repo.GetByIdAsync(id);
            if (material == null) return false;

            await _repo.DeleteAsync(material);
            return true;
        }

        public async Task<bool> PatchAsync(long id, PatchCourseMaterialDto dto)
        {
            var material = await _repo.GetByIdAsync(id);
            if (material == null) return false;

            // Only update non-null fields from DTO
            if (dto.StatusId.HasValue) material.StatusId = dto.StatusId.Value;
            if (dto.Title != null) material.Title = dto.Title;
            if (dto.Description != null) material.Description = dto.Description;
            if (dto.Link != null) material.Link = dto.Link;
            if (dto.MeetingId != null) material.MeetingId = dto.MeetingId;
            if (dto.MeetingPassword != null) material.MeetingPassword = dto.MeetingPassword;
            //if (dto.ParentMaterialId.HasValue) material.ParentMaterialId = dto.ParentMaterialId;

            await _repo.UpdateAsync(material);
            return true;
        }


        // --- Helper method: Map EF entity to DTO ---
        private ReadCourseMaterialDto MapToDto(CourseMaterial m)
        {
            return new ReadCourseMaterialDto
            {
                Id = m.Id,
                CourseRoundId = m.CourseRoundId,
                CreatedByAccountId = m.CreatedByAccountId,
                WeekId = m.WeekId,
                ParentMaterialId = m.ParentMaterialId,
                StatusId = m.StatusId,
                MaterialTypeStatusId = m.MaterialTypeStatusId,
                Title = m.Title,
                Description = m.Description,
                Link = m.Link,
                MeetingId = m.MeetingId,
                MeetingPassword = m.MeetingPassword
            };
        }
    }
}
