# +--------------------------------------------------------------------------------------------------------------------
# | File : Deploy_Tower.ps1
# | Version : 1.00
# | Purpose : Template for script to act as interface for deploying project.
# |           This script should be modified and checked-in at the same location as EnvironmentInformation directory for 
# |           the project.
# |          
# | TODO:
# |     1. Rename file name. Replace the ##PROJECTNAME## with the project name  
# |     2. Fill in value for $envInfoPath (Line 61) with the relative path to the EnvironmentInformation directory 
# |        of the project from mapped local tfs path
# |     3. Update Maintenance history and check-in the script to TFS location in EnvironmentInformation directory of the
# |        target project.      
# | 
# | 
# +--------------------------------------------------------------------------------------------------------------------
# | Maintenance History
# | -------------------
# | Name                Date        Version      Description
# | -------------------------------------------------------------------------------------------------------------------
# | Shun Ochiai         07-01-2016   1.00        Template Creation
# | Chris Bilsland      07-29-2016    100.00   Ready for deployment of some application
# +--------------------------------------------------------------------------------------------------------------------

Function Deploy 
{
    <#
	.SYNOPSIS
		Deploy using DeployProject.ps1 and project's environment informations.
        WPF window is launched during the deployment to help manage the deployment status.
	.DESCRIPTION
		Deploy using DeployProject.ps1 and project's environment informations.
        WPF window is launched during the deployment to help manage the deployment status.
        Mapped local tfs path is used to simplify the required parameters.
        Return with exit code number from DeployProject.ps1 script.
    .PARAMETER StagingDir
        The path to the root directory of the build output.
    .PARAMETER Environment
        Name of the target environment.
    .PARAMETER Deploy
        Deploy action. "APP" to perform only APP category deployments. "DB" to perform only DB category deployment.
        "APPandDB" to perform both APP and DB category deployments.
    .PARAMETER Sequential
        Switch parameter to enable sequential deployment mode.
    .PARAMETER TfsPath
        Local mapped tfs path. $Global:MAPPED_TFS_PATH variable is used as default.
    .INPUTS
        String
    .OUTPUTS
        int
	#>
    Param(
        [Parameter(Mandatory=$true)]
        [String] $StagingDir,
        [Parameter(Mandatory=$true)]
        [String] $Environment,
        [Parameter(Mandatory=$true)][ValidateSet("APP", "APPandDB", "DB")]
        [String] $Deploy,
        [Parameter()]
        [Switch] $Sequential,
        [Parameter()]
        [String] $TfsPath = $Global:MAPPED_TFS_PATH
    )
    #------------------------------------------------------------------------------------------------------------------
    # Custom paths - Modify here
    $envInfoPath = "AT_Tower\Deployment\EnvironmentInformation"

    #------------------------------------------------------------------------------------------------------------------
    # Custom variable check
    if ($envInfoPath -eq "")
    {   # Variable not set
        throw ("The function is not yet modified to perform project deployments.`nFill in value inside `$envInfoPath variable.")
    }

    #------------------------------------------------------------------------------------------------------------------
    # Variables
    $jobMonitor = New-Object PSObject
    $deployProjectScriptPath = (Join-Path $TfsPath (Join-Path $envInfoPath "DeployProject.ps1"))
    $jobMonitorScriptPath    = (Join-Path $TfsPath "AT_DevOps\Automation\Powershell\Valid\Tools\DeploymentTools\DeploymentMonitorWindow.ps1")
    $moduleDirectory         = (Join-Path $TfsPath "AT_DevOps\Automation\Powershell\Valid\Modules") 
    $scriptDirectory         = (Join-Path $TfsPath "AT_DevOps\Automation\Powershell\Valid\Scripts") 
    $frameworkDirectory      = (Join-Path $TfsPath "AT_DevOps\Automation\Powershell\Valid\DeploymentFramework") 
    $DBUpdate = $false
    $APPUpdate = $true

    #------------------------------------------------------------------------------------------------------------------
    # Params
    if ($Deploy -eq "APPandDB")
    {
        $DBUpdate = $true
    }

    if ($Deploy -eq "DB")
    {
        $DBUpdate = $true
        $APPUpdate = $false 
    }

    #------------------------------------------------------------------------------------------------------------------
    # Job monitor scriptblock
    $scriptBlock = {
        Param(
            [Parameter(Mandatory=$true)]
            [String] $JobMonitorScriptPath,
            [Parameter()]
            [PSObject] $JobMonitor,
            [Parameter(Mandatory=$true)]
            [String] $ModuleDirectory,
            [Parameter(Mandatory=$true)]
            [String] $FrameworkDirectory         
        )

        & $JobMonitorScriptPath `
            -JobMonitor $JobMonitor `
            -ModuleDirectory $ModuleDirectory `
            -FrameworkDirectory $FrameworkDirectory
    }

    #------------------------------------------------------------------------------------------------------------------
    # Import runspace module
    Import-Module (Join-Path $moduleDirectory "BackgroundJobs/RunspaceJobs.psm1") -Force -ErrorAction Stop

    # Start job monitor process
    $runSP = CreateRunspace
    $jobP  = StartRunspaceJob -Name "MonitorJob" -Runspace $runSP -ScriptBlock $scriptBlock `
               -Parameters  @{ JobMonitorScriptPath = $jobMonitorScriptPath; `
                               JobMonitor = $jobMonitor; `
                               ModuleDirectory = $moduleDirectory; `
                               FrameworkDirectory = $frameworkDirectory }

    #------------------------------------------------------------------------------------------------------------------
    # Start deployment
    & $deployProjectScriptPath  `
        -StagingDir $StagingDir `
        -Environment $Environment `
        -ModuleDirectory $moduleDirectory `
        -ScriptDirectory $scriptDirectory `
        -FrameworkDirectory $frameworkDirectory `
        -APPDeployment $APPUpdate `
        -DBDeployment $DBUpdate `
        -InteractiveMode $true  `
        -Sequential $Sequential.IsPresent `
        -JobMonitor $JobMonitor

    #------------------------------------------------------------------------------------------------------------------
    # Return result exitcode
    return $LASTEXITCODE
}