using GenZCoders.DTOs.ApplicationDto;
using GenZCoders.DTOs.ExamsDto;
using GenZCoders.Models;
using GenZCoders.Repos.ApplicationRepo;
using GenZCoders.Repos.CourseRoundRepo;
using GenZCoders.Repos.ExamRepo;
using GenZCoders.Repos.MediaRepo;
using Microsoft.EntityFrameworkCore;

namespace GenZCoders.Services.ApplicationService
{
    public class ApplicationService : IApplicationService
    {
        private const long DefaultStatusId = 14;

        private readonly IApplicationRepo _repo;
        private readonly IMediaRepository _mediaRepo;
        private readonly ICourseRoundRepo _courseRoundRepo;
        private readonly IExamQuestionBankRepo _examBankRepo;
        private readonly IExamQuestionRepo _examQuestionRepo;
        private readonly IStudentExamAnswerRepo _studentAnswerRepo;

        public ApplicationService(
            IApplicationRepo repo,
            IMediaRepository mediaRepo,
            ICourseRoundRepo courseRoundRepo,
            IExamQuestionBankRepo examBankRepo,
            IExamQuestionRepo examQuestionRepo,
            IStudentExamAnswerRepo studentAnswerRepo)
        {
            _repo = repo;
            _mediaRepo = mediaRepo;
            _courseRoundRepo = courseRoundRepo;
            _examBankRepo = examBankRepo;
            _examQuestionRepo = examQuestionRepo;
            _studentAnswerRepo = studentAnswerRepo;
        }

