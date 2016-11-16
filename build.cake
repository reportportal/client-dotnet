#tool nuget:?package=NUnit.ConsoleRunner&version=3.5.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var build = Argument("build", "1.0.0");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var isAppVeyorBuild = AppVeyor.IsRunningOnAppVeyor;

// Define directories.
var buildDir = Directory("./src/ReportPortal.Shared/bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
{
	CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
	.IsDependentOn("Clean")
	.Does(() =>
{
	NuGetRestore("./src/ReportPortal.Shared.sln");
});

Task("Build")
	.IsDependentOn("Restore-NuGet-Packages")
	.Does(() =>
{
	if(IsRunningOnWindows())
	{
	  // Use MSBuild
	  MSBuild("./src/ReportPortal.Shared.sln", new MSBuildSettings().SetConfiguration(configuration));
	}
	else
	{
	  // Use XBuild
	  XBuild("./src/ReportPortal.Shared.sln", settings =>
		settings.SetConfiguration(configuration));
	}
});

Task("Package")
	.IsDependentOn("Build")
	.Does(() =>
{
	if (isAppVeyorBuild)
	{
		if (AppVeyor.Environment.Repository.Tag.IsTag)
		{
			build = AppVeyor.Environment.Repository.Tag.Name;
		}
		else
		{
			build = AppVeyor.Environment.Build.Version;
		}
	}
	else
	{
		build += "-local";
	}
	NuGetPack("src/ReportPortal.Shared/ReportPortal.Shared.nuspec", new NuGetPackSettings()
	{
		BasePath = "./src/ReportPortal.Shared/bin/" + configuration,
		Version = build
	});
	}
	);

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
