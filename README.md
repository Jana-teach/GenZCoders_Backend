# GenZCoders Backend

GenZCoders Backend is a RESTful Web API built with **ASP.NET Core** for managing programming courses and the complete student enrollment lifecycle. The system provides secure authentication, course management, application processing, instructor assignment, and dashboard analytics through a clean, scalable architecture.

## Features

- JWT Authentication and Authorization
- Course CRUD operations
- Course Round management
- Student application management
- Instructor assignment to course rounds
- Assignment and submission management
- Course materials management
- Dashboard statistics and analytics
- RESTful API with Swagger documentation

## Tech Stack

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- LINQ
- Repository Pattern
- Dependency Injection
- Swagger (OpenAPI)

## Project Structure

```
Controllers/
Services/
Repositories/
Models/
DTOs/
Database/
Authentication/
Migrations/
```

## Getting Started

Clone the repository:

```bash
git clone https://github.com/your-username/GenZCodersBackend.git
```

Navigate to the project:

```bash
cd GenZCodersBackend
```

Restore dependencies:

```bash
dotnet restore
```

Apply database migrations:

```bash
dotnet ef database update
```

Run the project:

```bash
dotnet run
```

## Future Improvements

- Email notifications
- Certificate generation
- Payment gateway integration
- Attendance management
- Docker support
- CI/CD pipeline

## Author

**Jana Mostafa**  
Backend Developer | ASP.NET Core | C# | SQL Server
