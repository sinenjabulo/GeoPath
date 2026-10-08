using GeoPath.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<PlatformStatusService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapGet("/api/health", (PlatformStatusService statusService) => statusService.GetStatus());
app.MapGet("/api/architecture", () => new
{
    applicationName = "GeoPath",
    phase = "Foundation and Architecture",
    status = "In progress",
    stack = new[] { "Angular", "C#/.NET", "PostgreSQL" },
    sprint = "Sprint 1"
});

app.Run();
