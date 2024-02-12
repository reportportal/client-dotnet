using System;
using System.Collections.Generic;

namespace ReportPortal.Shared.Configuration
{
    /// <inheritdoc />
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        /// <inheritdoc />
        public IList<IConfigurationProvider> Providers { get; } = new List<IConfigurationProvider>();

        /// <inheritdoc />
        public IConfigurationBuilder Add(IConfigurationProvider provider)
        {
            Providers.Add(provider);

            return this;
        }

        /// <inheritdoc />
        public IConfiguration Build()
        {
            var properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (var provider in Providers)
            {
                var originalProperties = provider.Load();

                foreach (var property in originalProperties)
                {
                    if (property.Value.StartsWith(ConfigurationPath.AppenderPrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        if (properties.ContainsKey(property.Key))
                        {
                            properties[property.Key] += property.Value.Substring(ConfigurationPath.AppenderPrefix.Length);
                        }
                        else
                        {
                            properties[property.Key] = property.Value.Substring(ConfigurationPath.AppenderPrefix.Length);
                        }
                    }
                    else
                    {
                        properties[property.Key] = property.Value;
                    }
                }
            }

            return new Configuration(properties);
        }
    }
}
