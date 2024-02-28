# +--------------------------------------------------------------------------------------------------------------------
# | File : GenerateDeploymentInstructions.ps1
# | Version : 1.03
# | Purpose : Create deployment instructions object array
# | 
# +--------------------------------------------------------------------------------------------------------------------
# | Maintenance History
# | -------------------
# | Name                Date        Version      Description
# | -------------------------------------------------------------------------------------------------------------------
# | Shun Ochiai         06-17-2016   1.00        Creation
# | Shun Ochiai         04-14-2017   1.01        Generate from JSON
# | Shun Ochiai         05-05-2017   1.02        Parameter optimization
# | Shun Ochiai         09-25-2017   1.03        Optional RetryCountAPP and AutoContinueDB param added
# | Jay Shin            03-02-2018   1.04        Setup.ini
# +--------------------------------------------------------------------------------------------------------------------

Param(
    [Parameter(Mandatory=$true)]
    [String] $StagingDir,
    [Parameter(Mandatory=$true)]
    [String] $Environment,
    [Parameter()]
    [String] $CustomScriptDirectory = "", # Path to Custom Script Directory
    [Parameter(Mandatory=$true)]
    [String] $EnvironmentDirectory,
    [Parameter(Mandatory=$true)]
    [String] $ModuleDirectory,
    [Parameter(Mandatory=$true)]
    [String] $ScriptDirectory,
    [Parameter(Mandatory=$true)]
    [String] $FrameworkDirectory,
    [Parameter(ParameterSetName="APPDB")]  
    [Parameter(ParameterSetName="SelectRoles")] 
    [Parameter(ParameterSetName="SelectFilter")]
    [Parameter(ParameterSetName="SelectServers")]
    [bool] $APPDeployment = $false,
    [Parameter(ParameterSetName="APPDB")]  
    [Parameter(ParameterSetName="SelectRoles")] 
    [Parameter(ParameterSetName="SelectFilter")]
    [Parameter(ParameterSetName="SelectServers")]
    [bool] $DBDeployment  = $false,
    [Parameter()]
    [bool] $InteractiveMode = $true,
    [Parameter(Mandatory=$true, ParameterSetName="SelectFilter")] 
    [Parameter(Mandatory=$true, ParameterSetName="SelectRoles")] 
    [String[]] $SelectRoles,
    [Parameter(Mandatory=$true, ParameterSetName="SelectFilter")] 
    [Parameter(Mandatory=$true, ParameterSetName="SelectServers")] 
    [String[]] $SelectServers,
    [Parameter()]
    [int] $RetryCountAPP = 0,
    [Parameter()]
    [bool] $AutoContinueDB = $false
)

#-------------------------------------------------------------------------------
Import-Module (Join-Path $ModuleDirectory "FileCommands\IniModule.psm1") -Force
$iniHash = GetIniContent -IniFilePath (Join-Path $StagingDir "Setup\Setup.ini")

# Get selected roles based on Setup.ini (Passing selectRoles through parameter overrides this.)
$setupSelectRoles = @(($iniHash.Tower).GetEnumerator() | Where { $_.Value.Trim() -eq "true" } | Foreach { $_.Name })

if (($PSCmdlet.ParameterSetName -ne "SelectRoles" -and $PSCmdlet.ParameterSetName -ne "SelectFilter")`
     -and $setupSelectRoles.Count -eq 0)
{
    throw "No roles are set to true in Setup.ini. Either deploy without APP or select roles to deploy."
}

#-------------------------------------------------------------------------------
# Generate common deployment instructions
$commonInsGeneratorScript = (Join-Path $FrameworkDirectory "FWScripts\CommonDeploymentInstructionGenerator_JSON.ps1")
$commonInsGenParams = @{ 
    StagingDir = $StagingDir;
    Environment = $Environment;
    EnvironmentDirectory = $environmentDirectory;
    ModuleDirectory = $ModuleDirectory;
    ScriptDirectory = $ScriptDirectory;
    FrameworkDirectory = $FrameworkDirectory;
    APPDeployment = $APPDeployment
    DBDeployment = $DBDeployment;
    InteractiveMode = $InteractiveMode;
}

# Optional parameters
if ($setupSelectRoles)
{
	$commonInsGenParams.Add("SelectRoles", $setupSelectRoles)
}
elseif ($SelectRoles)
{
    $commonInsGenParams.Add("SelectRoles", $SelectRoles)
}
if ($SelectServers)
{
    $commonInsGenParams.Add("SelectServers", $SelectServers)
}
if ($PsBoundParameters.ContainsKey('RetryCountAPP'))
{
    $commonInsGenParams.Add("RetryCountAPP", $RetryCountAPP)
}
if ($PsBoundParameters.ContainsKey('AutoContinueDB'))
{
    $commonInsGenParams.Add("AutoContinueDB", $AutoContinueDB)
}
# Generate common instructions
[System.Array] $deploymentInstructions = & $commonInsGeneratorScript @commonInsGenParams


#-------------------------------------------------------------------------------
# Customize deployment instruction - Modify deployment instructions here

try
{

}
catch
{
    throw "Failed to customize deployment instructions.`nException:$($_.Exception.Message)"
}

#-------------------------------------------------------------------------------
# Return deployment instructions object
return $deploymentInstructions
