[![Build status](https://ci.appveyor.com/api/projects/status/thjw94949tm5lbw5/branch/master?svg=true)](https://ci.appveyor.com/project/nvborisenko/client-net/branch/master) [![NuGet Badge](https://buildstats.info/nuget/reportportal.client)](https://www.nuget.org/packages/reportportal.client) [![Coverage](https://codecov.io/gh/reportportal/client-net/branch/master/graph/badge.svg)](https://codecov.io/gh/reportportal/client-net)

## Project Description

API client for [Report Portal](https://reportportal.io)

This is a library containing methods, which allow to send the calls directly to Report portal and uses the RP API.
___
Technologies:
.NET Core, System.Net.Http

List the ready features here:
- Start, update, finish, delete launch;
- Create logs with all levels;
- Create/update user filter;

## Setup
Install **ReportPortal.Client** NuGet package.

[![NuGet version](https://badge.fury.io/nu/reportportal.client.svg)](https://badge.fury.io/nu/reportportal.client)

```powershell
PS> Install-Package ReportPortal.Client
```

## Usage
Example of using the library:

The work with tests logging is based on the ReportPortal.Client.Service object. To create it we  need to pass the Report Portal URI, project name and user UUID. Uuid value is specific for exact user and it could be found in Report Portal: User profile -> Access token
Creating a Service object:

````C#
var service = new ReportPortal.Client.Service(
new Uri("https://demo.reportportal.com/api/v1"), "ProjectName", "uuid");
 ````
 
Starting the launch:
````C#
var launch = await service.Launch.StartAsync(new StartLaunchRequest
        {
            Name = "LaunchName",
            Description = "LaunchDescription",
            StartTime = DateTime.UtcNow,
        });
````
To start test item we need to use the LaunchUuid received from the Launch.StartAsync method:
````C#
var test = await service.TestItem.StartAsync(new StartTestItemRequest
        {
            LaunchUuid = launch.Uuid,
            Name = "Test1",
            StartTime = DateTime.UtcNow,
            Type = TestItemType.Test
        });
````
To start log item the TestItemUuid is used which was received from the TestItem.StartAsync method:
````C#
var log = await service.LogItem.CreateAsync(new CreateLogItemRequest
         {
            TestItemUuid = test.Uuid,
            Text = "My log",
            Time = DateTime.UtcNow,
            Level = LogLevel.Debug
         }); 
````
Finishing the launch:
````C#
await Service.Launch.FinishAsync(launch.Uuid, new FinishLaunchRequest
        {
            EndTime = DateTime.UtcNow
        });
````