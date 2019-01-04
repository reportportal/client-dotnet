using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Shared.Configuration
{
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        public const string PREFIX = "REPORTPORTAL";
        public const string DELIMETER = "__";

        public IList<IConfigurationProvider> Providers { get; } = new List<IConfigurationProvider>();

        public IConfigurationBuilder Add(IConfigurationProvider provider)
        {
            Providers.Add(provider);

            return this;
        }

        public IConfiguration Build()
        {
            var values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach(var provider in Providers)
            {
                provider.Load();

                foreach(var value in provider.Properties)
                {
                    values[value.Key] = value.Value;
                }
            }

            return new Configuration(values);
        }
    }
}
