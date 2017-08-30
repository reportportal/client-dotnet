using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportPortal.Client.Models
{
    public class User
    {
        [DataMember(Name = "full_name")]
        public string Fullname { get; set; }

        public string Email { get; set; }
    }
}
