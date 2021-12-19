using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tipalti.Configuration.Startup;
using Tipalti.TheWho.Dal.Redis;

using Tipalti.TheWho.Dal.Sql;
using Tipalti.Redis;
using Tipalti.TheWho.Models.Confluence;
using System.Collections.Generic;
using Polly.Contrib.WaitAndRetry;
using Tipalti.TheWho.Dal.Confluence;
using Polly;

namespace Tipalti.TheWho
{
    public static class ServiceCollectionExtensions
    {
        
        public static void AddRedisTheWhoRepository(this IServiceCollection services)
        {
            services.AddScoped<IRedisTheWhoRepository>(serviceProvider =>
                new RedisTheWhoRepository(serviceProvider.GetService<IRedisConnectionProvider>()));
        }

        public static void AddConfluenceHttpClient(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            var confluenceConfiguration = new ConfluenceConfiguration();
            configuration.Bind(ConfluenceConfiguration.Provider, confluenceConfiguration);
            services.AddSingleton(confluenceConfiguration);

            IEnumerable<TimeSpan> delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: confluenceConfiguration.HttpClientRetryCount);
            services.AddHttpClient<IConfluenceRepository, ConfluenceRepository>()
            .AddTransientHttpErrorPolicy(p =>
            {
                return p.WaitAndRetryAsync(sleepDurations: delay,
                    onRetry: (x, i, c, t) => logger.LogWarning($"Request failed {x.Result}", x.Exception)
                );
            });
        }
    }
}
