namespace ReportPortal.Shared.Configuration.Providers
{
    public static class EnvironmentVariablesConfigurationProviderExtensions
    {
        public const string PREFIX = "REPORTPORTAL";
        public const string DELIMETER = "__";

        public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder builder)
        {
            builder.Add(new EnvironmentVariablesConfigurationProvider(PREFIX, DELIMETER, System.EnvironmentVariableTarget.User));

            builder.Add(new EnvironmentVariablesConfigurationProvider(PREFIX, DELIMETER, System.EnvironmentVariableTarget.Process));

            return builder;
        }
    }
}
