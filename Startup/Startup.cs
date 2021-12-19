using System;
using System.Net;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog.Extensions.Logging;
using Tipalti.APIDocumentation.Middlewares.Swagger;
using Tipalti.APIDocumentation.Swagger;
using Tipalti.Configuration.Startup;
using Tipalti.Configuration.Startup.Middlewares;
using Tipalti.ExceptionHandler;
using Tipalti.Telemetry;
using CrystalQuartz.AspNetCore;
using Quartz;
using Tipalti.Configuration.Quartz;
using Tipalti.Configuration.Quartz.CrystalQuartz;

namespace Tipalti.TheWho
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Hack to get ILogger as in .net core 3 no injections for this method or constructor are allowed (except config and host).
#pragma warning disable CA2000 // Dispose objects before losing scope
            ILogger logger = new SerilogLoggerProvider(StartupDefaults.CreateStartupLogger())
#pragma warning restore CA2000 // Dispose objects before losing scope
                                    .CreateLogger(nameof(Program));
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
            {
                builder.WithOrigins("http://localhost:5000/api/Products/v1").AllowAnyMethod().AllowAnyHeader();
            }));
            logger.LogInformation("Startup ConfigureServices");

            services.AddExceptionHandling(_configuration);

            services.AddTelemetry(_configuration, logger);

            services.AddHealthChecks(_configuration);

            services.AddRedis(_configuration, logger);

            DependencyInjectionConfigurator.Configure(services, _configuration, logger);

            services.SetAuthentication(_environment, logger, _configuration);

            services.AddControllers();

            services.AddApiVersioning();
            //Creates swagger end point i.e /swagger/index.html
            services.AddSwagger(_configuration,logger);

            AddQuartz(services, logger);


            logger.LogInformation("Startup ConfigureServices Done");
        }

        private void AddQuartz(IServiceCollection services, ILogger logger)
        {
            // Add the Quartz.net service for scheduling jobs

            logger.LogInformation("Startup ConfigureServices - Add Quartz");

            services.AddTipaltiQuartz(_configuration, Constants.QuartzSchedulerName, "TheWho");
            services.AddQuartzJobs();

            logger.LogInformation("Startup ConfigureServices - Add Quartz Done");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILogger<Startup> logger,
            IServiceProvider serviceProvider
            )
        {
            app.UseCors(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
            if (env is null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            logger.LogInformation("Startup Configure");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwaggerWithOptions(_configuration,env, serviceProvider);

            app.UseCorrelationId();

            app.UseTipaltiExceptionHandler();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecksUI();
                endpoints.MapControllers();
            });

            app.UseHealthChecks(StartupConstants.HealthEndpoint, new HealthCheckOptions()
            {
                Predicate = x => !x.Tags.Contains(StartupConstants.HealthCheckWarningTag),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecks(StartupConstants.HealthWarningsEndpoint, new HealthCheckOptions()
            {
                Predicate = x => x.Tags.Contains(StartupConstants.HealthCheckWarningTag),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UsePrometheusMetrics();

            app.UseTipaltiCrystalQuartz(logger, _configuration);
            logger.LogInformation("Startup Configure Done");
        }
    }
}
