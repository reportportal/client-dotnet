namespace ReportPortal.Shared.Execution.Logging
{
    /// <inheritdoc />
    public class LogMessageAttachment : ILogMessageAttachment
    {
        /// <summary>
        /// Creates new instance of <see href="LogMessageAttachment"/> class.
        /// </summary>
        /// <param name="mimeType">Type of attachment like image/png.</param>
        /// <param name="data">Binary data of the attachment.</param>
        public LogMessageAttachment(string mimeType, byte[] data)
        {
            MimeType = mimeType;
            Data = data;
        }

        /// <inheritdoc />
        public string MimeType { get; set; }

        /// <inheritdoc />
        public byte[] Data { get; set; }
    }
}
