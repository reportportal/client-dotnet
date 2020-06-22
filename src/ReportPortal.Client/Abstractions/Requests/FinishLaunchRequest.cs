using ReportPortal.Client.Converters;
using System;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request to finish execution of specified launch.
    /// </summary>
    [DataContract]
    public class FinishLaunchRequest
    {
        /// <summary>
        /// Date time when launch execution is finished.
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
    }
}
