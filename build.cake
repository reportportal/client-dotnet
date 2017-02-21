#tool nuget:?package=NUnit.ConsoleRunner&version=3.5.0
#tool nuget:?package=OpenCover

#tool coveralls.io
#addin Cake.Coveralls
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
var buildDir = Directory("./src/ReportPortal.Client/bin") + Directory(configuration);

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
	NuGetRestore("./src/ReportPortal.Client.sln");
});

Task("Build")
	.IsDependentOn("Restore-NuGet-Packages")
	.Does(() =>
{
	if(IsRunningOnWindows())
	{
	  // Use MSBuild
	  MSBuild("./src/ReportPortal.Client.sln", new MSBuildSettings().SetConfiguration(configuration));
	}
	else
	{
	  // Use XBuild
	  XBuild("./src/ReportPortal.Client.sln", settings =>
		settings.SetConfiguration(configuration));
	}
});

Task("Run-Unit-Tests")
	.IsDependentOn("Build")
	.Does(() =>
{
	NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
		NoResults = true
		});
});

Task("Generate-Coverage")
	.IsDependentOn("Run-Unit-Tests")
	.Does(() =>
{
	OpenCover(tool => {
  		tool.NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
			NoResults = true
			});
  		},
  		new FilePath("./CoverageResults.xml"),
  		new OpenCoverSettings().WithFilter("+[ReportPortal.Client]*")
    );
});

Task("Upload-Coverage-Report")
	.IsDependentOn("Generate-Coverage")
	.Does(() =>
{
		CoverallsIo("CoverageResults.xml", CoverallsNetReportType.OpenCover);
});

Task("Package")
	.IsDependentOn("Upload-Coverage-Report")
	.Does(() =>
{
	if (isAppVeyorBuild)
	{
		if (AppVeyor.Environment.Repository.Tag.IsTag)
		{
			build = AppVeyor.Environment.Repository.Tag.Name;
			BuildSystem.AppVeyor.UpdateBuildVersion(build);
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
	NuGetPack("src/ReportPortal.Client/ReportPortal.Client.nuspec", new NuGetPackSettings()
	{
		BasePath = "./src/ReportPortal.Client/bin/" + configuration,
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
