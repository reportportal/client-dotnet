using ReportPortal.Shared.Configuration.Providers;
using System;
using System.IO;

namespace ReportPortal.Shared.Configuration
{
    /// <summary>
    /// Provides extension methods for IConfigurationBuilder.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds default configuration sources to the configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <returns>The configuration builder.</returns>
        public static IConfigurationBuilder AddDefaults(this IConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var baseDir = Environment.CurrentDirectory;

            return builder.AddDefaults(baseDir);
        }

        /// <summary>
        /// Adds default configuration sources to the configuration builder with a specified base directory.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="baseDir">The base directory.</param>
        /// <returns>The configuration builder.</returns>
        public static IConfigurationBuilder AddDefaults(this IConfigurationBuilder builder, string baseDir)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddJsonFile(Path.Combine(baseDir, "ReportPortal.json"), optional: true);
            builder.AddJsonFile(Path.Combine(baseDir, "ReportPortal.config.json"), optional: true);
            builder.AddDirectoryProbing(baseDir);
            builder.AddEnvironmentVariables();

            return builder;
        }

        /// <summary>
        /// Adds a JSON file as a configuration source to the configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <param name="optional">A flag indicating whether the file is optional.</param>
        /// <returns>The configuration builder.</returns>
        public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string filePath, bool optional = true)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.Add(new JsonFileConfigurationProvider(ConfigurationPath.KeyDelimeter, filePath, optional));
        }

        /// <summary>
        /// Adds environment variables as a configuration source to the configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <returns>The configuration builder.</returns>
        public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddEnvironmentVariables("RP_", "_");
            builder.AddEnvironmentVariables("RP__", "__");

            builder.AddEnvironmentVariables("REPORTPORTAL_", "_");
            builder.AddEnvironmentVariables("REPORTPORTAL__", "__");

            return builder;
        }

        /// <summary>
        /// Adds environment variables with a specified prefix and delimiter as a configuration source to the configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="prefix">The prefix for the environment variables.</param>
        /// <param name="delimiter">The delimiter for the environment variables.</param>
        /// <returns>The configuration builder.</returns>
        public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder builder, string prefix = "REPORTPORTAL_", string delimiter = "_")
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Add(new EnvironmentVariablesConfigurationProvider(prefix, delimiter, EnvironmentVariableTarget.Machine));

            builder.Add(new EnvironmentVariablesConfigurationProvider(prefix, delimiter, EnvironmentVariableTarget.User));

            builder.Add(new EnvironmentVariablesConfigurationProvider(prefix, delimiter, EnvironmentVariableTarget.Process));

            return builder;
        }

        /// <summary>
        /// Adds a directory probing configuration provider to the configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder.</param>
        /// <param name="directoryPath">The path to the directory.</param>
        /// <param name="prefix">The prefix for the configuration keys.</param>
        /// <param name="delimiter">The delimiter for the configuration keys.</param>
        /// <param name="optional">A flag indicating whether the directory is optional.</param>
        /// <returns>The configuration builder.</returns>
        public static IConfigurationBuilder AddDirectoryProbing(this IConfigurationBuilder builder, string directoryPath, string prefix = "ReportPortal", string delimiter = "_", bool optional = true)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Add(new DirectoryProbingConfigurationProvider(directoryPath, prefix, delimiter, optional));

            return builder;
        }
    }
}
