using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Client.Converters;

namespace ReportPortal.Client.Abstractions.Responses
{
    [DataContract]
    public class UserFilter
    {
        /// <summary>
        /// description of user filter
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// list of entities
        /// </summary>
        [DataMember(Name = "entities")]
        public IEnumerable<FilterEntity> Entities { get; set; }

        /// <summary>
        /// id of user filter
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "is_link")]
        public bool IsLink { get; set; }

        /// <summary>
        /// name of user filter
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// list of parameters of selection
        /// </summary>
        [DataMember(Name = "selection_parameters")]
        public IEnumerable<FilterSelectionParameter> SelectionParameters { get; set; }

        /// <summary>
        /// is filter shared
        /// </summary>
        [DataMember(Name = "share")]
        public bool Share { get; set; }

        /// <summary>
        /// filter type
        /// </summary>
        [DataMember(Name = "type")]
        public string TypeStr { get; set; }

        /// <summary>
        /// user filter type enum
        /// </summary>
        public UserFilterType UserFilterType
        {
            get => EnumConverter.ConvertTo<UserFilterType>(TypeStr);
            set => TypeStr = EnumConverter.ConvertFrom(UserFilterType);

        }

        /// <summary>
        /// owner of user filter
        /// </summary>
        [DataMember(Name = "owner")]
        public string Owner { get; set; }
    }
}
