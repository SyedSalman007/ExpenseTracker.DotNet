# ExpenseTracker

A personal expense tracking REST API built with .NET 10 following Clean Architecture principles. Built as a portfolio project to demonstrate foundational .NET skills independent of frameworks like ABP.

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 Web API |
| ORM | EF Core 10 |
| Database | PostgreSQL 17 |
| Validation | FluentValidation |
| Mapping | Mapster |
| Auth | ASP.NET Core Identity + JWT Bearer |
| API Docs | Scalar |

## Architecture

Clean Architecture with four layers:

- **Domain** — Entities and repository interfaces. No external dependencies.
- **Application** — Business logic, DTOs, validators. Depends on Domain only.
- **Infrastructure** — EF Core, repository implementations, AppDbContext.
- **API** — Controllers, middleware, DI wiring.

## Getting Started

### Prerequisites
- .NET 10 SDK
- PostgreSQL 17

### Setup

1. Clone the repository
2. Create a PostgreSQL database named `expense_tracker`
3. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Database=expense_tracker;Username=yourusername;Password="
  }
}
```
4. Run migrations:
```bash
dotnet ef database update --project ExpenseTracker.Infrastructure --startup-project ExpenseTracker.API
```
5. Run the project:
```bash
dotnet run --project ExpenseTracker.API
```
6. Access API docs at `http://localhost:{port}/scalar/v1`
