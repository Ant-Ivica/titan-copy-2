# +--------------------------------------------------------------------------------------------------------------------
# | File : PushDSC_LVIS.ps1
# | Version : 1.03 
# | Purpose : Template for script to make pushing DSC to target environment easier for user.
# |           This script should be modified and checked-in at the same location as EnvironmentInformation directory for 
# |           the project.
# |          
# | TODO:
# |     1. Rename file name. Replace the ##PROJECTNAME## with the project name  
# |     2. Fill in value for $envInfoPath (Line 84) with the relative path to the EnvironmentInformation directory 
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
# | Shun Ochiai         04-14-2017   1.00        Template Creation
# | Gunasekaran V       01-15-2018   1.01        Updated for AT_myFAMS
# +--------------------------------------------------------------------------------------------------------------------

Function PushDSC 
{
    <#
	.SYNOPSIS
		Push DSC configuration using DeployDSC.ps1 and project's environment informations.
	.DESCRIPTION
		Deploy using DeployProject.ps1 and project's environment informations.
        Mapped local tfs path is used to simplify the required parameters.
    .PARAMETER Setup
        Run StageFilesOnly, DscOnly, or Full (both)
        Full and StageFilesOnly requires passing in parameter to StagingDir
    .PARAMETER StagingDir
        The path to the root directory of the build output. Mandatory for StageFilesOnly and Full setup.
    .PARAMETER Environment
        Name of the target environment.
    .PARAMETER TfsPath
        Local mapped tfs path. $Global:MAPPED_TFS_PATH variable is used as default.
    .PARAMETER EnableEmail
        Enable email notification and setup is DSCOnly or Full. Completion email enabled by default.
    .INPUTS
        String
    .OUTPUTS
        None
	#>
    Param(
        [Parameter(Mandatory=$true)][ValidateSet("Full", "DscOnly", "StageFilesOnly", "OutputMofOnly")]
        [String] $Setup,
        [Parameter(Mandatory=$true)]
        [String] $Environment,
        [Parameter()]
        [String] $TfsPath = $Global:MAPPED_TFS_PATH,
        [Parameter()] 
        [bool] $EnableEmail = $true,
        [Parameter()] 
        [String[]] $SelectRoles,
        [Parameter()] 
        [String[]] $SelectServers
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

        # Add OutputMofDir dynamic parameter
        $parameterName = 'OutputMofDir'
        $attributeCollection = New-Object System.Collections.ObjectModel.Collection[System.Attribute]
        $parameterAttribute = New-Object System.Management.Automation.ParameterAttribute
        if ($Setup -eq "OutputMofOnly")
        {   # Make mandatory only when OutputMofOnly. Otherwise the parameter will be ignored
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
        $OutputMofDir = $PsBoundParameters["OutputMofDir"]
    }
    process {
        #--------------------------------------------------------------------------------------------------------------
        # Custom paths - Modify here
        $envInfoPath = "AT_Tower\Deployment\EnvironmentInformation"

        #--------------------------------------------------------------------------------------------------------------
        # Custom variable check
        if ($envInfoPath -eq "")
        {   # Variable not set
            throw ("The function is not yet modified to perform project deployments.`nFill in value inside `$envInfoPath variable.")
        }

        #--------------------------------------------------------------------------------------------------------------
        # Variables
        $deployDscScriptPath     = (Join-Path $TfsPath (Join-Path $envInfoPath "DeployDSC.ps1"))
        $moduleDirectory         = (Join-Path $TfsPath "AT_DevOps\Automation\Powershell\Valid\Modules") 
        $scriptDirectory         = (Join-Path $TfsPath "AT_DevOps\Automation\Powershell\Valid\Scripts") 
        $frameworkDirectory      = (Join-Path $TfsPath "AT_DevOps\Automation\Powershell\Valid\DeploymentFramework") 

        if ($StagingDir -eq $null)
        {
            $StagingDir = ""
        }

        $deployDSCParams = @{
            Setup = $Setup 
            Environment = $Environment 
            ModuleDirectory = $ModuleDirectory
            ScriptDirectory = $ScriptDirectory 
            FrameworkDirectory = $FrameworkDirectory
            StagingDir = $StagingDir
            OutputMofDir = $OutputMofDir
            EnableEmail = $EnableEmail
        }

        # Optional parameters
        if ($PSBoundParameters.ContainsKey('SelectRoles'))
        {
            $deployDSCParams.Add("SelectRoles", $SelectRoles)
        }
        if ($PSBoundParameters.ContainsKey('SelectServers'))
        {
            $deployDSCParams.Add("SelectServers", $SelectServers)
        }

        # Run DeployDSC script
        & $deployDscScriptPath @deployDSCParams
    }
}