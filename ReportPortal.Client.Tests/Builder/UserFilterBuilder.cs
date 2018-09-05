using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using ReportPortal.Client.Models;
using ReportPortal.Client.Requests;
using ReportPortal.Client.Converters;

namespace ReportPortal.Client.Tests.Builder
{
    public class UserFilterBuilder
    {
        private static readonly RandomGenerator _randomGenerator = new RandomGenerator();
        
        public static FilterElement BuildRandomFlterElement()
        {
            var filterEntity = Builder<FilterEntity>.CreateNew()
                .With(x => x.UserFilterCondition = UserFilterCondition.Contains)
                .With(x => x.FilteringField = "name")
                .With(x => x.Value = _randomGenerator.NextString(10, 30))
                .Build();

            var order1 = Builder<FilterOrder>.CreateNew()
                .With(x => x.Asc = _randomGenerator.Boolean())
                .With(x => x.SortingColumn = "name")
                .Build();

            var selectionParameters = Builder<FilterSelectionParameter>.CreateNew()
                .With(x => x.Orders = new List<FilterOrder> { order1 })
                .Build();

            return Builder<FilterElement>.CreateNew()
                .With(x => x.Name = _randomGenerator.NextString(5, 50))
                .With(x => x.Description = _randomGenerator.Phrase(15))
                .With(x => x.IsLink = _randomGenerator.Boolean())
                .With(x => x.Share = true)
                .With(x => x.UserFilterType = UserFilterType.Launch)
                .With(x => x.Entities = new List<FilterEntity> { filterEntity })
                .With(x => x.SelectionParameters = selectionParameters)
                .Build();
        }
    }
}
