$framework = '4.5'
$script:project_config = "Debug"

properties {
	$project_name = "WebStarter"

	$base_dir = resolve-path .
	$build_dir = "$base_dir\build"
	$package_dir = "$build_dir\latestVersion"
	$package_file = "$package_dir\" + $project_name + ".zip"   
	$source_dir = "$base_dir\src"
	$test_dir = "$build_dir\test"
	$result_dir = "$build_dir\results"

	$web_proj = "$source_dir\ApiServer\ApiServer.csproj"
	$publish_dir = "$build_dir\publish"

	$db_scripts_dir = "$base_dir\..\Deployment\SqlScripts"
    
    $db_server = ".\SqlExpress"
    $db_user = "sa"
    $db_pass = "S2n3lib"
	$db_name = "WebStarter"	
    $scriptTableName = "usd_AppliedDatabaseScript"
	
	$test_assembly_patterns = @("UnitTests.dll", "IntegrationTests.dll")
}

task default -depends privateBuild
task privateRebuild -depends Clean, CommonAssemblyInfo, DropDatabase, CreateOrApplyDatabaseSchema, CreateQueue, Compile, Test, DataLoader
task privateBuild -depends Clean, CommonAssemblyInfo, CreateOrApplyDatabaseSchema, CreateQueue, Compile, Test, DataLoader
task publish -depends Clean, CommonAssemblyInfo, Compile, PublishWeb
task createDatabase -depends DropDatabase, CreateOrApplyDatabaseSchema, DataLoader
task dataLoad -depends DataLoader
task scAction -depends SCTask
task createMsgQueues -depends CreateQueue

task PublishWeb {
	MSBuild-Publish-Web
}

task SCTask {
    HgSync
}

task Clean {
	write-host "Start Cleanup"
	
	delete_file $package_file
	delete_directory $build_dir
	create_directory $test_dir 
	create_directory $result_dir
	
	create_directory $publish_dir
		
	set-location $source_dir
	get-childitem * -include *.dll -recurse 
	get-childitem * -include *.pdb -recurse 
	get-childitem * -include *.exe -recurse 
	set-location $base_dir
	
	# Clean both Release and Debug configs to avoid file overrides from Release config when doing Debug config builds
	exec { msbuild /t:clean /v:q /p:VisualStudioVersion=12.0 /p:Configuration=Debug /p:Platform="Any CPU" $source_dir\$project_name.sln }
	exec { msbuild /t:clean /v:q /p:VisualStudioVersion=12.0 /p:Configuration=Release /p:Platform="Any CPU" $source_dir\$project_name.sln }
	
	write-host "Cleanup done"
}

task CommonAssemblyInfo {
	create-commonAssemblyInfo $project_name "$source_dir\CommonAssemblyInfo.cs"
}

task DropDatabase {
	drop-database
}

task CreateOrApplyDatabaseSchema {
    manage-application-database
}

task Compile { 
	exec { msbuild.exe /t:build /v:q /detailedsummary /p:VisualStudioVersion=12.0 /p:Configuration=$project_config /nologo $source_dir\$project_name.sln }
}

task Test {
	copy_all_assemblies_for_test $test_dir    
	$test_assembly_patterns | %{ run_tests $_ }
}

task DataLoader {    
    insert-data-using-dataloader
}

task CreateQueue{
    create-nservicebus-messaging-queues
}

# -------------------------------------------------------------------------------------------------------------
# generalized functions 
# --------------------------------------------------------------------------------------------------------------
function HgSync
{
	$summary = invoke-expression "hg pull -u" 
	$summary
}

function GetHgVersion
{
    $result = "hg summary"
    $re = Invoke-Expression $result         
    $ver = [int]$re.Split(":")[1].Trim()
    $minor = $ver % 100
    $mejor = ($ver - $minor) /100
    return "1.$mejor.$minor" 
}


#
# Drop Database
#
function drop-database
{
    
    [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO')  | out-null
    $SMOserver = New-Object ('Microsoft.SqlServer.Management.Smo.Server') -argumentlist $db_server
            
    if ($SMOserver.Databases[$db_name] -ne $null)
    {   
        "Database exist. Droping $db_name"

        $query="USE master
                
                DECLARE @dbname nvarchar(128)
                SET @dbname = N'$db_name'
                IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = @dbname OR name = @dbname)))
                BEGIN	                
	                ALTER DATABASE $db_name SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE
	                DROP DATABASE $db_name
                END"

       run-executeNonQuery $query
    } 
}

