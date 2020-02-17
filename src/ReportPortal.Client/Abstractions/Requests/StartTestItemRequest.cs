using System;
using System.Collections.Generic;
using ReportPortal.Client.Converters;
using System.Runtime.Serialization;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Abstractions.Models;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a content of request for service to create new test item in progress state.
    /// </summary>
    [DataContract]
    public class StartTestItemRequest
    {
        /// <summary>
        /// ID of parent launch to create new test item.
        /// </summary>
        [DataMember(Name = "launchUuid")]
        public string LaunchUuid { get; set; }

        private string _name;

        /// <summary>
        /// A short name of test item.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get { return _name; } set { _name = StringTrimmer.Trim(value, 256); } }

        /// <summary>
        /// A long description of test item.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = true)]
        public string Description { get; set; }

        /// <summary>
        /// Date time when new test item is created.
        /// </summary>
        [DataMember(Name = "startTime")]
        public string StartTimeString { get; set; }

        public DateTime StartTime
        {
            get
            {
                return DateTimeConverter.ConvertTo(StartTimeString);
            }
            set
            {
                StartTimeString = DateTimeConverter.ConvertFrom(value);
            }
        }

        /// <summary>
        /// A type of test item.
        /// </summary>
        [DataMember(Name = "type")]
        public string TypeString { get { return EnumConverter.ConvertFrom(Type); } set { Type = EnumConverter.ConvertTo<TestItemType>(value); } }

        public TestItemType Type = TestItemType.Test;

        /// <summary>
        /// A list of tags.
        /// </summary>
        [DataMember(Name = "tags", EmitDefaultValue = true)]
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Retry status indicator.
        /// </summary>
        [DataMember(Name = "retry")]
        public bool IsRetry { get; set; }

        /// <summary>
        /// A list of parameters.
        /// </summary>
        [DataMember(Name = "parameters")]
        public IEnumerable<KeyValuePair<string, string>> Parameters { get; set; }

        /// <summary>
        /// A test item unique id.
        /// </summary>
        [DataMember(Name = "uniqueId", EmitDefaultValue = true)]
        public string UniqueId { get; set; }

        [DataMember(Name = "hasStats")]
        public bool HasStats { get; set; } = true;

        /// <summary>
        /// Test item attributes.
        /// </summary>
        [DataMember(Name = "attributes")]
        public IEnumerable<Attribute> Attributes { get; set; }

        [DataContract]
        public class Attribute
        {
            [DataMember(Name = "key")]
            public string Key { get; set; }

            [DataMember(Name = "value")]
            public string Value { get; set; }

            [DataMember(Name = "system")]
            public bool IsSystem { get; set; }
        }
    }
}
