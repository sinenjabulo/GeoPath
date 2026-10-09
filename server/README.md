# GeoPath backend

The backend uses PostgreSQL as the durable store for users and roles, geometry content, classes and enrollments, assignments, and learner attempts with stage outcomes. Passwords are stored only as password hashes. JWT access tokens are validated by the API and are not stored in PostgreSQL. The Angular client is intentionally deferred until the backend API is stable.

## Database configuration

Configure the connection string with either:

- `ConnectionStrings:DefaultConnection` in local configuration, or
- the `GEOPATH_DB_CONNECTION` environment variable.

Do not commit database passwords or production credentials.

## Apply schema migrations

From the repository root, restore the repository-local EF Core tool and apply migrations:

```bash
dotnet tool restore
dotnet tool run dotnet-ef database update \
  --project server/src/GeoPath.Infrastructure/GeoPath.Infrastructure.csproj \
  --startup-project server/src/GeoPath.Api/GeoPath.Api.csproj
```

Migrations create the initial relational schema and seed role definitions and representative public circle-geometry content.

## Run the API

After configuring PostgreSQL and applying migrations:

```bash
dotnet run --project server/src/GeoPath.Api
```

The API provides public read endpoints for examples, theorems, and definitions, plus authenticated learner endpoints for creating and retrieving attempts and recording thinking-stage outcomes. Learner identity is taken from the validated JWT; attempt routes do not accept a learner ID from the caller.

## API exploration

When running in the Development environment, open `http://localhost:5229/swagger` to browse and try the API endpoints. The underlying OpenAPI document is available at `http://localhost:5229/openapi/v1.json`. Swagger UI is disabled outside Development.

## Initial data model

- Accounts and role definitions
- Topics, examples, theorems, definitions, and their content links
- Teacher-owned classes, learner enrollments, and class assignments
- Learner attempts and one result per thinking stage

Mastery is not stored as an unqualified score; it can be derived from attempt history after the product defines its calculation.
