using ReportPortal.Shared.Extensibility.Commands.CommandArgs;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReportPortal.Shared.Execution.Metadata
{
    class MetaAttributesCollection : IMetaAttributesCollection
    {
        private ITestContext _testContext;
        private TestCommandsSource _commandsSource;

        private ObservableCollection<MetaAttribute> _attributes;

        public MetaAttributesCollection(ITestContext testContext, TestCommandsSource commandsSource)
        {
            _testContext = testContext;
            _commandsSource = commandsSource;

            var commandArgs = new TestAttributesCommandArgs(null);

            TestCommandsSource.RaiseOnGetTestAttributes(_commandsSource, _testContext, commandArgs);

            _attributes = new ObservableCollection<MetaAttribute>(commandArgs.Attributes);

            _attributes.CollectionChanged += _attributes_CollectionChanged;
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

        public int Count => _attributes.Count;

        public bool IsReadOnly => false;

        public void Add(MetaAttribute item)
        {
            _attributes.Add(item);
        }

        public void Add(string key, string value)
        {
            var item = new MetaAttribute(key, value);

            Add(item);
        }

        public void Clear()
        {
            _attributes.Clear();
        }

        public bool Contains(MetaAttribute item)
        {
            return _attributes.Contains(item);
        }

        public void CopyTo(MetaAttribute[] array, int arrayIndex)
        {
            _attributes.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MetaAttribute> GetEnumerator()
        {
            return _attributes.GetEnumerator();
        }

        public bool Remove(MetaAttribute item)
        {
            return _attributes.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _attributes.GetEnumerator();
        }
    }
}
