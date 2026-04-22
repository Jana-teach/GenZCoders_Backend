using GenZCoders.DTOs.CourseRoundDto;
using GenZCoders.Models;
using GenZCoders.Repos.CourseRoundRepo;

namespace GenZCoders.Services.CourseRoundService
{
    public class CourseRoundService : ICourseRoundService
    {
        private readonly ICourseRoundRepo _repo;

        public CourseRoundService(ICourseRoundRepo repo)
        {
            _repo = repo;
        }

        public async Task<List<CourseRoundDto>> GetAllAsync()
        {
            var rounds = await _repo.GetAllAsync();

            return rounds.Select(r => new CourseRoundDto
            {
                Id = r.Id,
                CourseId = r.CourseId,
                RoundNumber = r.RoundNumber,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                MinStudents = r.MinStudents,
                MaxStudents = r.MaxStudents,
                Price = r.Price,
                CreatedAt = r.CreatedAt,
                Status = r.Status.StatusName,
                CourseRoundGroupId = r.CourseRoundGroupId,
                Question1 = r.Question1,
                Question2 = r.Question2,
                Question3 = r.Question3,
                Question4 = r.Question4,
                Question5 = r.Question5,
                Question6 = r.Question6,
                Question7 = r.Question7,
                Question8 = r.Question8,
                Question9 = r.Question9,
                Question10 = r.Question10,

                InstructorId = r.CourseRoundInstructors
                    .OrderByDescending(x => x.AssignedDate)
                    .Select(x => x.InstructorId)
                    .FirstOrDefault(),

                InstructorName = r.CourseRoundInstructors
                    .OrderByDescending(x => x.AssignedDate)
                    .Select(x => x.Instructor.FullNameEn)
                    .FirstOrDefault(),

                Groups = r.GroupedRounds.Select(gr => new CourseRoundGroupItemDto
                {
                    Id = gr.Id,
                    RoundNumber = gr.RoundNumber,
                    StartDate = gr.StartDate,
                    EndDate = gr.EndDate,
                    Price = gr.Price
                }).ToList()

            }).ToList();
        }


        public async Task<CourseRoundDetailsDto?> GetByIdAsync(long id)
        {
            var round = await _repo.GetByIdAsync(id);
            if (round == null) return null;

            return new CourseRoundDetailsDto
            {
                Id = round.Id,
                RoundNumber = round.RoundNumber,
                CourseId = round.CourseId,
                CourseName = round.Course.Title,
                StartDate = round.StartDate,
                EndDate = round.EndDate,
                Price = round.Price,
                CourseRoundGroupId = round.CourseRoundGroupId,
                MinStudents = round.MinStudents,
                MaxStudents = round.MaxStudents,
                Status = round.Status?.StatusName,
                Question1 = round.Question1,
                Question2 = round.Question2,
                Question3 = round.Question3,
                Question4 = round.Question4,
                Question5 = round.Question5,
                Question6 = round.Question6,
                Question7 = round.Question7,
                Question8 = round.Question8,
                Question9 = round.Question9,
                Question10 = round.Question10,
                InstructorId = round.CourseRoundInstructors
                    .OrderByDescending(x => x.AssignedDate)
                    .Select(x => (long?)x.InstructorId)
                    .FirstOrDefault(),
                InstructorName = round.CourseRoundInstructors
                    .OrderByDescending(x => x.AssignedDate)
                    .Select(x => x.Instructor.FullNameEn)
                    .FirstOrDefault(),

                WeekTitles = round.CourseMaterials
                    .Where(cm => cm.Week != null)
                    .Select(cm => cm.Week!.WeekTitle)
                    .Distinct()
                    .ToList(),

                Groups = round.GroupedRounds.Select(gr => new CourseRoundGroupItemDto
                {
                    Id = gr.Id,
                    RoundNumber = gr.RoundNumber,
                    StartDate = gr.StartDate,
                    EndDate = gr.EndDate,
                    Price = gr.Price
                }).ToList()
            };
        }