#
# Create database
#
function create-database
{

    "Creating database $db_name"

    $SMOserver = new-object ('Microsoft.SqlServer.Management.Smo.Server') $db_server 

    # Instantiate the database object and add the filegroups
    $db = new-object ('Microsoft.SqlServer.Management.Smo.Database') ($SMOserver, $db_name)

    # Create the database
    $db.Create()
}

#
# Manage application database
#
function manage-application-database() {	
        
    #Creates a new database using our specifications
    [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO')  | out-null
    
    $SMOserver = New-Object ('Microsoft.SqlServer.Management.Smo.Server') -argumentlist $db_server

    #create SMO handle to your database
    $db = $SMOserver.Databases[$db_name]

    if ($db -eq $null)  
    {
        create-database
    }

    $db = $SMOserver.Databases[$db_name]

    #create usd table is not exist
    $table = $db.Tables[$scriptTableName]
    if($table -eq $null)
    {
        $tb = new-object Microsoft.SqlServer.Management.Smo.Table($db, "usd_AppliedDatabaseScript")
        $col1 = new-object Microsoft.SqlServer.Management.Smo.Column($tb,"ScriptFile", [Microsoft.SqlServer.Management.Smo.DataType]::NVarChar(255))
        $col2 = new-object Microsoft.SqlServer.Management.Smo.Column($tb, "DateApplied", [Microsoft.SqlServer.Management.Smo.DataType]::DateTime)
        $col3 = new-object Microsoft.SqlServer.Management.Smo.Column($tb, "Version", [Microsoft.SqlServer.Management.Smo.DataType]::Int)
        $tb.Columns.Add($col1)
        $tb.Columns.Add($col2)
        $tb.Columns.Add($col3)
        $tb.Create()
    }

    $connection = new-object system.data.sqlclient.sqlconnection("data source=$($db_server);initial catalog=$($db_name);User Id=$($db_user); Password=$($db_Pass)")
    $Command = New-Object System.Data.SQLClient.SQLCommand
    $Command.Connection=$connection
    
    $connection.Open()                    
    $Command.CommandText="SELECT ISNULL(MAX(Version), 0) FROM $scriptTableName"
    $lastScriptVersion = $Command.ExecuteScalar()                                      
    $connection.Close()    

	if(Test-Path "C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS") 
	{ 
		#Sql Server 2012
		Import-Module SqlPs -DisableNameChecking
		C: # Switch back from SqlServer
	} 
	else 
	{ 
		#Sql Server 2008
		Add-PSSnapin SqlServerCmdletSnapin100
        Add-PSSnapin SqlServerProviderSnapin100
	}

    $fileEntries = [IO.Directory]::GetFiles($db_scripts_dir);
    foreach($fileName in $fileEntries) 
    {   
        $scriptFileName = [System.IO.Path]::GetFileName($fileName)
        Write-Host "File : " $scriptFileName
             
        if($scriptFileName.Contains("_"))
        {                 
            $splitVersion = $scriptFileName.Split("_")
            
            $version=[int]$splitVersion[0]                                      

            if($version -le $lastScriptVersion)
            {
                "Skiping File : " + $fileName    
                continue
            }

            $lastScriptVersion = $version            
            
            Write-Host "Executing"   
            Invoke-Sqlcmd –ServerInstance .\sqlExpress –Database $db_name -InputFile $fileName
            
            run-executeNonQuery "INSERT INTO $scriptTableName (ScriptFile, DateApplied) VALUES ('$scriptFileName','$(Get-Date)')"
        }                   
    }

    Write-Host "Setting db version:" $lastScriptVersion

    run-executeNonQuery "UPDATE $scriptTableName SET Version = $lastScriptVersion where Version is null"
}


#
# DataLoader
#
function global:insert-data-using-dataloader
{
    Write-Host "Executing" "$db_scripts_dir\Dataloader.sql"
    Invoke-Sqlcmd –ServerInstance .\sqlExpress –Database $db_name -InputFile "$db_scripts_dir\Dataloader.sql"
}

#
# Query execute with ExecuteNonQuery
#
function run-executeNonQuery ($commandText)
{    
    $connection = new-object system.data.sqlclient.sqlconnection("data source=$($db_server);initial catalog=$($db_name);User Id=$($db_user); Password=$($db_Pass)")
    $Command = New-Object System.Data.SQLClient.SQLCommand
    
    $connection.Open()      
    $Command.Connection=$connection
    $Command.CommandText= $commandText
    $Command.ExecuteNonQuery()
    $connection.Close()
}

#
# Delete Files
#
function global:delete_file($file) {
	if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null } 
}

