using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to finish specified test item.
    /// </summary>
    [DataContract]
    public class FinishTestItemRequest
    {
        /// <summary>
        /// A long description of test item.
        /// </summary>
        [DataMember(Name = "description", EmitDefaultValue = true)]
        public string Description { get; set; }

        /// <summary>
        /// Date time when test item is finished.
        /// </summary>
        [DataMember(Name = "endTime")]
        public string EndTimeString { get; set; } = DateTimeConverter.ConvertFrom(DateTime.UtcNow);

        public DateTime EndTime
        {
            get
            {
                return DateTimeConverter.ConvertTo(EndTimeString);
            }
            set
            {
                EndTimeString = DateTimeConverter.ConvertFrom(value);
            }
        }

        /// <summary>
        /// A result of test item.
        /// </summary>
        [DataMember(Name = "status")]
        public string StatusString { get { return EnumConverter.ConvertFrom(Status); } set { Status = EnumConverter.ConvertTo<Status>(value); } }

        public Status Status { get; set; } = Status.Passed;

        /// <summary>
        /// A issue of test item if execution was proceeded with error.
        /// </summary>
        [DataMember(Name = "issue")]
        public Issue Issue { get; set; }

        /// <summary>
        /// A list of tags.
        /// </summary>
        [Obsolete("Use Attributes instead of tags")]
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Sets attributes when finishing test item.
        /// </summary>
        /// <value>List of attributes.</value>
        [DataMember(Name = "attributes")]
        public IList<ItemAttribute> Attributes { get; set; }

        /// <summary>
        /// Retry status indicator.
        /// </summary>
        [DataMember(Name = "retry")]
        public bool IsRetry { get; set; }

        /// <summary>
        /// Test Item to be marked as retry.
        /// </summary>
        [DataMember(Name = "retryOf")]
        public string RetryOf { get; set; }
    }
}
