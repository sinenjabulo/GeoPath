using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using GeoPath.Api;
using GeoPath.Application;
using GeoPath.Domain;
using GeoPath.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Registers OpenAPI support and serializes roles as readable enum names in JSON.
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Defines the role requirements used by protected teacher and admin endpoints.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireLearner", policy => policy.RequireRole("Learner"));
    options.AddPolicy("RequireTeacherOrAdmin", policy => policy.RequireRole("Teacher", "Admin"));
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("GeoPathClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Configures JWT validation using the same issuer, audience, and signing key as token creation.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secret = builder.Configuration["Authentication:JwtSecret"] ?? "GeoPath-Development-Secret-Replace-In-Production";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:JwtIssuer"] ?? "GeoPath",
            ValidAudience = builder.Configuration["Authentication:JwtAudience"] ?? "GeoPath",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.FromMinutes(1),
        };
    });

// Registers the platform status service and authentication dependencies.
builder.Services.AddSingleton<PlatformStatusService>();
builder.Services.AddSingleton<IAuthenticationTokenService, JwtTokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<GeometryContentService>();
builder.Services.AddScoped<LearningAttemptService>();
builder.Services.AddGeoPathInfrastructure(builder.Configuration);

var app = builder.Build();

// Publishes the OpenAPI document during local development.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "GeoPath API v1");
    });
}

// Validates bearer tokens before applying endpoint authorization policies.
app.UseCors("GeoPathClient");
app.UseAuthentication();
app.UseAuthorization();

// Exposes public health information and the current sprint's architecture status.
app.MapGet("/api/health", (PlatformStatusService statusService) => statusService.GetStatus());
app.MapGet("/api/architecture", () => new
{
    applicationName = "GeoPath",
    phase = "Learner onboarding and dashboard",
    status = "In progress",
    stack = new[] { "Angular", "C#/.NET", "PostgreSQL" },
    sprint = "Sprint 5"
});

// Registers an account and returns its public profile and access token.
app.MapPost("/api/auth/register", async (RegisterRequest request, AuthService authService) =>
{
    try
    {
        var result = await authService.RegisterAsync(request);
        app.Logger.LogInformation("New user registered: {UserId} ({Email})", result.User.Id, result.User.Email);
        return Results.Ok(new AuthResponse(
            AuthService.ToSummary(result.User),
            result.Token,
            result.ExpiresAtUtc));
    }
    catch (InvalidOperationException exception)
    {
        app.Logger.LogWarning("Registration failed for {Email}: {Message}", request.Email, exception.Message);
        return Results.BadRequest(new { message = exception.Message });
    }
});

// Verifies credentials and returns the user's public profile and access token.
app.MapPost("/api/auth/login", async (LoginRequest request, AuthService authService) =>
{
    try
    {
        var result = await authService.LoginAsync(request);
        app.Logger.LogInformation("User logged in: {UserId} ({Email})", result.User.Id, result.User.Email);
        return Results.Ok(new AuthResponse(
            AuthService.ToSummary(result.User),
            result.Token,
            result.ExpiresAtUtc));
    }
    catch (InvalidOperationException exception)
    {
        return Results.BadRequest(new { message = exception.Message });
    }
});

// Returns identity claims from the authenticated user's validated token.
app.MapGet("/api/auth/me", (ClaimsPrincipal user) =>
{
    var identity = new
    {
        id = user.FindFirstValue(ClaimTypes.NameIdentifier),
        name = user.FindFirstValue(ClaimTypes.Name),
        email = user.FindFirstValue(ClaimTypes.Email),
        role = user.FindFirstValue(ClaimTypes.Role),
    };

    return Results.Ok(identity);
}).RequireAuthorization();

// Demonstrates endpoints protected by teacher-or-admin and admin-only policies.
app.MapGet("/api/auth/teacher-only", () => Results.Ok(new { message = "Teacher-level access granted." }))
    .RequireAuthorization("RequireTeacherOrAdmin");
