using ReportPortal.Shared.Configuration.Providers;
using System;
using System.IO;

namespace ReportPortal.Shared.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds default configuration to the specified <see cref="IConfigurationBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add default configuration to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/> with default configuration added.</returns>
        public static IConfigurationBuilder AddDefaults(this IConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var baseDir = AppContext.BaseDirectory;

            return builder.AddDefaults(baseDir);
        }

        /// <summary>
        /// Adds default configuration sources to the <see cref="IConfigurationBuilder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add the default configuration sources to.</param>
        /// <param name="baseDir">The base directory path.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/> with the default configuration sources added.</returns>
        public static IConfigurationBuilder AddDefaults(this IConfigurationBuilder builder, string baseDir)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddJsonFile(Path.Combine(baseDir, "ReportPortal.json"), optional: true);
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

            builder.AddEnvironmentVariables("RP_", "_");
            builder.AddEnvironmentVariables("RP__", "__");

            builder.AddEnvironmentVariables("REPORTPORTAL_", "_");
            builder.AddEnvironmentVariables("REPORTPORTAL__", "__");

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
