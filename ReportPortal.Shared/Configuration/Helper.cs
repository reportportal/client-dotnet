using System;
using System.Text.RegularExpressions;

namespace ReportPortal.Shared.Configuration
{
    public class Helper
    {
        public static T GetEnvironmentProperty<T>(string parameter, object parameterValue)
        {
            var envMatch = Regex.Match(parameterValue.ToString(), "{env:(.*)}");
            if (envMatch.Success)
            {
                var envPropertyName = envMatch.Groups[1].Value;

                var envValue = Environment.GetEnvironmentVariable(envPropertyName);

                if (string.IsNullOrEmpty(envValue))
                {
                    envValue = Environment.GetEnvironmentVariable(envPropertyName, EnvironmentVariableTarget.User);

                    if (string.IsNullOrEmpty(envValue))
                    {
                        throw new ArgumentNullException(nameof(parameter), $"'{envPropertyName}' environment variable not found.");
                    }
                }

                if (typeof(T) == typeof(Uri))
                {
                    return (T)(object)new Uri(envValue);
                }
                else {
                    return (T)Convert.ChangeType(envValue, typeof(T));
                }
            }

            return (T)Convert.ChangeType(parameter, typeof(T));
        }
    }
}
