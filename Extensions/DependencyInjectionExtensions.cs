
// Extensions/DependencyInjectionExtensions.cs
using Microsoft.Extensions.DependencyInjection;
using VideoGame.Data.Repositories.Interfaces;
using VideoGame.Data.Repositories.Implementations;
using VideoGame.Services.Interfaces;
using VideoGame.Services.Implementations;

namespace VideoGame.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationInfrastructure(this IServiceCollection services)
        {
            // 1. Data Access Layer / Repositories Segment
            services.AddScoped<IVideoGameReadOnlyRepository, VideoGameReadOnlyRepository>();
            services.AddScoped<IVideoGameWriteOnlyRepository, VideoGameWriteOnlyRepository>();

            // 2. Business Logic Layer / Services Segment
            services.AddScoped<IVideoGameQueryService, VideoGameQueryService>();
            services.AddScoped<IVideoGameCommandService, VideoGameCommandService>();

            return services;
        }
    }
}
