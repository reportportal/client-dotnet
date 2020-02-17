using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
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
}
