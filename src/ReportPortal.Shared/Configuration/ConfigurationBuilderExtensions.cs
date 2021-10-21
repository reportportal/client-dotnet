using ReportPortal.Shared.Configuration.Providers;
using System;
using System.IO;

namespace ReportPortal.Shared.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddDefaults(this IConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var baseDir = Environment.CurrentDirectory;

            return builder.AddDefaults(baseDir);
        }

        public static IConfigurationBuilder AddDefaults(this IConfigurationBuilder builder, string baseDir)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddJsonFile(Path.Combine(baseDir, "ReportPortal.config.json"), optional: true);
            builder.AddDirectoryProbing(baseDir);
            builder.AddEnvironmentVariables();

            return builder;
        }

        public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string filePath, bool optional = true)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.Add(new JsonFileConfigurationProvider(ConfigurationPath.KeyDelimeter, filePath, optional));
        }

        public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddEnvironmentVariables("RP_");
            builder.AddEnvironmentVariables("REPORTPORTAL_");

            return builder;
        }

        public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder builder, string prefix = "REPORTPORTAL_", string delimeter = "_")
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Add(new EnvironmentVariablesConfigurationProvider(prefix, delimeter, EnvironmentVariableTarget.Machine));

            builder.Add(new EnvironmentVariablesConfigurationProvider(prefix, delimeter, EnvironmentVariableTarget.User));

            builder.Add(new EnvironmentVariablesConfigurationProvider(prefix, delimeter, EnvironmentVariableTarget.Process));

            return builder;
        }

        public static IConfigurationBuilder AddDirectoryProbing(this IConfigurationBuilder builder, string directoryPath, string prefix = "ReportPortal", string delimiter = "_", bool optional = true)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Add(new DirectoryProbingConfigurationProvider(directoryPath, prefix, delimiter, optional));

            return builder;
        }
    }
}