        public async Task<ApplicationDto> CreateAsync(CreateApplicationDto dto)
        {
            try
            {
                var courseRound = await _courseRoundRepo.GetByIdAsync(dto.CourseRoundId)
                    ?? throw new Exception("Course round not found");

                long statusId = courseRound.AutomatedWorkFlowJump == 15 ? 17 : DefaultStatusId;

                var application = new Application
                {
                    CourseRoundId = dto.CourseRoundId,
                    AccountId = dto.AccountId,
                    ApplicationDate = DateTime.UtcNow,
                    StatusId = statusId,
                    Answer1 = null,
                    Answer2 = null,
                    Answer3 = null,
                    Answer4 = null,
                    Answer5 = null,
                    Answer6 = null,
                    Answer7 = null,
                    Answer8 = null,
                    Answer9 = null,
                    Answer10 = null
                };

                await _repo.AddAsync(application);
                await _repo.SaveChangesAsync();

                if (dto.ExamAnswers?.Any() == true)
                {
                    await SaveExamAnswersAsync(dto.AccountId, dto.CourseRoundId, dto.ExamAnswers);
                }

                return await GetByIdAsync(application.Id)!;
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                throw new Exception($"Database error: {innerMessage}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
        public async Task<List<ExamQuestionDto>> GetExamQuestionsAsync(long courseRoundId)
        {
            var bankEntries = await _examBankRepo.GetByCourseRoundIdAsync(courseRoundId);

            if (!bankEntries.Any()) return new List<ExamQuestionDto>();

            var questionIds = bankEntries
                .Where(b => b.QuestionId.HasValue)
                .Select(b => b.QuestionId!.Value)
                .ToList();

            var questions = await _examQuestionRepo.GetByIdsAsync(questionIds);
            var questionDict = questions.ToDictionary(q => q.Id);

            return bankEntries
                .Where(b => b.QuestionId.HasValue && questionDict.ContainsKey(b.QuestionId.Value))
                .Select(b =>
                {
                    var q = questionDict[b.QuestionId!.Value];
                    return new ExamQuestionDto
                    {
                        Id = q.Id,
                        QuestionTitle = q.QuestionTitle,
                        Choice1 = q.Choice1,
                        Choice2 = q.Choice2,
                        Choice3 = q.Choice3,
                        Choice4 = q.Choice4
                    };
                })
                .ToList();
        }

        public async Task<bool> SubmitExamAnswersAsync(long accountId, long courseRoundId, List<ExamAnswerItemDto> answers)
        {
            await SaveExamAnswersAsync(accountId, courseRoundId, answers);
            return true;
        }

       private async Task SaveExamAnswersAsync(long accountId, long courseRoundId, List<ExamAnswerItemDto> answers)
       {
    try
    {
        // Get bank entries for this course round
        var bankEntries = await _examBankRepo.GetByCourseRoundIdAsync(courseRoundId);
        if (!bankEntries.Any())
            throw new Exception("No exam questions configured for this course round");

        Console.WriteLine($"DEBUG: Found {bankEntries.Count()} bank entries for CourseRound {courseRoundId}");

        // QuestionId -> QuestionbankId
        var questionToBank = bankEntries
            .Where(b => b.QuestionId.HasValue && b.Id != 0)
            .ToDictionary(b => b.QuestionId!.Value, b => (long)b.Id);

        Console.WriteLine($"DEBUG: QuestionToBank mapping: {string.Join(", ", questionToBank.Select(x => $"{x.Key}->{x.Value}"))}");

        // Get questions
        var questionIds = answers.Select(a => a.QuestionId).ToList();
        var questions = await _examQuestionRepo.GetByIdsAsync(questionIds);
        var questionDict = questions.ToDictionary(q => q.Id);

        Console.WriteLine($"DEBUG: Found {questions.Count()} questions for scoring");

        var studentAnswers = answers.Select(a =>
        {
            var question = questionDict.GetValueOrDefault(a.QuestionId);
            var isCorrect = question != null &&
                string.Equals(a.ChoosedAnswer, question.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            var bankId = questionToBank.GetValueOrDefault(a.QuestionId);

            Console.WriteLine($"DEBUG: Answer - QuestionId={a.QuestionId}, BankId={bankId}, Choosed={a.ChoosedAnswer}, Correct={isCorrect}");

            return new StudentExamAnswer
            {
                AccountId = accountId,
                ExamQuestionId = a.QuestionId,
                QuestionbankId = bankId > 0 ? bankId : null,  // <-- Don't set if 0
                ChoosedAnswer = a.ChoosedAnswer,
                Score = isCorrect
            };
        }).ToList();

        await _studentAnswerRepo.AddRangeAsync(studentAnswers);
        await _studentAnswerRepo.SaveChangesAsync();

        Console.WriteLine($"DEBUG: Saved {studentAnswers.Count} answers successfully");
    }
    catch (DbUpdateException dbEx)
    {
        var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
        throw new Exception($"Database error saving answers: {innerMessage}");
    }
}

        public async Task<ApplicationDto?> GetByIdAsync(long id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            var media = await _mediaRepo.GetByOwnerAsync("Application", id);

            if (media.Any() && entity.StatusId != 42)
            {
                entity.StatusId = 42;
                await _repo.SaveChangesAsync();
            }

            var dto = await MapToDtoAsync(entity);

            dto.Media = media.Any()
                ? new MediaForApplicationDto { FilePath = media.First().FilePath }
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
                    app.StatusId = 42;
                }
            }

            await _repo.SaveChangesAsync();

            var dtoTasks = apps.Select(a => MapToDtoAsync(a));
            var dtos = await Task.WhenAll(dtoTasks);

            foreach (var dto in dtos)
            {
                var media = medias.FirstOrDefault(m => m.TableId == dto.Id);
                if (media != null)
                {
                    dto.Media = new MediaForApplicationDto { FilePath = media.FilePath };
                }
            }

            return dtos.ToList();
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

        private async Task<ApplicationDto> MapToDtoAsync(Application a)
        {
            var dto = new ApplicationDto
            {
                Id = a.Id,
                CourseRoundId = a.CourseRoundId,
                AccountId = a.AccountId,
                ApplicationDate = a.ApplicationDate,
                Status = a.Status?.StatusName ?? "Unknown",
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

            // Fetch answers by AccountId + CourseRound (through QuestionbankId)
            var bankEntries = await _examBankRepo.GetByCourseRoundIdAsync(a.CourseRoundId);
            var bankIds = bankEntries.Select(b => (long)b.Id).ToList();

            if (bankIds.Any())
            {
                // Get all student answers for this account linked to these bank entries
                var allAnswers = await _studentAnswerRepo.GetByAccountIdAsync(a.AccountId);
                var studentAnswers = allAnswers
                    .Where(sa => sa.QuestionbankId.HasValue && bankIds.Contains(sa.QuestionbankId.Value))
                    .ToList();

                if (studentAnswers.Any())
                {
                    var questionIds = studentAnswers
                        .Where(sa => sa.ExamQuestionId.HasValue)
                        .Select(sa => sa.ExamQuestionId!.Value)
                        .ToList();

                    var questions = await _examQuestionRepo.GetByIdsAsync(questionIds);
                    var questionDict = questions.ToDictionary(q => q.Id);

                    dto.ExamAnswers = studentAnswers.Select(sa => new StudentExamAnswerDto
                    {
                        QuestionId = sa.ExamQuestionId,
                        QuestionTitle = sa.ExamQuestionId.HasValue && questionDict.ContainsKey(sa.ExamQuestionId.Value)
                            ? questionDict[sa.ExamQuestionId.Value].QuestionTitle
                            : null,
                        ChoosedAnswer = sa.ChoosedAnswer,
                        IsCorrect = sa.Score
                    }).ToList();
                }
            }

            return dto;
        }
    }
}