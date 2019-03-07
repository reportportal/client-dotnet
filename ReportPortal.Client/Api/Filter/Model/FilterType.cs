using System.Runtime.Serialization;

namespace ReportPortal.Client.Api.Filter.Model
{
    public enum FilterType
    {
        [DataMember(Name = "launch")]
        Launch,
        [DataMember(Name = "testitem")]
        TestItem,
        [DataMember(Name = "log")]
        Log
    }
}
