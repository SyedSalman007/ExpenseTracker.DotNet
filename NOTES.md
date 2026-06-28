# ExpenseTracker — Learning Notes

## Architecture

### Clean Architecture Layers
- **Domain** — Entities, repository interfaces. No dependencies on other layers.
- **Application** — Business logic, DTOs, validators. Depends on Domain only.
- **Infrastructure** — EF Core, repositories, DB context. Implements Domain interfaces.
- **API** — Controllers, middleware, DI wiring. Calls Application layer only.

### Dependency Rule
```
API → Application → Domain ← Infrastructure
```
Domain defines interfaces. Infrastructure implements them. Application uses them via DI — never knows about the database.

### Why API references Infrastructure
Only for DI registration in `Program.cs`. Controllers never directly call Infrastructure — they only talk to Application interfaces.

### Dependency Inversion Principle
Domain defines `IExpenseRepository`. Infrastructure implements it as `ExpenseRepository`. Application depends on the interface, not the class. At runtime DI injects the real implementation — Application has no idea it's talking to PostgreSQL.

---

## Project Setup

### Extension Method Pattern
Each layer registers its own services:
```csharp
// Infrastructure/Extensions/InfrastructureServiceExtensions.cs
public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
{
    services.AddDbContext<AppDbContext>(o => o.UseNpgsql(config.GetConnectionString("Default")));
    services.AddScoped<IExpenseRepository, ExpenseRepository>();
    return services;
}
```
`Program.cs` just calls `builder.Services.AddInfrastructure(builder.Configuration)` — clean and layered.

### global.json
Pins .NET SDK version per project folder. Allows using .NET 10 for this project while system default stays on .NET 8.
```json
{ "sdk": { "version": "10.0.100", "rollForward": "latestMinor" } }
```

### Scalar (replaces Swagger UI)
Modern API docs UI recommended for .NET 9+.
```csharp
app.MapOpenApi();
app.MapScalarApiReference(); // access at /scalar/v1
```

---

## Domain Layer

### BaseEntity
```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}
```
- `Guid` over `int` — no sequential guessing, better for distributed systems
- `DateTime.UtcNow` — always store UTC, convert to local on frontend
- `IsDeleted` — soft delete, never permanently remove financial data
- `UpdatedAt` nullable — null until first update

### Soft Delete Filter in AppDbContext
```csharp
modelBuilder.Entity<Expense>().HasQueryFilter(e => !e.IsDeleted);
```
All queries automatically exclude soft-deleted records globally.

### decimal for Money
Always use `decimal` for financial amounts, never `float` or `double` — floats have precision issues.
```csharp
entity.Property(e => e.Amount).HasPrecision(18, 2);
```

### null! on Navigation Properties
```csharp
public User User { get; set; } = null!;
```
Tells compiler EF Core will always populate this — avoids nullable warnings without lying about the type.

### Repository Interfaces in Domain
Domain defines *what* it needs, Infrastructure fulfills it:
```csharp
// Domain/Repositories/IRepository.cs
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}
```
Specific repositories extend this with domain-specific queries.

---

## Database & EF Core

### Connection String (PostgreSQL)
```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Database=expense_tracker;Username=salmanzaidi;Password="
  }
}
```
On Mac with Homebrew, the superuser is your Mac username, not `postgres`.

### PostgreSQL Default Port
Port `5432` is the standard PostgreSQL port — assigned automatically on install, no configuration needed.

### EF Core Migration Commands

**Create a migration:**
```bash
dotnet ef migrations add InitialCreate --project ExpenseTracker.Infrastructure --startup-project ExpenseTracker.API
```
- `migrations add` — compares entities against last snapshot, generates C# migration code
- `InitialCreate` — migration name (use descriptive names like `AddBudgetTable` for future ones)
- `--project Infrastructure` — where to write migration files (where AppDbContext lives)
- `--startup-project API` — where to read app config from (connection string, DI setup)

**Apply migration to database:**
```bash
dotnet ef database update --project ExpenseTracker.Infrastructure --startup-project ExpenseTracker.API
```
Runs the `Up()` method in the migration — creates actual tables in PostgreSQL.

**Generated migration files:**
```
Migrations/
├── 20260628_InitialCreate.cs           ← Up() and Down() methods
├── 20260628_InitialCreate.Designer.cs  ← EF metadata
└── AppDbContextModelSnapshot.cs        ← current model snapshot for diffing
```

**`__EFMigrationsHistory` table**
EF creates this automatically to track which migrations have been applied.

### Must-run from solution root
Migration commands must be run from the folder containing the `.sln` file, not from inside any project folder.

---

## ABP Framework vs Clean Architecture

| | ABP (Talverse) | This Project |
|---|---|---|
| App Layer | Interface + Class | Interface + Class |
| Domain Layer | Interface + Class (Domain Service) | Interfaces only |
| Infrastructure | Interface + Class | Interface + Class |
| Use when | Multi-tenant SaaS, enterprise scale | Learning fundamentals |

ABP abstracts auth, audit logging, permissions, multi-tenancy. This project builds those manually to understand what ABP hides.

---

## Tooling

| Tool | Purpose |
|---|---|
| Rider | Primary .NET IDE |
| TablePlus | PostgreSQL GUI client |
| SourceTree | Git GUI |
| Scalar | API documentation UI |
| PostgreSQL 17 | Database |
