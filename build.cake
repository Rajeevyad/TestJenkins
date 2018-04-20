//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
#addin nuget:?package=Cake.Npm&version=0.13.0
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
        DotNetBuild("TestJenkins/TestJenkins.csproj",
        settings=>settings.SetConfiguration(configuration)
                                        .WithTarget("Build"));
    });

	Task("Test")
       .Does(() => 
       {
        
		FilePathCollection projects = GetFiles("./**/*.Test.csproj");
        foreach (FilePath project in projects)
        {
				if (!DirectoryExists(coveragePath))
				{
					CreateDirectory(coveragePath);
				}

               OpenCover(tool=>tool.DotNetCoreTest(
                project.FullPath,
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true
                }),
                           coverageXml,
                           new OpenCoverSettings()
                           .WithFilter("+[TestJenkins.*]*")
                           .WithFilter("-[TestJenkins.*Test*]*")
                           );
        }
       });


	   
Task("Report-Coverage")
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
	.IsDependentOn("Test")
	.IsDependentOn("Report-Coverage");
	// .IsDependentOn("PublishWeb");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////
RunTarget(target);