#
# Delete Directory
#
function global:delete_directory($directory_name) {
	rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

#
# Create Directory
#
function global:create_directory($directory_name) {
	mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function global:Copy_and_flatten ($source,$filter,$dest) {    
    set-location $base_dir
	ls $source -filter $filter -r | cp -dest $dest
}

#
# Copy all assemblies for test
#
function global:copy_all_assemblies_for_test($destination){    
	create_directory $destination
	
	Copy_and_flatten $source_dir *.exe $destination               
	Copy_and_flatten $source_dir *.dll $destination
	Copy_and_flatten $source_dir *.config $destination
	Copy_and_flatten $source_dir *.pdb $destination

	create_directory $destination\Configs
	Copy_and_flatten $source_dir *Config.xml $destination\Configs
}

function run_tests([string]$pattern) {

	"Test Directory: " + $test_dir
	"Assembly Names: " + $pattern      
	
	$items = Get-ChildItem -Path $test_dir $pattern
	$items | %{ run_nunit $_.Name }
}

function global:run_nunit ($test_assembly) {   
	$assembly_to_test = $test_dir + "\" + $test_assembly
	$results_output = $result_dir + "\" + $test_assembly + ".xml"
	write-host "Running NUnit Tests in: " $test_assembly
	exec { & tools\nunit\nunit-console-x86.exe $assembly_to_test /nologo /nodots /xml=$results_output /exclude=DataLoader}
}

#
# Create CommonAssemblyInfo
#
function global:create-commonAssemblyInfo($applicationName,$filename) {
    $hgVersion = "1.0"

    "using System.Reflection;
     using System.Runtime.InteropServices;

     //------------------------------------------------------------------------------
     // <auto-generated>
     //     This code was generated by a tool.
     //     Runtime Version:4.5.1
     //
     //     Changes to this file may cause incorrect behavior and will be lost if
     //     the code is regenerated.
     // </auto-generated>
     //------------------------------------------------------------------------------

     [assembly: ComVisibleAttribute(false)]
     [assembly: AssemblyVersionAttribute(""$hgVersion"")]
     [assembly: AssemblyFileVersionAttribute(""$hgVersion"")]
     [assembly: AssemblyCopyrightAttribute(""Sanelib, LLC. Copyright 2012"")]
     [assembly: AssemblyProductAttribute(""$applicationName"")]
     [assembly: AssemblyCompanyAttribute(""Sanelib, LLC"")]
     [assembly: AssemblyConfigurationAttribute(""$script:project_config"")]
     [assembly: AssemblyInformationalVersionAttribute(""$hgVersion"")]"  | out-file $filename -encoding "ASCII"    
}


function MSBuild-Publish-Web
{
    Write-Host "Publishing web from $web_proj"
	
	$res = msbuild "$web_proj" /verbosity:minimal /p:VisualStudioVersion=12.0 "/t:ResolveReferences;_CopyWebApplication;publish" /p:Configuration=Debug /p:OutDir="$publish_dir\bin\" /p:WebProjectOutputDir="$publish_dir"

    if ($res.Contains("error")) { throw $res }
    
	exec { & ..\tools\7zip\7za.exe a -pS2n3lib -r -mx9 $build_dir\publish.7z $publish_dir }

    Write-Host "Web published"
}

#
# Create Messageing MSMQ Queues for NServiceBus
#

function create-nservicebus-messaging-queues{
    [Reflection.Assembly]::LoadWithPartialName( "System.Messaging" )
    $msmq = [System.Messaging.MessageQueue]

    $queueList = ("$project_name.BUS", "$project_name.BUS.retries", "$project_name.BUS.timeouts", "$project_name.BUS.timeoutsdispatcher", "$project_name.API", "$project_name.API.retries", "$project_name.API.timeouts", "$project_name.API.timeoutsdispatcher", "audit")
   
    foreach($queueName in $queueList)
    {
    	$queueName = ".\private$\" + $queueName
    	if($msmq::Exists($queueName))
        {  
            $msmq::Delete($queueName) 
        }
    	
    	$queue = $msmq::Create($queueName, 1)
    	$queue.UseJournalQueue = $TRUE	
    	$queue.MaximumJournalSize = 1024 #kilobytes
    	$queue.SetPermissions("Everyone", [System.Messaging.MessageQueueAccessRights]::FullControl, [System.Messaging.AccessControlEntryType]::Set)
    }
}