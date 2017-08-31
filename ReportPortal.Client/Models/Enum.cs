using System.ComponentModel;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Models
{
    /// <summary>
    /// Describes statuses of tests items.
    /// </summary>
    public enum Status
    {
        [DataMember(Name = "IN_PROGRESS")]
        InProgress,
        [DataMember(Name = "PASSED")]
        Passed,
        [DataMember(Name = "FAILED")]
        Failed,
        [DataMember(Name = "SKIPPED")]
        Skipped,
        [DataMember(Name = "INTERRUPTED")]
        Interrupted
    }

    /// <summary>
    /// Describes types of test items.
    /// </summary>
    public enum TestItemType
    {
        None,
        [DataMember(Name = "SUITE")]
        Suite,
        [DataMember(Name = "TEST")]
        Test,
        [DataMember(Name = "STEP")]
        Step,
        [DataMember(Name = "BEFORE_CLASS")]
        BeforeClass,
        [DataMember(Name = "AFTER_CLASS")]
        AfterClass,
        [DataMember(Name = "AFTER_METHOD")]
        AfterMethod,
        [DataMember(Name = "BEFORE_METHOD")]
        BeforeMethod
    }

    /// <summary>
    /// Describes issue types for test items.
    /// </summary>
    public enum IssueType
    {
        [DataMember(Name = "PB001")]
        ProductionBug,
        [DataMember(Name = "AB001")]
        AutomationBug,
        [DataMember(Name = "SI001")]
        SystemIssue,
        [DataMember(Name = "TI001")]
        ToInvestigate,
        [DataMember(Name = "ND001")]
        NoDefect
    }

    /// <summary>
    /// Describes levels for log items.
    /// </summary>
    public enum LogLevel
    {
        [DataMember(Name = "TRACE")]
        Trace,
        [DataMember(Name = "DEBUG")]
        Debug,
        [DataMember(Name = "INFO")]
        Info,
        [DataMember(Name = "WARN")]
        Warning,
        [DataMember(Name = "ERROR")]
        Error
    }

    /// <summary>
    /// Describes modes for launches.
    /// </summary>
    public enum LaunchMode
    {
        [DataMember(Name = "DEFAULT")]
        Default,
        [DataMember(Name = "DEBUG")]
        Debug
    }
}
