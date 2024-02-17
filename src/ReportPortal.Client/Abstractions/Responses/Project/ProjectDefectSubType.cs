namespace ReportPortal.Client.Abstractions.Responses.Project
{
    /// <summary>
    /// Represents a defect sub-type in a project.
    /// </summary>
    public class ProjectDefectSubType
    {
        /// <summary>
        /// Gets or sets the ID of the defect sub-type.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the color associated with the defect sub-type.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the locator of the defect sub-type.
        /// </summary>
        public string Locator { get; set; }

        /// <summary>
        /// Gets or sets the long name of the defect sub-type.
        /// </summary>
        public string LongName { get; set; }

        /// <summary>
        /// Gets or sets the short name of the defect sub-type.
        /// </summary>
        public string ShortName { get; set; }
    }
}
