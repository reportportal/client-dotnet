using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Responses;
using ReportPortal.Client.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReportPortal.Client.Abstractions.Requests
{
    /// <summary>
    /// Defines a request for creating of user filters
    /// </summary>
    [DataContract]
    public class CreateUserFilterRequest
    {
        /// <summary>
        /// Name of user filter.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of user filter.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// List of conditions to filter data.
        /// </summary>
        [DataMember(Name = "conditions")]
        public IEnumerable<Condition> Conditions { get; set; }

        /// <summary>
        /// List of parameters of selection.
        /// </summary>
        [DataMember(Name = "orders")]
        public IEnumerable<FilterOrder> Orders { get; set; }

        /// <summary>
        /// Indicates if filter is shared.
        /// </summary>
        [DataMember(Name = "share")]
        public bool IsShared { get; set; }

        /// <summary>
        /// Filter type.
        /// </summary>
        [DataMember(Name = "type")]
        public string TypeStr { get; set; }

        /// <summary>
        /// Owner of the filter.
        /// </summary>
        [DataMember(Name = "owner")]
        public string Owner { get; set; }

        /// <summary>
        /// User filter type enum.
        /// </summary>
        public UserFilterType UserFilterType
        {
            get => EnumConverter.ConvertTo<UserFilterType>(TypeStr);
            set => TypeStr = EnumConverter.ConvertFrom(UserFilterType);

        }
    }
}