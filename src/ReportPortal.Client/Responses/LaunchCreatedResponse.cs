using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReportPortal.Client.Responses
{
    [DataContract]
    public class LaunchCreatedResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
