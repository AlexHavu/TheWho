using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tipalti.TheWho.Dal.Sql;


namespace Tipalti.TheWho
{
    public static class DependencyInjectionConfigurator
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            services.AddDbTheWhoRepository(configuration, logger);
            services.AddRedisTheWhoRepository();
        }
    }
}
