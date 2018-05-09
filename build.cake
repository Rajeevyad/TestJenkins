//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
#addin nuget:?package=Cake.Npm&version=0.13.0
#tool nuget:?package=xunit.runner.console&version=2.2.0
#tool nuget:?package=OpenCover&Version=4.6.519
#tool nuget:?package=ReportGenerator&version=2.5.8

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var distDirectory = Directory("./TestJenkins/bin/dist"); 
var coveragePath = Directory(distDirectory) + Directory("Coverage");
var coverageXml = coveragePath + File("Coverage.xml");


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Clean")
	.Does(() => 
	{
		// Clean solution directories.
		FilePathCollection projects = GetFiles("./**/**/*.csproj");
		foreach(var path in projects)
		{
			Information("Cleaning {0}", path);
			CleanDirectories(path + "/**/bin/" + configuration);
			CleanDirectories(path + "/**/obj/" + configuration);
		}
	});

Task("Restore")
    .Does(()=>{
            NuGetRestore("TestJenkins.sln");
        });

Task("NpmInstall")
.Does(() =>
{
    NpmInstall(s => s.FromPath("./TestJenkins/"));
});

Task("Build")
    .Does(()=>{
        // DotNetBuild("TestJenkins/TestJenkins.csproj",
        // settings=>settings.SetConfiguration(configuration).WithTarget("Build"));
	  DotNetCoreBuild("TestJenkins.sln", new DotNetCoreBuildSettings
		{
			Configuration = configuration,
			ArgumentCustomization = arg => arg.AppendSwitch("/p:DebugType","=","Full")
		});
    });

	Task("Test")
       .Does(() => 
       {

		 var success = true;
		 var openCoverSettings = new OpenCoverSettings
		{
			OldStyle = true,
			MergeOutput = true
		}
		.WithFilter("+[*]*")
		.WithFilter("-[FakeItEasy]*")
		.WithFilter("-[*Test]*");

 		// foreach(var project in  GetFiles("./**/*.Test.csproj"))
        // {
            try 
            {
                // var projectFile = "TestJenkins/TestJenkins.csproj"; //MakeAbsolute(project).ToString();
				Information("Testing {0}", "TestjenkinsTest/TestjenkinsTest.Test.csproj");
				if (!DirectoryExists(coveragePath))
				{
					CreateDirectory(coveragePath);
				}

                var dotNetTestSettings = new DotNetCoreTestSettings
                {
                    Configuration = configuration,
                    NoBuild = true
                };

                OpenCover(context => context.DotNetCoreTest("TestjenkinsTest/TestjenkinsTest.Test.csproj", dotNetTestSettings), coverageXml, openCoverSettings);
            }
            catch(Exception ex)
            {
                success = false;
                Error("There was an error while running the tests", ex);
            }
        //}

		if(success == false)
		{
			throw new CakeException("There was an error while running the tests");
		}
		
       });

	   
Task("CodeCoverage")
       .Does(()=>
       {
             ReportGenerator(
               coverageXml,
               coveragePath,
               new ReportGeneratorSettings
               {
                    ReportTypes = new[] {ReportGeneratorReportType.Html}
               }
               );
       });


		// Publish the app to the /dist folder
Task("PublishWeb")  
    .Does(() =>
    {
        DotNetCorePublish(
            "./TestJenkins/TestJenkins.csproj",
            new DotNetCorePublishSettings()
            {
                Configuration = configuration,
                OutputDirectory = distDirectory,
                ArgumentCustomization = args => args.Append("--no-restore"),
            });
    }); 

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

// Default build - can be run locally, doesn't interact with nuget feed or Octopus
Task("Default")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.IsDependentOn("NpmInstall")
	.IsDependentOn("Build")	
	.IsDependentOn("PublishWeb");
	// .IsDependentOn("Test")
	// .IsDependentOn("CodeCoverage")

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////
RunTarget(target);