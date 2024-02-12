using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Extensibility.Embedded.Analytics
{
    // TODO: Makethis class testable, or even create GlobalConfiguration
    internal static class ClientIdProvider
    {
        public static readonly string FILE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".rp", "rp.properties");
        private const string CLIENT_ID_KEY = "client.id";

        /// <summary>
        /// Asynchronously gets the client ID from the properties file. 
        /// If the file does not exist or the client ID is not found, a new ID is generated and saved to the file.
        /// </summary>
        /// <returns>The client ID as a string.</returns>
        public static async Task<string> GetClientIdAsync()
        {
            string clientId = await ReadClientIdAsync();

            if (string.IsNullOrEmpty(clientId))
            {
                clientId = Guid.NewGuid().ToString();
                await SaveClientIdAsync(clientId);
            }

            return clientId;
        }

        private static async Task<string> ReadClientIdAsync()
        {
            if (File.Exists(FILE_PATH))
            {
                using (var reader = new StreamReader(FILE_PATH))
                {
                    var contents = await reader.ReadToEndAsync();
                    var matches = new Regex($@"{CLIENT_ID_KEY}\s*=\s*(\S*)").Matches(contents);
                    if (matches.Count > 0)
                    {
                        return matches[0].Groups[1].Value.Trim();
                    }
                }
            }

            return null;
        }

        private static async Task SaveClientIdAsync(string clientId)
        {
            StringBuilder contents = new StringBuilder();
            if (File.Exists(FILE_PATH))
            {
                using (var reader = new StreamReader(FILE_PATH))
                {
                    contents.Append(await reader.ReadToEndAsync());
                    if (contents.Length > 0 && !contents.ToString().EndsWith("\n"))
                    {
                        contents.Append("\n");
                    }
                }
            }
            contents.Append($"{CLIENT_ID_KEY} = {clientId}\n");

            Directory.CreateDirectory(Path.GetDirectoryName(FILE_PATH)); // Ensure the directory exists
            using (var writer = new StreamWriter(FILE_PATH))
            {
                await writer.WriteAsync(contents.ToString());
            }
        }
    }
}
