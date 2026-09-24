using VideoGame.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Polly;
//using VideoGame.Data.Repositories.Interfaces;
//using VideoGame.Data.Repositories.Implementations;
//using VideoGame.Services.Implementations;
//using VideoGame.Services.Interfaces;
using VideoGame.Extensions;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. SERVICES CONFIGURATION (DEPENDENCY INJECTION POOL)
// =========================================================================

// Add controller framework infrastructure
builder.Services.AddControllers();

// Configure EF Core targeting local SQL Express Instance via LocalDB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Dependency Injection
builder.Services.AddApplicationInfrastructure();

// Configure Relaxed CORS policy allowing local Angular server (port 4200) full pipeline access
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Configure Resilient HTTP client factory pattern for outbound communication
builder.Services.AddHttpClient("ExternalGameMetadataClient", client =>
{
    client.BaseAddress = new Uri("https://example-metadata-service.com");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddStandardResilienceHandler(options =>
{
    // Define robust timeout strategies
    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(10);
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(3);

    // Define progressive retry parameters for handling transient drop blips
    options.Retry.MaxRetryAttempts = 3;
    options.Retry.Delay = TimeSpan.FromMilliseconds(200);
    options.Retry.BackoffType = DelayBackoffType.Exponential;
});

// Build out logging capabilities explicitly (Console provider maps natively)
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

// =========================================================================
// 2. MIDDLEWARE CONFIGURATION (HTTP REQUEST PIPELINE)
// =========================================================================

var app = builder.Build();

// Activate cross-origin configurations
app.UseCors("AllowAngular");

// Handle routing patterns natively matching controller decorations
app.MapControllers();

// =========================================================================
// 3. DATABASE SEED AND MIGRATION RUNNER UTILITY
// =========================================================================

// Use a scoped provider instance to safely handle and execute initial database structures
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        logger.LogInformation("Verifying local SQL Server connection state and checking database status...");

        // This line runs your EF Core migrations on engine bootup automatically,
        // removing the need to execute manual bash migration updates across host systems.
        dbContext.Database.Migrate();

        logger.LogInformation("Database structure initialized and records successfully seeded to MSSQL platform.");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "An unhandled execution error disrupted initial system deployment mapping.");
    }
}

// Fire up web engine instance listening to local ports (typically port 5000/5001)
app.Run();