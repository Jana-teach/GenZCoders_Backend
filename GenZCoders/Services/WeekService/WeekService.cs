using GenZCoders.DTOs.WeekDto;
using GenZCoders.Models;
using GenZCoders.Models.GenZCoders.Models;
using GenZCoders.Repos.WeekRepo;
using Microsoft.EntityFrameworkCore;
using System;

namespace GenZCoders.Services.WeekService
{
    public class WeekService : IWeekService
    {
        private readonly SchoolDbContext _context;
        private readonly IWeekRepo _repo;

        public WeekService(SchoolDbContext context, IWeekRepo repo)
        {
            _context = context;
            _repo = repo;
        }

        public async Task<List<WeekDto>> GetAllAsync()
        {
            var weeks = await _context.Weeks
               .Include(w => w.CourseMaterials)
                   .ThenInclude(cm => cm.ChildMaterials)
               .Where(w => w.BusinessEntityName == "GenZCoders")
               .Select(w => new WeekDto
               {
                   Id = w.Id,
                   WeekTitle = w.WeekTitle,
                   StartDate = w.StartDate,
                   EndDate = w.EndDate,
                   CourseMaterials = w.CourseMaterials.Select(cm => new CourseMaterialDto
                   {
                       Id = cm.Id,
                       CourseRoundId = cm.CourseRoundId,
                       CreatedByAccountId = cm.CreatedByAccountId,
                       ParentMaterialId = cm.ParentMaterialId,
                       StatusId = cm.StatusId,
                       MaterialTypeStatusId = cm.MaterialTypeStatusId,
                       Title = cm.Title,
                       Description = cm.Description,
                       Link = cm.Link,
                       MeetingId = cm.MeetingId,
                       MeetingPassword = cm.MeetingPassword,
                       ChildMaterials = cm.ChildMaterials.Select(ch => new CourseMaterialDto
                       {
                           Id = ch.Id,
                           Title = ch.Title,
                           Description = ch.Description
                       }).ToList()
                   }).ToList()
               })
               .ToListAsync();

            return weeks;
        }


        public async Task<WeekDto?> GetByIdAsync(int id)
        {
            var week = await _context.Weeks
                .Include(w => w.CourseMaterials)
                    .ThenInclude(cm => cm.ChildMaterials)
                .Where(w => w.Id == id && w.BusinessEntityName == "GenZCoders")
                .Select(w => new WeekDto
                {
                    Id = w.Id,
                    WeekTitle = w.WeekTitle,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    CourseMaterials = w.CourseMaterials.Select(cm => new CourseMaterialDto
                    {
                        Id = cm.Id,
                        CourseRoundId = cm.CourseRoundId,
                        CreatedByAccountId = cm.CreatedByAccountId,
                        ParentMaterialId = cm.ParentMaterialId,
                        StatusId = cm.StatusId,
                        MaterialTypeStatusId = cm.MaterialTypeStatusId,
                        Title = cm.Title,
                        Description = cm.Description,
                        Link = cm.Link,
                        MeetingId = cm.MeetingId,
                        MeetingPassword = cm.MeetingPassword,
                        ChildMaterials = cm.ChildMaterials.Select(ch => new CourseMaterialDto
                        {
                            Id = ch.Id,
                            Title = ch.Title,
                            Description = ch.Description
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return week;
        }

        public async Task<WeekDto> CreateAsync(CreateWeekDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var week = new Weeks
            {
                WeekTitle = dto.WeekTitle,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                BusinessEntityName = "GenZCoders"
            };

            _context.Weeks.Add(week);
            await _context.SaveChangesAsync();

            var material = new CourseMaterial
            {
                CourseRoundId = dto.CourseRoundId,
                CreatedByAccountId = dto.AccountId,
                WeekId = week.Id,
                ParentMaterialId = null,
                StatusId = 1,
                MaterialTypeStatusId = 35,
                Title = "First Material",
                Description = "First Material",
                Link = null,
                MeetingId = null,
                MeetingPassword = null
            };

            _context.CourseMaterials.Add(material);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new WeekDto
            {
                Id = week.Id,
                WeekTitle = week.WeekTitle,
                StartDate = week.StartDate,
                EndDate = week.EndDate,
                CourseMaterials = new List<CourseMaterialDto>
                {
                    new CourseMaterialDto
                    {
                        Id = material.Id,
                        CourseRoundId = material.CourseRoundId,
                        CreatedByAccountId = material.CreatedByAccountId,
                        ParentMaterialId = null,
                        StatusId = material.StatusId,
                        MaterialTypeStatusId = material.MaterialTypeStatusId,
                        Title = material.Title,
                        Description = material.Description,
                        Link = null,
                        MeetingId = null,
                        MeetingPassword = null,
                        ChildMaterials = new List<CourseMaterialDto>()
                    }
                }
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateWeekDto dto)
        {
            var week = await _repo.GetByIdAsync(id);
            if (week == null) return false;

            week.WeekTitle = dto.WeekTitle ?? week.WeekTitle;
            week.StartDate = dto.StartDate ?? week.StartDate;
            week.EndDate = dto.EndDate ?? week.EndDate;

            var defaultMaterial = await _context.CourseMaterials
                .Where(cm => cm.WeekId == week.Id && cm.ParentMaterialId == null)
                .FirstOrDefaultAsync();


            await _repo.UpdateAsync(week);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var week = await _repo.GetByIdAsync(id);
            if (week == null) return false;

            var materials = await _context.CourseMaterials
                .Where(cm => cm.WeekId == week.Id)
                .ToListAsync();

            _context.CourseMaterials.RemoveRange(materials);

            await _repo.DeleteAsync(week);
            return true;
        }
    }

}