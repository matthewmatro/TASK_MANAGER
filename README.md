# TaskManager

TaskManager is a simple task tracking backend API built with .NET 8. The solution contains two projects:

- `TaskAPI` - ASP.NET Core Web API that exposes endpoints to manage tasks.
- `CommonObjects` - shared models used by the API (for example `TaskObject`).

This README explains how to build, run and test the API locally.

Prerequisites

- .NET 8 SDK
- A SQL Server instance (local or remote)
- (Optional) `dotnet-ef` global tool for migrations: `dotnet tool install --global dotnet-ef`

Configuration

1. Add a connection string named `DefaultConnection` in `TaskAPI/appsettings.json` or `TaskAPI/appsettings.Development.json`.

Example `appsettings.Development.json` snippet:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskManagerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Adjust the connection string for your SQL Server credentials and environment.

Build and run

From the repository root:

```
dotnet restore
dotnet build
dotnet run --project TaskAPI
```

When running in the Development environment Swagger UI will be available at `https://localhost:{port}/swagger`.

Database migrations

If you want to create and apply EF Core migrations (recommended):

```
cd TaskAPI
dotnet ef migrations add InitialCreate -o Data/Migrations
dotnet ef database update
```

(Depending on solution layout you may need to include `-s`/`-p` options to target the startup and migrations assembly.)

API Endpoints

All endpoints are under the base route: `api/task`.

- GET `api/task`
  - Returns all tasks.
  - Example: `curl https://localhost:5001/api/task`

- GET `api/task/{date}`
  - Returns tasks due on the provided date that are not completed. The route expects a date/time value (ISO 8601 recommended, e.g. `2026-01-01` or `2026-01-01T00:00:00`).
  - Example: `curl https://localhost:5001/api/task/2026-01-01`

- GET `api/task/count-by-type`
  - Returns a dictionary of task type ? count.
  - Example: `curl https://localhost:5001/api/task/count-by-type`

- POST `api/task`
  - Creates a new task. Provide a JSON `TaskObject` in the request body.
  - Example payload:

```
{
  "name": "Buy groceries",
  "description": "Milk, eggs, bread",
  "type": "Personal",
  "due_date": "2026-01-10T12:00:00",
  "completed": false
}
```

- PATCH `api/task`
  - Updates an existing task. The controller locates the task by `id` in the provided `TaskObject` and updates non-null fields.
  - Example payload (must include `id`):

```
{
  "id": 1,
  "completed": true
}
```

Notes and caveats

- The `CommonObjects` project contains the `TaskObject` model at `CommonObjects/Models/TaskObject.cs`.
- The `TaskAPI` project expects a `DefaultConnection` connection string.
- The API uses Swagger in Development mode; use it to explore endpoints and test requests.
