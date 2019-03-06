using System;
using ReportPortal.Client.Converter;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Api.Launch.Requests
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
    }
}
