# +--------------------------------------------------------------------------------------------------------------------
# | File : DeployProject.ps1
# | Version : 1.02
# | Purpose : Script to generate deployment instructions and start a deployment.
# |           Deployment instructions are generated based on environment information directory this script exists in.
# |           The script exits with exitcode 0 on success and a non-zero value on failure.
# | 
# | Parameters:
# |     StagingDir - Staging directory
# |     Environment - Environment name
# |     ModuleDirectory - Common devops modules directory
# |     ScriptDirectory - Common devops script directory
# |     FrameworkDirectory - Directory of the deployment framework
# |     APPDeployment - Flag to enable or disable creating instruction for app deployment
# |     DBDeployment - Flag to enable or disable creating instruction for db deployment
# |     InteractiveMode - Flag to enable or disable interactive mode for the deployment
# |     Sequential - Flag to enable or disable sequential deployment
# |     SelectRoles - Optional filter to select specific roles to create instructions for APP category deployment
# |     SelectServers - Optional filter to select specific servers to create instructions for APP category deployment
# |     JobMonitor - Optional parameter. Pass a PSObject. The object will be populated with "Jobs", "DeployStatusFilePath", and "Monitor"
# |     RetryCountAPP - Optional parameter for when InteractiveMode&APPDeployment flag is $true. Max number of auto APP deployment retries per step.
# |     AutoContinueDB - Optional parameter for when InteractiveMode&DBDeployment flag is $true. Auto continue DB deployment step on failure.
# | 
# +--------------------------------------------------------------------------------------------------------------------
# | Maintenance History
# | -------------------
# | Name                Date        Version      Description
# | -------------------------------------------------------------------------------------------------------------------
# | Shun Ochiai         06-24-2016   1.00        Creation
# | Shun Ochiai         05-05-2017   1.01        Parameter optimization
# | Shun Ochiai         09-22-2017   1.02        Dynamic parameters for non-interactive deployments
# +--------------------------------------------------------------------------------------------------------------------

Param(
    [Parameter(Mandatory=$true)]
    [String] $StagingDir,
    [Parameter(Mandatory=$true)]
    [string] $Environment,
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
    [Parameter()] 
    [bool] $Sequential  = $false,
    [Parameter(Mandatory=$true, ParameterSetName="SelectFilter")] 
    [Parameter(Mandatory=$true, ParameterSetName="SelectRoles")] 
    [String[]] $SelectRoles,
    [Parameter(Mandatory=$true, ParameterSetName="SelectFilter")] 
    [Parameter(Mandatory=$true, ParameterSetName="SelectServers")] 
    [String[]] $SelectServers,
    [Parameter()]
    [PSObject] $JobMonitor = $null   
)
DynamicParam {
    if ($InteractiveMode -eq $false)
    {   # Hidden dynamic parameters for non-interactive deployments only

        # Create parameter dictionary
        $runtimeParameterDictionary = New-Object System.Management.Automation.RuntimeDefinedParameterDictionary

        if ($APPDeployment -eq $true)
        {   # Add RetryCountAPP dynamic parameter
            $attributeCollection = New-Object System.Collections.ObjectModel.Collection[System.Attribute]
            $parameterAttribute = New-Object System.Management.Automation.ParameterAttribute
            $attributeCollection.Add($parameterAttribute)

            $runtimeParameter = New-Object System.Management.Automation.RuntimeDefinedParameter('RetryCountAPP', [int], $attributeCollection)

            $runtimeParameterDictionary.Add('RetryCountAPP', $runtimeParameter)
        }
        
        if ($DBDeployment -eq $true)
        {   # Add AutoContinueDB dynamic parameter
            $attributeCollection = New-Object System.Collections.ObjectModel.Collection[System.Attribute]
            $parameterAttribute = New-Object System.Management.Automation.ParameterAttribute
            $attributeCollection.Add($parameterAttribute)

            $runtimeParameter = New-Object System.Management.Automation.RuntimeDefinedParameter('AutoContinueDB', [bool], $attributeCollection)

            $runtimeParameterDictionary.Add('AutoContinueDB', $runtimeParameter)
        }

        # Return the dynamic parameter
        return $runtimeParameterDictionary
    }
}
begin 
{
    # Bind the parameter to a friendly variable
    $RetryCountAPP = $PsBoundParameters['RetryCountAPP']
    $AutoContinueDB = $PsBoundParameters['AutoContinueDB']
}
process
{
    # Script variables

    # Variables to pass to Generate Deployment Instructions script
    $generateInsScriptPath    = (Join-path $PSScriptRoot "GenerateDeploymentInstructions.ps1")
    $environmentDirectory     = (Join-path $PSScriptRoot "$Environment`_Configuration")

    # Variables to pass to Deploy Application script
    $customScriptDirectory    = (Join-path $PSScriptRoot "CustomScripts")
    $deployApplicationPath    = (Join-path $FrameworkDirectory "FWScripts\DeployApplication.ps1")
    $loggingDirectory         = (Join-Path $StagingDir "_DeploymentLogs\Deployment_$Environment`_$(Get-Date -Format yyyyMMdd_hhmmss)")
    $deployStatusFilePath     = (Join-Path $StagingDir "_DeploymentCSV\$Environment\CurrentDeployment.csv")

    # Job monitor object
    if ($JobMonitor -ne $null)
    {
        $JobMonitor | Add-Member -MemberType NoteProperty -Name "DeployStatusFilePath" -Value $DeployStatusFilePath -Force
        $JobMonitor | Add-Member -MemberType NoteProperty -Name "Jobs" -Value @() -Force
        $JobMonitor | Add-Member -MemberType NoteProperty -Name "Monitor" -Value $false -Force
    }

    #-------------------------------------------------------------------------------
    # Get deployment instructions

    [System.Array] $deploymentInstructions = @()
    $insGenParams = @{ 
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
    if ($SelectRoles)
    {
        $insGenParams.Add("SelectRoles", $SelectRoles)
    }
    if ($SelectServers)
    {
        $insGenParams.Add("SelectServers", $SelectServers)
    }
    if ($PsBoundParameters.ContainsKey('RetryCountAPP'))
    {
        $insGenParams.Add("RetryCountAPP", $RetryCountAPP)
    }
    if ($PsBoundParameters.ContainsKey('AutoContinueDB'))
    {
        $insGenParams.Add("AutoContinueDB", $AutoContinueDB)
    }

    # Generate instructions
    [System.Array] $deploymentInstructions = & $generateInsScriptPath @insGenParams

    #-------------------------------------------------------------------------------
    # Start deployment

    $allSuccess = & $deployApplicationPath `
                      -DeploymentInstructions $deploymentInstructions `
                      -Sequential $Sequential `
                      -ModuleDirectory $ModuleDirectory `
                      -ScriptDirectory $ScriptDirectory `
                      -FrameworkDirectory $FrameworkDirectory `
                      -CustomScriptDirectory $CustomScriptDirectory `
                      -LoggingDirectory $LoggingDirectory `
                      -DeployStatusFilePath $deployStatusFilePath `
                      -JobMonitor $JobMonitor

    #-------------------------------------------------------------------------------
    # End of deployment
    if ($allSuccess)
    {
        Write-Verbose -Verbose "Deployment successful"
        Exit 0
    }
    else
    {
        Write-Error -Verbose "Deployment failed"
        Exit 1
    }
}