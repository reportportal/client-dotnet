namespace ReportPortal.Shared.Execution.Logging
{
    public class LogMessageAttachment : ILogMessageAttachment
    {
        public LogMessageAttachment(string mimeType, byte[] data)
        {
            MimeType = mimeType;
            Data = data;
        }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }
    }
}
