namespace ReportPortal.Shared.Configuration.Providers
{
    public static class EnvironmentVariablesConfigurationProviderExtensions
    {
        public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder builder)
        {
            return builder.Add(new EnvironmentVariablesConfigurationProvider("REPORTPORTAL", "__"));
        }
    }
}
