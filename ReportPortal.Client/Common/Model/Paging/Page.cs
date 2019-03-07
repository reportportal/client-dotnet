using System.Runtime.Serialization;

namespace ReportPortal.Client.Common.Model.Paging
{
    [DataContract]
    public class Page
    {
        public Page()
        {
        }

        public Page(int number, int size)
        {
            Number = number;
            Size = size;
        }

        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "totalElements")]
        public int TotalElements { get; set; }

        [DataMember(Name = "totalPages")]
        public int TotalPages { get; set; }

        [DataMember(Name = "number")]
        public int Number { get; set; }
    }
}
