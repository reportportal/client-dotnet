namespace ReportPortal.Shared.Execution.Metadata
{
    class TestMetadataEmitter : ITestMetadataEmitter
    {
        private TestCommandsSource _commandsSource;
        private ITestContext _testContext;

        private IMetaAttributesCollection _attributes;

        public TestMetadataEmitter(ITestContext testContext, TestCommandsSource commandsSource)
        {
            _testContext = testContext;
            _commandsSource = commandsSource;
        }

        public IMetaAttributesCollection Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    _attributes = new MetaAttributesCollection(_testContext, _commandsSource);
                }

                return _attributes;
            }
        }
    }
}
