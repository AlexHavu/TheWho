using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Tipalti.Authentication.AspNetCore.Configuration;
using Tipalti.Configuration.Quartz.HealthChecks;
using Tipalti.Configuration.Startup;
using Tipalti.Configuration.Startup.HealthChecks;
using Tipalti.Configuration.Startup.SecurityExtensions;
using Tipalti.TheWho.Dal.Sql;

namespace Tipalti.TheWho
{
    public static class SiteConfiguration
    {
        public static IWebHostBuilder ConfigureSerilog(this IWebHostBuilder builder)
        {
            return builder.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                IConfiguration config = hostingContext.Configuration;
                var applicationName = hostingContext.HostingEnvironment.ApplicationName;
                StartupDefaults.ConfigureSerilog(loggerConfiguration, applicationName, config,null);
            });
         }
        public static IWebHostBuilder ConfigureAppsSettings(this IWebHostBuilder builder, ILogger logger)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            return builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                StartupDefaults.ConfigureEnvironmentVariables(config,logger, hostingContext.HostingEnvironment.EnvironmentName);
            });
        }
        public static void SetAuthentication(this IServiceCollection services, IWebHostEnvironment environment,Microsoft.Extensions.Logging.ILogger logger,IConfiguration config)
        {
            if (environment is null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            var jwtConfig = new JwtTokenConfiguration
            {
                Audience = environment.ApplicationName,
                EnvName = environment.EnvironmentName,
                jwtBearerEvents = new JwtBearerEvents
                {
                    OnAuthenticationFailed = TipaltiJwtEventsHelper.OnAuthenticationFailed,
                }
            };

            services.SetDefaultJwtSecurityHandling(config, jwtConfig, logger);
        }

        public static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new NullReferenceException(nameof(configuration));
            }

            services
                //.AddHealthChecksUI("healthChecksDB", x => x.AddHealthCheckEndpoint("am I healthy?", StartupConstants.HealthEndpoint))//Create health point UI i.e /healthchecks-ui#/healthchecks
                .AddHealthChecks()//Built in health checks (/health)
                .AddDbContextCheck<DbTheWhoContext>()
                .AddQuartzChecks(configuration, new string[] { StartupConstants.HealthCheckWarningTag })
                .AddRedis(redisConnectionString: configuration[StartupConstants.RedisConnectionString], name: "Redis health check", failureStatus: HealthStatus.Unhealthy, tags: new string[] { "redis" })
                .AddSerilogChecks();
        }

        public static void AddApiVersioning(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(
               options =>
               {
                   // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                   // note: the specified format code will format the version as "'v'major[.minor][-status]"
                   options.GroupNameFormat = "'v'VV";

                   // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                   // can also be used to control the format of the API version in route templates
                   options.SubstituteApiVersionInUrl = true;
               });

            services.AddApiVersioning(
               options =>
               {
                   // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                   options.ReportApiVersions = true;
               });

        }

    }
}
