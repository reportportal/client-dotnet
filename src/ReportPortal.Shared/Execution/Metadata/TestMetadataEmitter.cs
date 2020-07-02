using ReportPortal.Shared.Extensibility.Commands.CommandArgs;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReportPortal.Shared.Execution.Metadata
{
    public class TestMetadataEmitter : ITestMetadataEmitter
    {
        private TestCommandsSource _commandsSource;
        private ITestContext _testContext;

        private ObservableCollection<MetaAttribute> _attributes;

        public TestMetadataEmitter(ITestContext testContext, TestCommandsSource commandsSource)
        {
            _testContext = testContext;
            _commandsSource = commandsSource;
        }

        public ICollection<MetaAttribute> Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    var commandArgs = new TestAttributesCommandArgs(null);

                    TestCommandsSource.RaiseOnGetTestAttributes(_commandsSource, _testContext, commandArgs);

                    _attributes = new ObservableCollection<MetaAttribute>(commandArgs.Attributes);

                    _attributes.CollectionChanged += _attributes_CollectionChanged;
                }

                return _attributes;
            }
        }

        private void _attributes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var attributes = new Collection<MetaAttribute>();
                foreach (MetaAttribute attribute in e.NewItems)
                {
                    attributes.Add(attribute);
                }

                var args = new TestAttributesCommandArgs(attributes);

                TestCommandsSource.RaiseOnAddTestAttributes(_commandsSource, _testContext, args);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var attributes = new Collection<MetaAttribute>();
                foreach (MetaAttribute attribute in e.OldItems)
                {
                    attributes.Add(attribute);
                }

                var args = new TestAttributesCommandArgs(attributes);

                TestCommandsSource.RaiseOnRemoveTestAttributes(_commandsSource, _testContext, args);
            }
        }
    }
}
