﻿using System.Runtime.Serialization;

namespace ReportPortal.Client.Responses
{
    [DataContract]
    public class LaunchFinishedResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }
    }
}