# Task Manager

A simple task management app. It has a .NET 8 Web API at the back, an Angular app at the front, and SQL Server for the data.

## Tech Used

- Back-end: .NET 8 Web API, Entity Framework Core (Code First)
- Front-end: Angular 18 with plain CSS
- Database: SQL Server Express (local)

## What You Need

- .NET 8 SDK
- Node.js (LTS) and Angular CLI
- SQL Server Express
- EF Core tool: `dotnet tool install --global dotnet-ef --version 8.0.11`

## How to Run

### 1. Database and Back-end

Open `TaskManager.Api/appsettings.json` and check the connection string matches your SQL Server name. The default is `localhost\SQLEXPRESS`.

```
cd TaskManager.Api
dotnet ef database update
dotnet run --urls http://localhost:5000
```

- Swagger: http://localhost:5000/swagger

`dotnet ef database update` creates the database and adds 10 sample tasks.

### 2. Front-end

Open a new terminal:

```
cd task-ui
npm install
ng serve
```

- App: http://localhost:4200

## API Endpoints

| Method | URL | What it does |
|--------|-----|--------------|
| GET | /api/tasks | List tasks. Optional filters: `?status=ToDo&priority=High` |
| GET | /api/tasks/{id} | Get one task |
| POST | /api/tasks | Create a task |
| PUT | /api/tasks/{id} | Update a task |
| DELETE | /api/tasks/{id} | Soft delete a task |
| GET | /api/tasks/summary | Count of tasks by status and priority |

Status values: `ToDo`, `InProgress`, `Done`
Priority values: `Low`, `Medium`, `High`, `Critical`

## What is Done

- CRUD API with filter by status and priority
- Soft delete: the row stays in the database, only `IsDeleted` is set to true
- EF Core Code First migration with 10 seed tasks
- Validation: Title is required, Status and Priority must be valid values
- Global exception handler that gives the same error format every time
- LINQ query for filter and sort (in `TaskService.GetAllAsync`)
- Raw SQL query for the summary (in `TaskService.GetSummaryAsync`)
- Angular UI with:
  - Task table
  - Create task form
  - Status dropdown to update status
  - Priority colours
  - Filters
  - Summary chips

## Folder Structure

```
TaskManagement/
├── TaskManager.Api/   (back-end)
└── task-ui/           (front-end)
```