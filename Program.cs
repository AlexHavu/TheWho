using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Tipalti.Configuration.Startup;

namespace Tipalti.TheWho
{
    public class Program
    {
        /// <summary>
        /// Loading configuration before, as we need to decide some configurations during the startup stage
        /// Like pass Redis connection string for serilog, debug mode etc..
        /// </summary>
        public static IConfiguration Configuration { get; } = StartupDefaults.LoadStartupConfiguration();

        public static void Main(string[] args)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            StreamWriter file = File.CreateText("Serilog.txt");
#pragma warning restore CA2000 // Dispose objects before losing scope
            Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));

            if (Configuration[StartupConstants.DebugMode] == "true")
            {
                using Serilog.Core.Logger log = StartupDefaults.CreateStartupLogger();
                StartupDefaults.LogEnvironmentVariables(log);
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            Serilog.Core.Logger log = StartupDefaults.CreateStartupLogger();
#pragma warning restore CA2000 // Dispose objects before losing scope

            IHostBuilder host = Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder
                            .ConfigureAppsSettings(log)
                            .UseStartup<Startup>()
                            .ConfigureSerilog();
                    });
            return host;
        }
    }
}