app.MapGet("/api/auth/admin-only", () => Results.Ok(new { message = "Admin-level access granted." }))
    .RequireAuthorization("RequireAdmin");

app.MapGet("/api/classes", async (
    ClaimsPrincipal principal,
    GeoPathDbContext dbContext,
    CancellationToken cancellationToken) =>
{
    if (!TryGetAuthenticatedUserId(principal, out var learnerId))
    {
        return Results.Unauthorized();
    }

    var classes = await dbContext.ClassEnrollments.AsNoTracking()
        .Where(enrollment => enrollment.LearnerId == learnerId && enrollment.Status == EnrollmentStatus.Active)
        .Join(
            dbContext.Classrooms.AsNoTracking(),
            enrollment => enrollment.ClassroomId,
            classroom => classroom.Id,
            (enrollment, classroom) => new
            {
                id = classroom.Id,
                name = classroom.Name,
                joinedAtUtc = enrollment.EnrolledAtUtc,
            })
        .OrderByDescending(entry => entry.joinedAtUtc)
        .ToListAsync(cancellationToken);

    return Results.Ok(classes);
}).RequireAuthorization("RequireLearner");

app.MapPost("/api/classes/join", async (
    JoinClassRequest request,
    ClaimsPrincipal principal,
    GeoPathDbContext dbContext,
    CancellationToken cancellationToken) =>
{
    if (!TryGetAuthenticatedUserId(principal, out var learnerId))
    {
        return Results.Unauthorized();
    }

    var joinCode = (request.JoinCode ?? request.Code ?? string.Empty).Trim();
    if (string.IsNullOrWhiteSpace(joinCode))
    {
        return Results.BadRequest(new { message = "A class join code is required." });
    }

    var normalizedJoinCode = GeoPathProgramSupport.NormalizeJoinCode(joinCode);
    var classroom = await dbContext.Classrooms.AsNoTracking()
        .SingleOrDefaultAsync(candidate => candidate.JoinCodeHash == GeoPathProgramSupport.HashJoinCode(normalizedJoinCode), cancellationToken);

    if (classroom is null)
    {
        return Results.BadRequest(new { message = "Invalid class join code." });
    }

    var existingEnrollment = await dbContext.ClassEnrollments.AsNoTracking()
        .AnyAsync(
            enrollment =>
                enrollment.ClassroomId == classroom.Id
                && enrollment.LearnerId == learnerId
                && enrollment.Status == EnrollmentStatus.Active,
            cancellationToken);

    if (existingEnrollment)
    {
        return Results.Ok(new
        {
            message = "You are already enrolled in this class.",
            @class = new { id = classroom.Id, name = classroom.Name },
        });
    }

    dbContext.ClassEnrollments.Add(new ClassEnrollment
    {
        ClassroomId = classroom.Id,
        LearnerId = learnerId,
        Status = EnrollmentStatus.Active,
        EnrolledAtUtc = DateTimeOffset.UtcNow,
    });

    await dbContext.SaveChangesAsync(cancellationToken);

    return Results.Ok(new
    {
        message = "Class joined successfully.",
        @class = new { id = classroom.Id, name = classroom.Name },
    });
}).RequireAuthorization("RequireLearner");

// Returns only published geometry examples and their topic summaries.
app.MapGet("/api/examples", async (GeometryContentService contentService, CancellationToken cancellationToken) =>
    Results.Ok(await contentService.GetExamplesAsync(cancellationToken)));
app.MapGet("/api/examples/{id:guid}", async (
    Guid id,
    GeometryContentService contentService,
    CancellationToken cancellationToken) =>
{
    var example = await contentService.GetExampleAsync(id, cancellationToken);
    return example is null ? Results.NotFound() : Results.Ok(example);
});

