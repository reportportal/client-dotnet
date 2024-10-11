using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Internal.Logging;
using ReportPortal.Shared.MimeTypes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ReportPortal.Shared.Extensibility.Embedded.LaunchArtifacts
{
    public class LaunchArtifactsEventsObserver : IReportEventsObserver
    {
        private static readonly ITraceLogger _logger = TraceLogManager.Instance.GetLogger(typeof(LaunchArtifactsEventsObserver));

        public string BaseDirectory { get; set; } = Environment.CurrentDirectory;

        public void Initialize(IReportEventsSource reportEventsSource)
        {
            reportEventsSource.OnBeforeLaunchFinishing += ReportEventsSource_OnBeforeLaunchFinishing;
        }

        private void ReportEventsSource_OnBeforeLaunchFinishing(Reporter.ILaunchReporter launchReporter, ReportEvents.EventArgs.BeforeLaunchFinishingEventArgs args)
        {
            var artifactPaths = args.Configuration.GetValues<string>("Launch:Artifacts", null);

            if (artifactPaths != null)
            {
                foreach (var filePattern in artifactPaths)
                {
                    var artifacts = Directory.GetFiles(BaseDirectory, filePattern);

                    foreach (var artifact in artifacts)
                    {
                        var createLogItemRequest = new CreateLogItemRequest
                        {
                            LaunchUuid = launchReporter.Info.Uuid,
                            Time = DateTime.UtcNow,
                            Level = Client.Abstractions.Models.LogLevel.Trace,
                            Text = Path.GetFileName(artifact),
                        };

                        AttachFile(artifact, ref createLogItemRequest);

                        Task.Run(async () => await args.ClientService.LogItem.CreateAsync(createLogItemRequest)).GetAwaiter().GetResult();
                    }
                }
            }
        }

        private static void AttachFile(string filePath, ref CreateLogItemRequest request)
        {
            try
            {
                using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        var bytes = memoryStream.ToArray();

                        request.Attach = new LogItemAttach
                        {
                            Name = Path.GetFileName(filePath),
                            MimeType = MimeTypeMap.GetMimeType(Path.GetExtension(filePath)),
                            Data = bytes
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                request.Text = $"{request.Text}\n> Couldn't read content of `{filePath}` file. \n{ex}";
            }
        }
    }
}
