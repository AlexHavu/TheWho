using Microsoft.Extensions.Configuration;
using Tipalti.Configuration.Startup;

namespace Tipalti.TheWho.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsDebugMode(this IConfiguration configuration) => configuration.GetValue<bool>(StartupConstants.DebugMode);
    }
}
