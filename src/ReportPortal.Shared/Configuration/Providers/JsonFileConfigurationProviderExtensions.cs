using System;

namespace ReportPortal.Shared.Configuration.Providers
{
    public static class JsonFileConfigurationProviderExtensions
    {
        public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string filePath, bool optional = true)
        {
            return builder.Add(new JsonFileConfigurationProvider(ConfigurationPath.KeyDelimeter, filePath, optional));
        }
    }
}
