[![Build status](https://ci.appveyor.com/api/projects/status/thjw94949tm5lbw5/branch/master?svg=true)](https://ci.appveyor.com/project/nvborisenko/client-net/branch/master) [![NuGet Badge](https://buildstats.info/nuget/reportportal.client)](https://www.nuget.org/packages/reportportal.client) [![Coverage](https://codecov.io/gh/reportportal/client-net/branch/master/graph/badge.svg)](https://codecov.io/gh/reportportal/client-net)

# Project Name
API client for https://reportportal.io

## General Information
 This is a library containing methods, which allow to send the calls directly to Report portal and uses the RP API.

## Technologies Used
.NET Core, System.Net.Http

## Features
List the ready features here:
- Start, update, finish, delete launch;
- Create logs with all levels;
- Create/update user filter;

## Setup
Download using Nuget package console:

PM> Install-Package ReportPortal.Client

## Usage
Example for starting launch in Report Portal:

````C#
//Create service object
//Uuid value is specific for exact user, it could be found in Report Portal: User profile -> Access token
 protected readonly Service Service = new Service(new Uri("https://demo.reportportal.io/api/v1"), ProjectName, "uuid");
 
 //Start the launch
var launch = await Service.Launch.StartAsync(new StartLaunchRequest
	{
			Name = "LaunchName",
			Description = "LaunchDescription",
			StartTime = DateTime.UtcNow,
	});
 
````
After running this code the launch should be reflected in the Report Portal UI with this information:

**LaunchName #1
User name (connected with the specified uuid)
LaunchDescription**

## Project Status
Project is: _complete_