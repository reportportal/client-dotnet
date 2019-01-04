using System.Collections.Generic;

namespace ReportPortal.Shared.Configuration
{
    public interface IConfigurationBuilder
    {
        IList<IConfigurationProvider> Providers { get; }

        IConfigurationBuilder Add(IConfigurationProvider provider);

        IConfiguration Build();
    }
}
