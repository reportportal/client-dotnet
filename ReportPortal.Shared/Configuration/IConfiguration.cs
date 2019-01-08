using System.Collections.Generic;

namespace ReportPortal.Shared.Configuration
{
    public interface IConfiguration
    {
        IDictionary<string, object> Values { get; }

        T GetValue<T>(string variable);

        T GetValue<T>(string variable, T defaultValue);

        IEnumerable<T> GetValues<T>(string variable);

        IEnumerable<T> GetValues<T>(string variable, IEnumerable<T> defaultValue);
    }
}
