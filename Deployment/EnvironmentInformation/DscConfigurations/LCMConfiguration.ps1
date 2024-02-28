# +--------------------------------------------------------------------------------------------------------------------
# | File : LCMConfiguration.ps1
# | Version : 1.02
# | Purpose : Template for LCM configuration
# | 
# | Custom LCM configuration parameters can be included.
# | 
# +--------------------------------------------------------------------------------------------------------------------
# | Maintenance History
# | -------------------
# | Name                Date         Version      Description
# | -------------------------------------------------------------------------------------------------------------------
# | Shun Ochiai         04-13-2017   1.00         Creation
# | Jaime Liu           07-27-2017   1.01         Changed ConfigurationMode to ApplyOnly
# | Jaime Liu           07-27-2017   1.02         Added CertificateId
# +--------------------------------------------------------------------------------------------------------------------

Param(
    [Parameter(Mandatory=$true)]
    [String] $MofDir,
    [Parameter(Mandatory=$true)]
    [PSObject[]] $DSCInstructions
)

[DSCLocalConfigurationManager()]
configuration LCMConfig
{
    Param(
        [Parameter(Mandatory=$true)]
        [PSObject[]] $DSCInstructions
    )

    foreach ($dscIns in $DSCInstructions)
    {
        $server = $dscIns.Server
        Node $server
        {
            Settings
            {
                ConfigurationMode = "ApplyOnly"
                CertificateId = "D78D079C374E6054A6D821825EEF672A681F48BB"
                
            }
        }
    }
} # End of LCMConfig desired state
            
LCMConfig -OutputPath $MofDir -DSCInstructions $DSCInstructions | Out-Null