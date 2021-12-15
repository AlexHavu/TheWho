using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tipalti.APIDocumentation.Configuration;
using Tipalti.Configuration.Startup;
using Tipalti.ExceptionHandler;
using Tipalti.ExceptionHandler.Exceptions;
using Tipalti.ExceptionHandler.Models.Dtos;
using Tipalti.TheWho.Extensions;
using Tipalti.Redis;
using Tipalti.TheWho.Jobs;

namespace Tipalti.TheWho
{
    public static class GlobalDependencyInjection
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            var redisConnectionString = configuration.GetValue<string>(StartupConstants.RedisConnectionString);

            if (configuration[StartupConstants.DebugMode] == "true")
            {
                logger.LogInformation($"Redis Connection string is {redisConnectionString} ");
            }

            services.AddSingleton<IRedisConnectionProvider>(serviceProvider => new RedisConnectionProvider(redisConnectionString));
        }

        public static void AddExceptionHandling(this IServiceCollection services, IConfiguration configuration) => services
            .AddTipaltiExceptionHandler(options => options
                .SetDebugMode(configuration.IsDebugMode())
                .AddHandler<UserInputException>(e => new ExceptionMapping
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = e.ErrorCode
                })
            );



        /// <summary>
        /// Use this method to register your quartz jobs.
        /// </summary>
        public static void AddQuartzJobs(this IServiceCollection services)
        {
            services.AddTransient<SampleJob>();
        }
    }
}

