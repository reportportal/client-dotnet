using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ReportPortal.Client.Models
{
    [DataContract]
    public class EntryCreated
    {
        /// <summary>
        /// ID of crated entry
        /// </summary>
        [DataMember(Name= "id")]
        public string Id { get; set; }
    }
}
