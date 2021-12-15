using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tipalti.Configuration.Startup;
using Tipalti.TheWho.Dal.Redis;

using Tipalti.TheWho.Dal.Sql;
using Tipalti.Redis;

namespace Tipalti.TheWho
{
    public static class ServiceCollectionExtensions
    {
        
        public static void AddRedisTheWhoRepository(this IServiceCollection services)
        {
            services.AddScoped<IRedisTheWhoRepository>(serviceProvider =>
                new RedisTheWhoRepository(serviceProvider.GetService<IRedisConnectionProvider>()));
        }
    }
}
