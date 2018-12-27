using System.Runtime.Serialization;

namespace ReportPortal.Shared.Configuration
{
    [DataContract]
    public class Authentication
    {
        private string _uuid;

        [DataMember(Name = "uuid")]
        public string Uuid
        {
            get { return Helper.GetEnvironmentProperty<string>(nameof(Uuid), _uuid); }
            set => _uuid = value;
        }
    }
}
