using System;

namespace ReportPortal.Shared.Configuration.Providers
{
    public static class JsonFileConfigurationProviderExtensions
    {
        public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string filePath)
        {
            return builder.Add(new JsonFileConfigurationProvider(ConfigurationPath.KEYDELIMETER, filePath));
        }
    }
}
