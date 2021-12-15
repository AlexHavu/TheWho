using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tipalti.Configuration.Startup;


namespace Tipalti.TheWho.Dal.Sql
{
    internal static class SqlServiceCollectionExtensions
    {
        public static void AddDbTheWhoRepository(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            var connection = configuration.GetConnectionString(StartupConstants.TipaltiConnectionString);
            var readOnlyConnection = configuration.GetConnectionString(StartupConstants.TipaltiReadOnlyConnectionString);

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (configuration[StartupConstants.DebugMode] == "true")
            {
                logger.LogInformation($"DB Connection string is {connection}");
                logger.LogInformation($"Read only DB Connection string is {readOnlyConnection}");
            }

            services.AddDbContext<DbTheWhoContext>(options => options.UseSqlServer(connection), ServiceLifetime.Scoped);
            services.AddDbContext<ReadOnlyDbTheWhoContext>(options => options.UseSqlServer(readOnlyConnection), ServiceLifetime.Scoped);
            services.AddScoped<IDbTheWhoContext>(serviceProvider => serviceProvider.GetService<DbTheWhoContext>());
            services.AddScoped<IReadOnlyDbTheWhoContext>(serviceProvider => serviceProvider.GetService<ReadOnlyDbTheWhoContext>());
            services.AddScoped<IDbTheWhoRepository, DbTheWhoRepository>();
        }
    }
}
