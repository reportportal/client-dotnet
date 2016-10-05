using System.ComponentModel;

namespace ReportPortal.Client.Models
{
    /// <summary>
    /// Describes statuses of tests items.
    /// </summary>
    public enum Status
    {
        None,
        [Description("IN_PROGRESS")]
        InProgress,
        [Description("PASSED")]
        Passed,
        [Description("FAILED")]
        Failed,
        [Description("SKIPPED")]
        Skipped,
        [Description("INTERRUPTED")]
        Interrupted
    }

    /// <summary>
    /// Describes types of test items.
    /// </summary>
    public enum TestItemType
    {
        None,
        [Description("SUITE")]
        Suite,
        [Description("TEST")]
        Test,
        [Description("STEP")]
        Step,
        [Description("BEFORE_CLASS")]
        BeforeClass,
        [Description("AFTER_CLASS")]
        AfterClass,
        [Description("AFTER_METHOD")]
        AfterMethod,
        [Description("BEFORE_METHOD")]
        BeforeMethod
    }

    /// <summary>
    /// Describes issue types for test items.
    /// </summary>
    public enum IssueType
    {
        None,
        [Description("PB001")]
        ProductionBug,
        [Description("AB001")]
        AutomationBug,
        [Description("SI001")]
        SystemIssue,
        [Description("TI001")]
        ToInvestigate,
        [Description("ND001")]
        NoDefect
    }

    /// <summary>
    /// Describes levels for log items.
    /// </summary>
    public enum LogLevel
    {
        None,
        [Description("TRACE")]
        Trace,
        [Description("DEBUG")]
        Debug,
        [Description("INFO")]
        Info,
        [Description("WARN")]
        Warning,
        [Description("ERROR")]
        Error
    }

    /// <summary>
    /// Describes modes for launches.
    /// </summary>
    public enum LaunchMode
    {
        [Description("DEFAULT")]
        Default,
        [Description("DEBUG")]
        Debug
    }
}
