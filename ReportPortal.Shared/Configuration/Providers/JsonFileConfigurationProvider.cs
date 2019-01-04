using System;
using System.Collections.Generic;
using System.Text;

namespace ReportPortal.Shared.Configuration.Providers
{
    public class JsonFileConfigurationProvider : IConfigurationProvider
    {
        public IDictionary<string, string> Properties => throw new NotImplementedException();

        public IDictionary<string, string> Load()
        {
            throw new NotImplementedException();
        }
    }
}
