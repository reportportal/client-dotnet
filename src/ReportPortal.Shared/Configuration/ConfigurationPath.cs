using System;

namespace ReportPortal.Shared.Configuration
{
    /// <summary>
    /// Stores well known configuration property names.
    /// </summary>
    public static class ConfigurationPath
    {
        public static readonly string KeyDelimeter = ":";
        public static readonly string AppenderPrefix = "++";

        public static readonly string ServerUrl = $"Server{KeyDelimeter}Url";
        public static readonly string ServerProject = $"Server{KeyDelimeter}Project";
        [Obsolete("'Server:Authentication:Uuid' parameter is deprecated. Use 'Server:ApiKey' instead.")]
        public static readonly string ServerAuthenticationUuid = $"Server{KeyDelimeter}Authentication{KeyDelimeter}Uuid";
        public static readonly string ServerAuthenticationKey = $"Server{KeyDelimeter}ApiKey";

        public static readonly string LogsBatchCapacity = $"Server{KeyDelimeter}LogsBatchCapacity";
        public static readonly string AsyncReporting = $"Server{KeyDelimeter}AsyncReporting";

        public static readonly string LaunchName = $"Launch{KeyDelimeter}Name";
        public static readonly string LaunchDescription = $"Launch{KeyDelimeter}Description";
        public static readonly string LaunchDebugMode = $"Launch{KeyDelimeter}DebugMode";
        public static readonly string LaunchTags = $"Launch{KeyDelimeter}Tags";
    }
}
