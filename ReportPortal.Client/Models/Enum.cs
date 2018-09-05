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

    public struct WellKnownIssueType
    {
        public const string ProductBug = "PB001";
        public const string AutomationBug = "AB001";
        public const string SystemIssue = "SI001";
        public const string ToInvestigate = "TI001";
        public const string NotDefect = "ND001";
    }

    public enum AnalyzerMode
    {
        [DataMember(Name = "ALL")]
        All,
        [DataMember(Name = "CURRENT_LAUNCH")]
        CurrentLaunch,
        [DataMember(Name = "LAUNCH_NAME")]
        LaunchName
    }

    public enum AnalyzerItemsMode
    {
        [DataMember(Name = "TO_INVESTIGATE")]
        ToInvestigate,
        [DataMember(Name = "AUTO_ANALYZED")]
        AutoAnalyzed,
        [DataMember(Name = "MANUALLY_ANALYZED")]
        ManuallyAnalyzed
    }

    public enum UserFilterType
    {
        [DataMember(Name = "launch")]
        Launch,
        [DataMember(Name = "testitem")]
        TestItem,
        [DataMember(Name = "log")]
        Log
    }

    public enum UserFilterCondition
    {
        [DataMember(Name = "cnt")]
        Contains,
        [DataMember(Name = "!cnt")]
        NotContains,
        [DataMember(Name = "eq")]
        Equals,
        [DataMember(Name = "!eq")]
        NotEquals,
    }
}
