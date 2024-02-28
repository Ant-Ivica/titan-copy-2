# +---------------------------------------------------------------------------------------------------------------------
# | File: PostBuildScript.ps1                                          
# | Version: 1.1                          
# | Synopsis: Script launched after TFS build.
# |           This post build script can be customized to create config files and perform deployments.
# +---------------------------------------------------------------------------------------------------------------------
# | Maintenance History                                            
# | -------------------                                            
# | Name            Date        Version     C/R  Description        
# | --------------------------------------------------------------------------------------------------------------------
# | Shun Ochiai     07-13-2016  1.00        Post build script template
# | Padma Lingamaneni     03-05-2018  1.1        Adding Setup to destination
# +---------------------------------------------------------------------------------------------------------------------

Param()

# Declare local variables from TFS environment variable paths
$stagingDir = $env:BUILD_ARTIFACTSTAGINGDIRECTORY
$sourceDir  = $env:BUILD_SOURCESDIRECTORY

# Declare variables
$moduleDirectory    = (Join-Path $sourceDir "PSDeploymentFramework\Modules")
$scriptDirectory    = (Join-Path $sourceDir "PSDeploymentFramework\Scripts")
$frameworkDirectory = (Join-Path $sourceDir "PSDeploymentFramework\DeploymentFramework")
$configDirectory    = $sourceDir + "\Tower\Config\*"
$setupDirectory    = $sourceDir + "\Tower\Setup\*"
$DBDirectory        = $sourceDir + "\DBScripts\Current\*"
$configDestination  = $stagingDir + '\Config\'
$setupDestination   = $stagingDir + '\Setup\'
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
		 if(!(test-path $SetupDestination))
        {
            try
            {
                New-Item $SetupDestination -type Directory -ErrorAction Stop | Out-Null 
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
    
	#Copy the Setup.ini
    Copy-Item -Path $SetupDirectory -Destination $SetupDestination -Recurse

    #Copy the roslyn folder and *.dlls to _PublishedWebsites\FA.LVIS.Tower.UI\bin
    #Move-Item -Path "$stagingDir\roslyn" -Destination "$stagingDir\_PublishedWebsites\FA.LVIS.Tower.UI\bin\" -Force
    Get-ChildItem $stagingDir -Attributes !Directory | Move-Item -Destination "$stagingDir\_PublishedWebsites\FA.LVIS.Tower.UI\bin\" -Force
    
    # Create config script. Use either CreateConfigFiles.ps1 (For generating using CSV) or CreateConfigFiles_SCM.ps1 (For PS 2.0 setup)
    $createConfigScript = (Join-Path $scriptDirectory "PostBuild\CreateConfigFiles.ps1")
    #$createConfigScript = (Join-Path $scriptDirectory "PostBuild\CreateConfigFiles_SCM.ps1")
    & $createConfigScript -StagingDir $stagingDir 

	
}
catch [exception]
{
    throw $_.Exception
}










