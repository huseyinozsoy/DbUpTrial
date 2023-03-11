using DbUp;
using DbUpTrial.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DbUpTrial.Extension
{
    public static class DatabaseExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                logger.LogInformation("Migrating postresql database.");

                var connectionString = configuration.GetSection(nameof(DatabaseSettings))[nameof(DatabaseSettings.ConnectionString)];

                // this will ensure that the database is created
                EnsureDatabase.For.PostgresqlDatabase(connectionString);

                var upgrader = DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

                var result = upgrader.PerformUpgrade();

                if (!result.Successful)
                {
                    logger.LogError(result.Error, "An error occurred while migrating the postresql database");
                    return host;
                }

                logger.LogInformation("Migrated postresql database.");
            }

            return host;
        }
    }
}
