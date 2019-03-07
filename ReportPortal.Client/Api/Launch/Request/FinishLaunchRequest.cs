using System;
using System.Runtime.Serialization;
using ReportPortal.Client.Converter;

namespace ReportPortal.Client.Api.Launch.Request
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
            get => DateTimeConverter.ConvertTo(EndTimeString);
            set => EndTimeString = DateTimeConverter.ConvertFrom(value);
        }
    }
}
