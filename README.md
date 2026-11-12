# Task Manager

A simple task management app for an internal team. The back-end is a .NET 8 Web API, the front-end is Angular, and the data is stored in SQL Server.

## Tech Used

- Back-end: .NET 8 Web API, Entity Framework Core 8 (Code First)
- Front-end: Angular 18, plain CSS (no CSS framework)
- Database: SQL Server Express (local)
- API testing: Swagger

## What You Need

- .NET 8 SDK
- Node.js (LTS) and Angular CLI (`npm install -g @angular/cli`)
- SQL Server Express (or LocalDB)
- EF Core tool: `dotnet tool install --global dotnet-ef --version 8.0.11`

## How to Run

### 1. Database and Back-end

Open `TaskManager.Api/appsettings.json` and check the connection string. The default is:

```
Server=localhost\SQLEXPRESS;Database=TaskManagerDb;Trusted_Connection=True;TrustServerCertificate=True;
```

If you use LocalDB, change the server to `(localdb)\MSSQLLocalDB`.

Then run:

```
cd TaskManager.Api
dotnet ef database update
dotnet run --urls http://localhost:5000
```

- Swagger: http://localhost:5000/swagger

`dotnet ef database update` creates the database, creates the `Tasks` table and adds 10 sample tasks. The migration is already in the repo, so there is no need to create a new one.

### 2. Front-end

Open a second terminal:

```
cd task-ui
npm install
ng serve
```

- App: http://localhost:4200

Keep the API running while you use the app. The front-end calls the API at `http://localhost:5000`. If you change the API port, update `apiUrl` in `task-ui/src/app/services/task.service.ts`.

## Task Fields

| Field | Notes |
|-------|-------|
| Id | Auto number |
| Title | Required, max 200 characters |
| Description | Optional, max 1000 characters |
| Status | `ToDo`, `InProgress`, `Done` |
| Priority | `Low`, `Medium`, `High`, `Critical` |
| AssignedTo | Optional text, max 100 characters |
| CreatedDate | Set by the API |
| ModifiedDate | Updated by the API on every change |
| IsDeleted | `true` after a soft delete |

## API Endpoints

| Method | URL | What it does |
|--------|-----|--------------|
| GET | /api/tasks | List tasks. Optional filters: `?status=ToDo&priority=High` |
| GET | /api/tasks/{id} | Get one task |
| POST | /api/tasks | Create a task |
| PUT | /api/tasks/{id} | Update a task |
| DELETE | /api/tasks/{id} | Soft delete a task |
| GET | /api/tasks/summary | Count of tasks grouped by status and priority |

Example body for POST and PUT:

```json
{
  "title": "Fix login bug",
  "description": "Users cannot log in on mobile",
  "status": "ToDo",
  "priority": "High",
  "assignedTo": "Rahul"
}
```

Errors always come back in the same format (ProblemDetails JSON). A missing title or a wrong status value gives a 400. A task that is not found gives a 404.

## Where to Find Each Requirement

| Requirement | Where |
|-------------|-------|
| Filter and sort with LINQ | `TaskService.GetAllAsync` |
| Raw SQL summary (not EF) | `TaskService.GetSummaryAsync` |
| Soft delete | `TaskService.DeleteAsync` and the query filter in `AppDbContext` |
| Input validation | `DTOs/TaskDtos.cs` |
| Global exception handler | `Middleware/GlobalExceptionMiddleware.cs` |
| Seed data (10 tasks) | `Data/SeedData.cs`, added through the EF migration |
| Migrations | `TaskManager.Api/Migrations` |

## Front-end Features

- Task table with priority colours
- Create task form
- Status dropdown to update the status of a task
- Delete button (soft delete)
- Filters for status and priority
- Summary chips on top, from `/api/tasks/summary`
- Buttons show a loading state while a request is running

## Check the Soft Delete

After deleting a task in the UI, run this in SSMS (database `TaskManagerDb`):

```sql
SELECT * FROM Tasks WHERE IsDeleted = 1;
```

The task is still in the table. Only `IsDeleted` is set to 1.

## Notes

- Status and Priority are saved as numbers in the database (Status: 1 ToDo, 2 InProgress, 3 Done. Priority: 1 Low, 2 Medium, 3 High, 4 Critical). The API sends and accepts them as text.
- Soft deleted tasks are hidden from all normal queries by a global query filter.
- Not done (optional in the assignment): authentication, unit tests, Docker, .NET Standard library.

## What I Would Add Next

- Login with JWT
- Pagination and search
- Unit and integration tests
- Docker and docker-compose
- A PATCH endpoint just for status