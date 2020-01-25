﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReportPortal.Client.Models
{
    [DataContract]
    public class EntryCreated
    {
        /// <summary>
        /// ID of created entry
        /// </summary>
        [DataMember(Name= "id")]
        public string Id { get; set; }
    }
}
