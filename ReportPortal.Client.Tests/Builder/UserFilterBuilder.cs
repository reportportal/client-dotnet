using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Client.Converters;

namespace ReportPortal.Client.Tests.Builder
{
    public class UserFilterBuilder
    {
        public static FilterElement BuildFlterElement()
        {
            var filterEntity = new FilterEntity
            {
                UserFilterCondition = UserFilterCondition.Contains,
                FilteringField = "name",
                Value = "test value"
            };

            var order1 = new FilterOrder
            {
                Asc = true,
                SortingColumn = "name",
            };

            var selectionParameters = new FilterSelectionParameter
            {
                Orders = new List<FilterOrder> {order1},
                PageNumber = 1
            };

            return new FilterElement
            {
                Name = Guid.NewGuid().ToString(),
                Description = "testDscr_1",
                IsLink = false,
                Share = true,
                UserFilterType = UserFilterType.Launch,
                Entities = new List<FilterEntity> {filterEntity},
                SelectionParameters = selectionParameters
            };
        }
    }
}
