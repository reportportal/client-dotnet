using System;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Models;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request to finish specified test item.
    /// </summary>
    public class FinishTestItemRequest
    {
        /// <summary>
        /// Date time when test item is finished.
        /// </summary>
        [DataMember(Name = "end_time")]
        public string EndTimeString { get; set; }

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
        public Status Status { get; set; }

        /// <summary>
        /// A issue of test item if execution was proceeded with error.
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public Issue Issue { get; set; }
    }
}