// Returns published theorem and definition library entries.
app.MapGet("/api/theorems", async (GeometryContentService contentService, CancellationToken cancellationToken) =>
    Results.Ok(await contentService.GetTheoremsAsync(cancellationToken)));
app.MapGet("/api/definitions", async (GeometryContentService contentService, CancellationToken cancellationToken) =>
    Results.Ok(await contentService.GetDefinitionsAsync(cancellationToken)));

// Creates and retrieves attempts scoped to the authenticated learner.
app.MapPost("/api/attempts", async (
    CreateAttemptRequest request,
    ClaimsPrincipal principal,
    LearningAttemptService attemptService,
    CancellationToken cancellationToken) =>
{
    if (!TryGetAuthenticatedUserId(principal, out var learnerId))
    {
        return Results.Unauthorized();
    }

    try
    {
        var attempt = await attemptService.CreateAsync(learnerId, request.ExampleId, cancellationToken);
        return Results.Created($"/api/attempts/{attempt.Id}", LearningAttemptService.ToResponse(attempt));
    }
    catch (InvalidOperationException exception)
    {
        return Results.BadRequest(new { message = exception.Message });
    }
}).RequireAuthorization("RequireLearner");

app.MapGet("/api/attempts", async (
    ClaimsPrincipal principal,
    LearningAttemptService attemptService,
    CancellationToken cancellationToken) =>
{
    if (!TryGetAuthenticatedUserId(principal, out var learnerId))
    {
        return Results.Unauthorized();
    }

    var attempts = await attemptService.GetForLearnerAsync(learnerId, cancellationToken);
    return Results.Ok(attempts.Select(LearningAttemptService.ToResponse));
}).RequireAuthorization("RequireLearner");

app.MapGet("/api/attempts/{id:guid}", async (
    Guid id,
    ClaimsPrincipal principal,
    LearningAttemptService attemptService,
    CancellationToken cancellationToken) =>
{
    if (!TryGetAuthenticatedUserId(principal, out var learnerId))
    {
        return Results.Unauthorized();
    }

    var attempt = await attemptService.GetForLearnerAsync(id, learnerId, cancellationToken);
    return attempt is null
        ? Results.NotFound()
        : Results.Ok(LearningAttemptService.ToResponse(attempt));
}).RequireAuthorization("RequireLearner");

app.MapPut("/api/attempts/{id:guid}/stages/{stage}", async (
    Guid id,
    string stage,
    RecordStageResultRequest request,
    ClaimsPrincipal principal,
    LearningAttemptService attemptService,
    CancellationToken cancellationToken) =>
{
    if (!Enum.TryParse<AttemptStage>(stage, ignoreCase: true, out var parsedStage)
        || !Enum.IsDefined(parsedStage))
    {
        return Results.BadRequest(new { message = "Unsupported attempt stage." });
    }

    if (!TryGetAuthenticatedUserId(principal, out var learnerId))
    {
        return Results.Unauthorized();
    }

    try
    {
        var attempt = await attemptService.RecordStageResultAsync(
            id,
            learnerId,
            parsedStage,
            request.Outcome,
            request.ResponseJson,
            cancellationToken);
        return attempt is null
            ? Results.NotFound()
            : Results.Ok(LearningAttemptService.ToResponse(attempt));
    }
    catch (InvalidOperationException exception)
    {
        return Results.BadRequest(new { message = exception.Message });
    }
}).RequireAuthorization("RequireLearner");

// Extracts the authenticated account identifier from a validated JWT principal.
static bool TryGetAuthenticatedUserId(ClaimsPrincipal principal, out Guid userId)
{
    return Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GeoPathDbContext>();
    var hasDemoClassroom = await GeoPathProgramSupport.EnsureDemoClassroomAsync(dbContext);
    if (!hasDemoClassroom)
    {
        app.Logger.LogWarning(
            "The demo classroom was not created because no teacher account exists. Create a teacher account before using the demo class join code.");
    }
}

// Starts the API host and begins accepting requests.
app.Run();
