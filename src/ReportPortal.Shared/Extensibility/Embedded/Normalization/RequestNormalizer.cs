using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Shared.Converters;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Reporter;
using System.Collections.Generic;

namespace ReportPortal.Shared.Extensibility.Embedded.Normalization
{
    /// <summary>
    /// Report events observer which makes basic validation and normalization before sending http requests to server.
    /// Examples:
    /// - Care about self/parent start/finish time
    /// - Limit long strings (name, attributes)
    /// </summary>
    public class RequestNormalizer : IReportEventsObserver
    {
        // TODO: make it configurable
        internal const int MAX_LAUNCH_NAME_LENGTH = 256;
        internal const int MAX_TEST_ITEM_NAME_LENGTH = 1024;

        internal const int MAX_ATTRIBUTE_KEY_LENGTH = 128;
        internal const int MAX_ATTRIBUTE_VALUE_LENGTH = 128;

        /// <inheritdoc/>
        public void Initialize(IReportEventsSource reportEventsSource)
        {
            reportEventsSource.OnBeforeLaunchStarting += ReportEventsSource_OnBeforeLaunchStarting;
            reportEventsSource.OnBeforeLaunchFinishing += ReportEventsSource_OnBeforeLaunchFinishing;
            reportEventsSource.OnBeforeTestStarting += ReportEventsSource_OnBeforeTestStarting;
            reportEventsSource.OnBeforeTestFinishing += ReportEventsSource_OnBeforeTestFinishing;
        }

        private void ReportEventsSource_OnBeforeLaunchStarting(ILaunchReporter launchReporter, BeforeLaunchStartingEventArgs args)
        {
            args.StartLaunchRequest.Name = StringTrimmer.Trim(args.StartLaunchRequest.Name, MAX_LAUNCH_NAME_LENGTH);
            
            NormalizeAttributes(args.StartLaunchRequest.Attributes);
        }

        private void ReportEventsSource_OnBeforeLaunchFinishing(ILaunchReporter launchReporter, BeforeLaunchFinishingEventArgs args)
        {
            if (args.FinishLaunchRequest.EndTime < launchReporter.Info.StartTime)
            {
                args.FinishLaunchRequest.EndTime = launchReporter.Info.StartTime;
            }
        }

        private void ReportEventsSource_OnBeforeTestStarting(ITestReporter testReporter, BeforeTestStartingEventArgs args)
        {
            var parentStartTime = testReporter.ParentTestReporter?.Info.StartTime ?? testReporter.LaunchReporter.Info.StartTime;

            if (args.StartTestItemRequest.StartTime < parentStartTime)
            {
                args.StartTestItemRequest.StartTime = parentStartTime;
            }

            args.StartTestItemRequest.Name = StringTrimmer.Trim(args.StartTestItemRequest.Name, MAX_TEST_ITEM_NAME_LENGTH);

            NormalizeAttributes(args.StartTestItemRequest.Attributes);
        }

        private void ReportEventsSource_OnBeforeTestFinishing(ITestReporter testReporter, BeforeTestFinishingEventArgs args)
        {
            if (args.FinishTestItemRequest.EndTime < testReporter.Info.StartTime)
            {
                args.FinishTestItemRequest.EndTime = testReporter.Info.StartTime;
            }

            NormalizeAttributes(args.FinishTestItemRequest.Attributes);

            args.FinishTestItemRequest.LaunchUuid = testReporter.LaunchReporter.Info.Uuid;
        }

        private static void NormalizeAttributes(IEnumerable<ItemAttribute> attributes)
        {
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    attribute.Key = StringTrimmer.Trim(attribute.Key, MAX_ATTRIBUTE_KEY_LENGTH);
                    attribute.Value = StringTrimmer.Trim(attribute.Value, MAX_ATTRIBUTE_VALUE_LENGTH);
                }
            }
        }
    }
}
