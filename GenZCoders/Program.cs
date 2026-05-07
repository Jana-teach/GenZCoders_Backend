using GenZCoders.Controllers;
using GenZCoders.Models;
using GenZCoders.Repos.AccountRoleRepo;
using GenZCoders.Repos.ApplicationRepo;
using GenZCoders.Repos.AuthRepo;
using GenZCoders.Repos.CourseMaterialRepo;
using GenZCoders.Repos.CourseRepo;
using GenZCoders.Repos.CourseRoundInstructorRepo;
using GenZCoders.Repos.CourseRoundRepo;
using GenZCoders.Repos.EngineerDashboardRepo;
using GenZCoders.Repos.ExamRepo;
using GenZCoders.Repos.LoginRepo;
using GenZCoders.Repos.MediaRepo;
using GenZCoders.Repos.StudentExtensionRepo;
using GenZCoders.Repos.WeekRepo;
using GenZCoders.Services;
using GenZCoders.Services.AccountService;
using GenZCoders.Services.ApplicationService;
using GenZCoders.Services.AuthService;
using GenZCoders.Services.CourseMaterialService;
using GenZCoders.Services.CourseRoundInstructorService;
using GenZCoders.Services.CourseRoundService;
using GenZCoders.Services.CourseService;
using GenZCoders.Services.EngineerDashboardService;
using GenZCoders.Services.MediaService;
using GenZCoders.Services.WeekService;
using GenZCoders.Services.Zoom;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SchoolDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepo, CourseRepo>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<ILoginRepo, LoginRepo>();
builder.Services.AddScoped<IAccountRoleRepo, AccountRoleRepo>();
builder.Services.AddScoped<ICourseRoundRepo, CourseRoundRepo>();
builder.Services.AddScoped<ICourseRoundService, CourseRoundService>();
builder.Services.AddScoped<IApplicationRepo, ApplicationRepo>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IStudentExtensionRepo, StudentExtensionRepo>();
builder.Services.AddScoped<IWeekRepo, WeekRepo>();
builder.Services.AddScoped<IWeekService, WeekService>();
builder.Services.AddScoped<ICourseMaterialService, CourseMaterialService>();
builder.Services.AddScoped<ICourseMaterialRepo, CourseMaterialRepo>();
builder.Services.AddScoped<ICourseRoundInstructorRepository, CourseRoundInstructorRepository>();
builder.Services.AddScoped<ICourseRoundInstructorService, CourseRoundInstructorService>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IAccountRoleRepo, AccountRoleRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEngineerDashboardRepo, EngineerDashBoardRepo>();
builder.Services.AddScoped<IEngineerDashboardService, EngineerDashboardService>();
builder.Services.AddHttpClient<IZoomService, ZoomService>();
builder.Services.AddScoped<IExamQuestionBankRepo, ExamQuestionBankRepo>();
builder.Services.AddScoped<IExamQuestionRepo, ExamQuestionRepo>();
builder.Services.AddScoped<IStudentExamAnswerRepo, StudentExamAnswerRepo>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Logging.AddConsole();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = feature?.Error;

        if (exception is not null)
        {
            app.Logger.LogError(exception, "Unhandled exception");
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            System.ArgumentException => StatusCodes.Status400BadRequest,
            System.InvalidOperationException => StatusCodes.Status409Conflict,
            System.UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            System.Collections.Generic.KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        await context.Response.WriteAsJsonAsync(new
        {
            message = exception?.Message ?? "An unexpected error occurred."
        });
    });
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