        public async Task<CourseRoundDetailsDto> CreateAsync(CreateCourseRoundDto dto)
        {
            if (dto.CourseRoundGroupId.HasValue)
            {
                var parentRound = await _repo.GetByIdAsync(dto.CourseRoundGroupId.Value);
                if (parentRound == null)
                    throw new Exception("Invalid CourseRoundGroupId");
            }

            var round = new CourseRound
            {
                CourseId = dto.CourseId,
                RoundNumber = dto.RoundNumber,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                MinStudents = dto.MinStudents,
                MaxStudents = dto.MaxStudents,
                Price = dto.Price,
                StatusId = 38,
                CreatedAt = DateTime.UtcNow,
                CourseRoundGroupId = dto.CourseRoundGroupId,

                Question1 = dto.Question1,
                Question2 = dto.Question2,
                Question3 = dto.Question3,
                Question4 = dto.Question4,
                Question5 = dto.Question5,
                Question6 = dto.Question6,
                Question7 = dto.Question7,
                Question8 = dto.Question8,
                Question9 = dto.Question9,
                Question10 = dto.Question10,

                AutomatedWorkFlowJump = dto.AutomatedWorkFlowJump,
            };

            await _repo.AddAsync(round);
            await _repo.SaveChangesAsync();

            return await GetByIdAsync(round.Id);
        }

        public async Task<bool> UpdateAsync(long id, UpdateCourseRoundDto dto)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return false;

            if (dto.CourseRoundGroupId.HasValue)
            {
                var parentRound = await _repo.GetByIdAsync(dto.CourseRoundGroupId.Value);
                if (parentRound == null)
                    throw new Exception("Invalid CourseRoundGroupId");
            }

            r.CourseId = dto.CourseId;
            r.RoundNumber = dto.RoundNumber;
            r.StartDate = dto.StartDate;
            r.EndDate = dto.EndDate;
            r.MinStudents = dto.MinStudents;
            r.MaxStudents = dto.MaxStudents;
            r.Price = dto.Price;
            r.StatusId = dto.StatusId;

            r.Question1 = dto.Question1;
            r.Question2 = dto.Question2;
            r.Question3 = dto.Question3;
            r.Question4 = dto.Question4;
            r.Question5 = dto.Question5;
            r.Question6 = dto.Question6;
            r.Question7 = dto.Question7;
            r.Question8 = dto.Question8;
            r.Question9 = dto.Question9;
            r.Question10 = dto.Question10;

            _repo.Update(r);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> PatchAsync(long id, PatchCourseRoundDto dto)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return false;

            if (dto.CourseRoundGroupId.HasValue)
            {
                var parentRound = await _repo.GetByIdAsync(dto.CourseRoundGroupId.Value);
                if (parentRound == null)
                    throw new Exception("Invalid CourseRoundGroupId");
            }

            if (dto.CourseId.HasValue) r.CourseId = dto.CourseId.Value;
            if (dto.RoundNumber.HasValue) r.RoundNumber = dto.RoundNumber.Value;
            if (dto.StartDate.HasValue) r.StartDate = dto.StartDate.Value;
            if (dto.EndDate.HasValue) r.EndDate = dto.EndDate.Value;
            if (dto.MinStudents.HasValue) r.MinStudents = dto.MinStudents.Value;
            if (dto.MaxStudents.HasValue) r.MaxStudents = dto.MaxStudents.Value;
            if (dto.Price.HasValue) r.Price = dto.Price.Value;
            if (dto.StatusId.HasValue) r.StatusId = dto.StatusId.Value;

            if (dto.Question1 != null) r.Question1 = dto.Question1;
            if (dto.Question2 != null) r.Question2 = dto.Question2;
            if (dto.Question3 != null) r.Question3 = dto.Question3;
            if (dto.Question4 != null) r.Question4 = dto.Question4;
            if (dto.Question5 != null) r.Question5 = dto.Question5;
            if (dto.Question6 != null) r.Question6 = dto.Question6;
            if (dto.Question7 != null) r.Question7 = dto.Question7;
            if (dto.Question8 != null) r.Question8 = dto.Question8;
            if (dto.Question9 != null) r.Question9 = dto.Question9;
            if (dto.Question10 != null) r.Question10 = dto.Question10;

            _repo.Update(r);
            return await _repo.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return false;

            _repo.Remove(r);
            return await _repo.SaveChangesAsync();
        }
    }

}
