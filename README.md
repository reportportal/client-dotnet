API client for [Report Portal](https://reportportal.io)

Provides an ability to interact with Report Portal API in .NET/C#. Supports starting/finishing launches/tests, sending logs.

[![CI](https://github.com/reportportal/client-net/actions/workflows/ci.yml/badge.svg)](https://github.com/reportportal/client-net/actions/workflows/ci.yml) [![NuGet Badge](https://buildstats.info/nuget/reportportal.client)](https://www.nuget.org/packages/reportportal.client) [![Coverage](https://codecov.io/gh/reportportal/client-net/branch/master/graph/badge.svg)](https://codecov.io/gh/reportportal/client-net)

## Setup

Install **ReportPortal.Client** NuGet package.

```powershell
PS> Install-Package ReportPortal.Client
```

## Usage

The main entry point to start interact with API is `ReportPortal.Client.Service` class. It requires uri, project name and uuid. Uuid value is specific for an user and it can be obtained on User Profile page.

```C#
var service = new ReportPortal.Client.Service(
new Uri("https://demo.reportportal.com"), "my_project", "my_uuid");
 ```
 
Starting new launch:
```C#
var launch = await service.Launch.StartAsync(new StartLaunchRequest
    {
        Name = "LaunchName",
        Description = "LaunchDescription"
    });
```

To start test item we need to use the `LaunchUuid` received from the previous step:
```C#
var test = await service.TestItem.StartAsync(new StartTestItemRequest
    {
        LaunchUuid = launch.Uuid,
        Name = "Test1",
        Type = TestItemType.Test
    });
```

To send log item the `TestItemUuid` is used which was received from the previous step:
```C#
var log = await service.LogItem.CreateAsync(new CreateLogItemRequest
    {
        TestItemUuid = test.Uuid,
        Text = "My log",
        Level = LogLevel.Debug
    }); 
```

Finishing the test:
```C#
await Service.TestItem.FinishAsync(test.Uuid, new FinishTestItemRequest
    {
        Status = Status.Passed
    });
```

Finishing the launch:
```C#
await Service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest());
```
