using ReportPortal.Client.Converters;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Models
{
    [DataContract]
    public class ItemAttribute
    {
        public const int MAX_KEY_SIZE = 128;
        public const int MAX_VALUE_SIZE = 128;

        private string _key;

        [DataMember(Name = "key")]
        public string Key { get { return _key; } set { _key = StringTrimmer.Trim(value, MAX_KEY_SIZE); } }

        private string _value;

        [DataMember(Name = "value")]
        public string Value { get { return _value; } set { _value = StringTrimmer.Trim(value, MAX_VALUE_SIZE); } }

        [DataMember(Name = "system")]
        public bool IsSystem { get; set; }
    }
}
