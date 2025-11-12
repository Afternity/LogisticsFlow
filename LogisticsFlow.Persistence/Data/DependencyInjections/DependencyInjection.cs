using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LogisticsFlow.Persistence.Data.DbContexts;

namespace LogisticsFlow.Persistence.Data.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
          this IServiceCollection services,
          string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string not provided");

            services.AddDbContext<LogisticsFlowDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                         maxRetryCount: 5,
                         maxRetryDelay: TimeSpan.FromSeconds(30),
                         errorNumbersToAdd: null);

                    sqlOptions.CommandTimeout(30);
                });

                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
