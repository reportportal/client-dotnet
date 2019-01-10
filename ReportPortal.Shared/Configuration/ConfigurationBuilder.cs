using System;
using System.Collections.Generic;
using System.Text;

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
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach(var provider in Providers)
            {
                provider.Load();

                foreach(var value in provider.Properties)
                {
                    if (value.Value.StartsWith("+="))
                    {
                        if (values.ContainsKey(value.Key))
                        {
                            values[value.Key] += value.Value.Substring(2);
                        }
                        else
                        {
                            values[value.Key] = value.Value.Substring(2);
                        }
                    }
                    else
                    {
                        values[value.Key] = value.Value;
                    }
                }
            }

            return new Configuration(values);
        }
    }
}
