using System.Collections.Generic;

namespace ReportPortal.Shared.Configuration
{
    public interface IConfigurationProvider
    {
        IDictionary<string, string> Properties { get; }

        IDictionary<string, string> Load();
    }
}
