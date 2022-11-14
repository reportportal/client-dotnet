Sometimes it would be great to have ability to explore what payload is sent to ReportPortal server and update it or add extra one based on your own conditions. `ReportPortal.Shared` package provides you this ability by implementing `IReportEventsObserver` interface.

![Diagram](./assets/extentions_diagram.png)

# How to use observers

To be able to observe events, next conditions should be met:
1. Implement `IReportEventsObserver` interface
2. Name of an assembly with implementation from previous step should contain `ReportPortal` (e.g. `ReportPortal.MyExtension.dll`)
3. The assembly should be in the same directory with `ReportPortal.Shared.dll`

# What can be observed
`IReportEventsObserver` interface allows to observe such events as:

- before/after starting/finishing launch
- before/after starting/finishing test item
- before/after sending log items

Please, have a look the interface for understanding what actions also can be observed.

# Examples

## 1. Test names updating

Let's imagine that you use snake case for naming your tests, but would like to improve readability of test names on ReportPortal server by replacing each underscore by empty space.

Next code snippet shows how it can be done:

```cs
public class ReportPortalEventsObserver : IReportEventsObserver
{
    // this method invoked once
    public void Initialize(IReportEventsSource reportEventsSource)
    {
        // we are interested in event when any test item starts to execute (including suites, tests, steps)
        reportEventsSource.OnBeforeTestStarting += (reporter, args) => {
            args.StartTestItemRequest.Name = args.StartTestItemRequest.Name.Replace('_', ' ');
        });
    }
}
```

## 2. Adding dynamic information

Let's imagine that you have to add some dynamic information to ReportPortal launch. _Build number_, for instance. 

Next code snippet shows how it can be done:

```cs
public class ReportPortalEventsObserver : IReportEventsObserver
{
    public void Initialize(IReportEventsSource reportEventsSource)
    {
        reportEventsSource.OnBeforeLaunchStarting += ReportEventsSource_OnBeforeLaunchStarting;
    }

    private void ReportEventsSource_OnBeforeLaunchStarting(ILaunchReporter launchReporter, BeforeLaunchStartingEventArgs args)
    {
        args.StartLaunchRequest.Attributes = args.StartLaunchRequest.Attributes ?? new List<ItemAttribute>();

        args.StartLaunchRequest.Attributes.Add(new ItemAttribute
        {
            Key = nameof(Configuration.BuildNumber),
            Value = Configuration.BuildNumber
        });
    }
}
```
