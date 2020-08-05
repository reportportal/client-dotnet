namespace ReportPortal.Shared.Execution.Logging
{
    /// <summary>
    /// Represents binary attachment for log events.
    /// </summary>
    public interface ILogMessageAttachment
    {
        /// <summary>
        /// Type of attachment like image/png.
        /// </summary>
        string MimeType { get; set; }

        /// <summary>
        /// Binary data of the attachment.
        /// </summary>
        byte[] Data { get; set; }
    }
}
