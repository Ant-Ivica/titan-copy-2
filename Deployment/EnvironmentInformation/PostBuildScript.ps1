# +---------------------------------------------------------------------------------------------------------------------
# | File: PostBuildScript.ps1                                          
# | Version: 1.00                          
# | Synopsis: Script launched after TFS build.
# |           This post build script can be customized to create config files and perform deployments.
# +---------------------------------------------------------------------------------------------------------------------
# | Maintenance History                                            
# | -------------------                                            
# | Name            Date        Version     C/R  Description        
# | --------------------------------------------------------------------------------------------------------------------
# | Shun Ochiai     07-13-2016  1.00        Post build script template
# +---------------------------------------------------------------------------------------------------------------------

Param()

# Declare local variables from TFS environment variable paths
$stagingDir = $env:TF_BUILD_BINARIESDIRECTORY
$sourceDir  = $env:TF_BUILD_SOURCESDIRECTORY

# Declare variables
$moduleDirectory    = (Join-Path $sourceDir "PSDeploymentFramework\Modules")
$scriptDirectory    = (Join-Path $sourceDir "PSDeploymentFramework\Scripts")
$frameworkDirectory = (Join-Path $sourceDir "PSDeploymentFramework\DeploymentFramework")
$configDirectory    = $sourceDir + "\Tower\Config\*"
$DBDirectory        = $sourceDir + "\DBScripts\*"
$configDestination  = $stagingDir + '\Config\'
$DBDestination      = $stagingDir + '\_Database\'
$APPDeployment   = $true
$DBDeployment    = $false
$InteractiveMode = $false # This needs to stay false for post build script 
$Sequential      = $false

try
{ 
    # Check and create folders Config and DBScript
    if(!(test-path $configDestination))
        {
            try
            {
                New-Item $configDestination -type Directory -ErrorAction Stop | Out-Null 
            }
            catch [exception]
            {
                $failMessage = "$($_.Exception.ToString()). $($_.InvocationInfo.PositionMessage)" + "Could not create ResourceFiles directory."
                return $failMessage
            }
        }
      if(!(test-path $DBDestination))
        {
            try
            {
                New-Item $DBDestination -type Directory -ErrorAction Stop | Out-Null 
            }
            catch [exception]
            {
                $failMessage = "$($_.Exception.ToString()). $($_.InvocationInfo.PositionMessage)" + "Could not create ResourceFiles directory."
                return $failMessage
            }
        }

	#Copy the Config Files
    Copy-item -Path $configDirectory -Destination $configDestination -Recurse

	#Copy the DBScripts
    Copy-Item -Path $DBDirectory -Destination $DBDestination -Recurse
    
    #Copy the roslyn folder and *.dlls to _PublishedWebsites\FA.LVIS.Tower.UI\bin
    #Move-Item -Path "$stagingDir\roslyn" -Destination "$stagingDir\_PublishedWebsites\FA.LVIS.Tower.UI\bin\" -Force
    Get-ChildItem $stagingDir -Attributes !Directory | Move-Item -Destination "$stagingDir\_PublishedWebsites\FA.LVIS.Tower.UI\bin\" -Force
    
    # Create config script. Use either CreateConfigFiles.ps1 (For generating using CSV) or CreateConfigFiles_SCM.ps1 (For PS 2.0 setup)
    $createConfigScript = (Join-Path $scriptDirectory "PostBuild\CreateConfigFiles.ps1")
    #$createConfigScript = (Join-Path $scriptDirectory "PostBuild\CreateConfigFiles_SCM.ps1")
    & $createConfigScript -StagingDir $stagingDir 

	<#
    # Deploy Script - Edit the project name <PROJNAME> and edit the Environment name <ENV>
    $envName = '<ENV>'
    $projName = '<PROJNAME>'
    if ($envName -ne '<ENV>' -and $projName -ne '<PROJNAME>')
    { 
        $deployProjectScript = (Join-Path $sourceDir "\$projName\Deployment\EnvironmentInformation\DeployProject.ps1")
        & $deployProjectScript -StagingDir $stagingDir `
            -Environment $envName `
            -ModuleDirectory $moduleDirectory `
            -ScriptDirectory $scriptDirectory `
            -FrameworkDirectory $frameworkDirectory `
            -APPDeployment $APPDeployment `
            -DBDeployment $DBDeployment `
            -InteractiveMode $InteractiveMode `
            -Sequential $Sequential   
    }
    #>
}
catch [exception]
{
    throw $_.Exception
}










