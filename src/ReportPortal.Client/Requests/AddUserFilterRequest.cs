using System.Collections.Generic;
using System.Runtime.Serialization;
using ReportPortal.Client.Converters;
using ReportPortal.Client.Filtering;
using ReportPortal.Client.Models;

namespace ReportPortal.Client.Requests
{
    /// <summary>
    /// Defines a request for creating of user filters
    /// </summary>
    [DataContract]
    public class AddUserFilterRequest
    {
        /// <summary>
        /// list of filter elements
        /// </summary>
        [DataMember(Name = "elements")]
        public IEnumerable<FilterElement> FilterElements { get; set; }
    }

    [DataContract]
    public class FilterElement
    {
        /// <summary>
        /// description of filter element
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// list of entities
        /// </summary>
        [DataMember(Name = "entities")]
        public IEnumerable<FilterEntity> Entities { get; set; }

        /// <summary>
        /// is element a link
        /// </summary>
        [DataMember(Name = "is_link")]
        public bool IsLink { get; set; }

        /// <summary>
        /// name of filter element
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// selection parameters
        /// </summary>
        [DataMember(Name = "selection_parameters")]
        public FilterSelectionParameter SelectionParameters { get; set; }

        /// <summary>
        /// is filter shared
        /// </summary>
        [DataMember(Name = "share")]
        public bool Share { get; set; }

        /// <summary>
        /// type of filter element
        /// </summary>
        [DataMember(Name = "type")]
        public string TypeStr { get; set; }

        /// <summary>
        /// enum corresponds to the filter type
        /// </summary>
        public UserFilterType UserFilterType
        {
            get => EnumConverter.ConvertTo<UserFilterType>(TypeStr);
            set => TypeStr = EnumConverter.ConvertFrom(value);
        }
    }

    [DataContract]
    public class FilterEntity
    {
        /// <summary>
        /// condition to filter with
        /// </summary>
        [DataMember(Name = "condition")]
        public string ConditionStr { get; set; }

        public FilterOperation UserFilterCondition
        {
            get => EnumConverter.ConvertTo<FilterOperation>(ConditionStr);
            set => ConditionStr = EnumConverter.ConvertFrom(value);
        }

        /// <summary>
        /// field to filter by
        /// </summary>
        [DataMember(Name = "filtering_field")]
        public string FilteringField { get; set; }

        /// <summary>
        /// value to filter by
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }

    [DataContract]
    public class FilterSelectionParameter
    {
        /// <summary>
        /// list of orders to sort
        /// </summary>
        [DataMember(Name = "orders")]
        public IEnumerable<FilterOrder> Orders { get; set; }

        /// <summary>
        /// the number of page
        /// </summary>
        [DataMember(Name = "page_number")]
        public int PageNumber { get; set; }
    }

    [DataContract]
    public class FilterOrder
    {
        /// <summary>
        /// is asc
        /// </summary>
        [DataMember(Name = "isAsc")]
        public bool Asc { get; set; }

        /// <summary>
        /// a column to sort by
        /// </summary>
        [DataMember(Name = "sortingColumn")]
        public string SortingColumn { get; set; }
    }
}