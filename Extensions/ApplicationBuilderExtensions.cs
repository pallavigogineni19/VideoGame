using Microsoft.EntityFrameworkCore;
using VideoGame.Data;

namespace VideoGame.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task InitializeDatabaseAsync(
        this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
        var dbContext = services.GetRequiredService<ApplicationDbContext>();

        try
        {
            logger.LogInformation(
                "Verifying database connection and applying migrations...");

            await dbContext.Database.MigrateAsync();

            logger.LogInformation(
                "Database structure initialized successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(
                ex,
                "An error occurred while initializing the database.");

            throw;
        }
    }
}




