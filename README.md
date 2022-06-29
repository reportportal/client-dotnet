[![Build status](https://ci.appveyor.com/api/projects/status/thjw94949tm5lbw5/branch/master?svg=true)](https://ci.appveyor.com/project/nvborisenko/client-net/branch/master) [![NuGet Badge](https://buildstats.info/nuget/reportportal.client)](https://www.nuget.org/packages/reportportal.client) [![Coverage](https://codecov.io/gh/reportportal/client-net/branch/master/graph/badge.svg)](https://codecov.io/gh/reportportal/client-net)

## Project Description

API client for [Report Portal](https://reportportal.io)

This is a library containing methods, which allow to send the calls directly to Report portal and uses the RP API.

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

````C#
//Create service object
//Uuid value is specific for exact user, it could be found in Report Portal: User profile -> Access token
 var service = new ReportPortal.Client.Service(new Uri("https://demo.reportportal.com/api/v1"), "ProjectName", "uuid");
 
 //Start the launch
var launch = await service.Launch.StartAsync(new StartLaunchRequest
        {
            Name = "LaunchName",
            Description = "LaunchDescription",
            StartTime = DateTime.UtcNow,
        });

//Start test item
var test = await service.TestItem.StartAsync(new StartTestItemRequest
        {
            LaunchUuid = launch.Uuid,
            Name = "Test1",
            StartTime = DateTime.UtcNow,
            Type = TestItemType.Test
        });

//Add log item
var log = await service.LogItem.CreateAsync(new CreateLogItemRequest
         {
            TestItemUuid = test.Uuid,
            Text = "My log",
            Time = DateTime.UtcNow,
            Level = LogLevel.Debug
         }); 
````