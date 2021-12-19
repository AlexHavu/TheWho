using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tipalti.TheWho.Dal.Sql;
using Tipalti.TheWho.Indexers;

namespace Tipalti.TheWho
{
    public static class DependencyInjectionConfigurator
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            services.AddDbTheWhoRepository(configuration, logger);
            services.AddRedisTheWhoRepository();
            services.AddConfluenceHttpClient(configuration, logger);
            services.AddScoped<IConfluenceIndexer, ConfluenceIndexer>();
            services.AddScoped<IServiceIndexer, ServiceIndexer>();
            services.AddScoped<ITeamIndexer, TeamIndexer>();
        }
    }
}
