# +--------------------------------------------------------------------------------------------------------------------
# | File : DeployDSC.ps1
# | Version : 1.12
# | Purpose : Script to generate DSC instructions and apply desired state configuration on target environment.
# |           By applying DSC, applications and services can be configured or created.
# |           
# |           Additional configurations can be included by modifying the configuration block.
# | 
# | Note: This script must be launched as admin
# |    
# | Parameters:
# |     StagingDir - Dynamic Parameter for Staging directory when Setup is Full or StageFilesOnly
# |     Setup - Run StageFilesOnly, DscOnly, or Full (both)
# |     Environment - Environment name
# |     ModuleDirectory - Common devops modules directory
# |     ScriptDirectory - Common devops script directory
# |     FrameworkDirectory - Directory of the deployment framework
# |     SelectRoles - Optional filter to select specific roles to create instructions for APP category deployment
# |     SelectServers - Optional filter to select specific servers to create instructions for APP category deployment
# |     ConfigurationMode - Optional parameter to configure LCM mode. Default is ApplyAndAutoCorrect. Is not used when setup is StageFilesOnly
# |     OutputMofDir - Mandatory parameter when setup is OutputMofOnly. Otherwise ignored. Pass location of directory to save MOF Files.
# | 
# | StageFilesOnly - Create target directory and copy files from staging directory if folder was not initially there.
# | DscOnly        - Only apply DSC configuration on the environment.
# | Full           - Apply StageFilesOnly first then apply DscOnly after files have been staged.
# | OutputMofOnly  - Only generate MOF files and store it at OutputMofDir directory location.
# | 
# +--------------------------------------------------------------------------------------------------------------------
# | Maintenance History
# | -------------------
# | Name                Date        Version      Description
# | -------------------------------------------------------------------------------------------------------------------
# | Shun Ochiai         04-10-2017   1.00        Creation
# | Shun Ochiai         04-11-2017   1.01        Modified dynamic parameter so that it can always be passed.
# | Shun Ochiai         04-13-2017   1.02        Added DSC distribution script
# | Shun Ochiai         04-13-2017   1.03        Start DSC replaced with powershell jobs instead of -Wait
# | Shun Ochiai         04-18-2017   1.04        Added job cleanup after receiving job
# | Shun Ochiai         04-19-2017   1.05        Removed configuration mode variable
# | Shun Ochiai         04-20-2017   1.06        Added DSC instruction generator call
# | Shun Ochiai         04-21-2017   1.07        Match DSC ins generator parameters with DSCInstructionGeneratorJSON.ps1
# | Shun Ochiai         04-26-2017   1.08        Moved core functionality to another script.
# | Shun Ochiai         05-05-2017   1.09        Parameter optimization
# | Shun Ochiai         05-16-2017   1.10        Added OutputMofOnly option.
# | Shun Ochiai         10-02-2017   1.11        Added EnableEmail option.
# | Shun Ochiai         10-25-2017   1.12        Added exit code
# +--------------------------------------------------------------------------------------------------------------------

[CmdletBinding(DefaultParameterSetName="All")] 
Param(
    [Parameter()][ValidateSet("Full", "DscOnly", "StageFilesOnly", "OutputMofOnly")]
    [String] $Setup = "Full",
    [Parameter(Mandatory=$true)]
    [string] $Environment,
    [Parameter(Mandatory=$true)]
    [String] $ModuleDirectory,
    [Parameter(Mandatory=$true)]
    [String] $ScriptDirectory,
    [Parameter(Mandatory=$true)]
    [String] $FrameworkDirectory,
    [Parameter(Mandatory=$true, ParameterSetName="SelectFilter")] 
    [Parameter(Mandatory=$true, ParameterSetName="SelectRoles")] 
    [String[]] $SelectRoles,
    [Parameter(Mandatory=$true, ParameterSetName="SelectFilter")] 
    [Parameter(Mandatory=$true, ParameterSetName="SelectServers")] 
    [String[]] $SelectServers,
    [Parameter()] 
    [bool] $EnableEmail = $false,
    [Parameter()] 
    [String] $OutputMofDir = ""
)

DynamicParam {
    # Create parameter dictionary
    $runtimeParameterDictionary = New-Object System.Management.Automation.RuntimeDefinedParameterDictionary
    
    # Add StagingDir dynamic parameter
    $parameterName = 'StagingDir'

    $attributeCollection = New-Object System.Collections.ObjectModel.Collection[System.Attribute]
    $parameterAttribute = New-Object System.Management.Automation.ParameterAttribute

    if ($Setup -eq "Full" -or $Setup -eq "StageFilesOnly")
    {   # Make mandatory only when Full or StageFilesOnly. Otherwise the parameter will be ignored
        $parameterAttribute.Mandatory = $true
    }
    $attributeCollection.Add($parameterAttribute)

    $runtimeParameter = New-Object System.Management.Automation.RuntimeDefinedParameter($parameterName, [string], $attributeCollection)
    $runtimeParameterDictionary.Add($parameterName, $runtimeParameter)

    # Return the dynamic parameter
    return $runtimeParameterDictionary
}

begin {
    # Bind the parameter to a friendly variable
    $StagingDir = $PsBoundParameters["StagingDir"]
}
process {
    try
    {   
        # Variables
        $maxThreads = 20

        # Core DSC deployment script path
        $dscCore = (Join-Path $FrameworkDirectory "DSCExtension\DeployDSCCore.ps1")

        $dscCoreParams = @{
            Setup = $Setup 
            StagingDir = $StagingDir 
            Environment = $Environment 
            FrameworkDirectory = $FrameworkDirectory 
            ModuleDirectory = $ModuleDirectory
            ScriptDirectory = $ScriptDirectory 
            EnvironmentInformationDir = $PSSCriptRoot 
            MaxThreads = $maxThreads
            EnableEmail = $EnableEmail
            OutputMofDir = $OutputMofDir
        }

        # Optional parameters
        if ($PSBoundParameters.ContainsKey('SelectRoles'))
        {
            $dscCoreParams.Add("SelectRoles", $SelectRoles)
        }
        if ($PSBoundParameters.ContainsKey('SelectServers'))
        {
            $dscCoreParams.Add("SelectServers", $SelectServers)
        }

        # Run DSC Core script
        & $dscCore @dscCoreParams

        # Complete DSC
        exit 0
    }
    catch [Exception]
    {
        Write-Error $_.Exception.Message
        exit 1
    }
}

