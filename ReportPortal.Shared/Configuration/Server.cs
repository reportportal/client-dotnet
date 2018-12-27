using System;
using System.Runtime.Serialization;

namespace ReportPortal.Shared.Configuration
{
    [DataContract]
    public class Server
    {
        private Uri _url;
        private string _project;

        [DataMember(Name = "url")]
        public Uri Url
        {
            get { return Helper.GetEnvironmentProperty<Uri>(nameof(Url), _url); }
            set => _url = value;
        }

        [DataMember(Name = "project")]
        public string Project
        {
            get { return Helper.GetEnvironmentProperty<string>(nameof(Project), _project); }
            set => _project = value;
        }

        [DataMember(Name = "authentication")]
        public Authentication Authentication { get; set; }

        [DataMember(Name = "proxy")]
        public Uri Proxy { get; set; }
    }
}
