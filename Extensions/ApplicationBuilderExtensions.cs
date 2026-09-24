using VideoGame.Data;
using VideoGame.Middleware;
using Microsoft.EntityFrameworkCore;

namespace VideoGame.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionMiddleware>();
    }

    public static async Task InitializeDatabaseAsync(
        this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var services = scope.ServiceProvider;

        var logger = services
            .GetRequiredService<ILogger<ApplicationDbContext>>();

        var dbContext = services
            .GetRequiredService<ApplicationDbContext>();

        try
        {
            logger.LogInformation(
                "Verifying database connection and applying migrations...");

            await dbContext.Database.MigrateAsync();

            logger.LogInformation(
                "Database initialized successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(
                ex,
                "Database initialization failed.");

            throw;
        }
    }
}
