namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Represents a struct that defines well-known issue types.
    /// </summary>
    public struct WellKnownIssueType
    {
        /// <summary>
        /// Represents a product bug issue type.
        /// </summary>
        public const string ProductBug = "PB001";

        /// <summary>
        /// Represents an automation bug issue type.
        /// </summary>
        public const string AutomationBug = "AB001";

        /// <summary>
        /// Represents a system issue type.
        /// </summary>
        public const string SystemIssue = "SI001";

        /// <summary>
        /// Represents an issue type that needs to be investigated.
        /// </summary>
        public const string ToInvestigate = "TI001";

        /// <summary>
        /// Represents an issue type that is not a defect.
        /// </summary>
        public const string NotDefect = "ND001";
    }
}
