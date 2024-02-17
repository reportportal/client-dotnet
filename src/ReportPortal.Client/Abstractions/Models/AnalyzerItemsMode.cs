namespace ReportPortal.Client.Abstractions.Models
{
    /// <summary>
    /// Represents the mode of analyzer items.
    /// </summary>
    public class AnalyzerItemsMode
    {
        /// <summary>
        /// Represents the "To Investigate" mode of analyzer items.
        /// </summary>
        public const string ToInvestigate = "TO_INVESTIGATE";

        /// <summary>
        /// Represents the "Auto Analyzed" mode of analyzer items.
        /// </summary>
        public const string AutoAnalyzed = "AUTO_ANALYZED";

        /// <summary>
        /// Represents the "Manually Analyzed" mode of analyzer items.
        /// </summary>
        public const string ManuallyAnalyzed = "MANUALLY_ANALYZED";
    }
}
