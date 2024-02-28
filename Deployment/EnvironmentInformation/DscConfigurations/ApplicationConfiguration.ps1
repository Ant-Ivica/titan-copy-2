# +--------------------------------------------------------------------------------------------------------------------
# | File : ApplicationConfiguration.ps1
# | Version : 1.01
# | Purpose : Template for Application Configuration
# | 
# | Custom DSC resources can be included.
# | 
# +--------------------------------------------------------------------------------------------------------------------
# | Maintenance History
# | -------------------
# | Name                Date         Version      Description
# | -------------------------------------------------------------------------------------------------------------------
# | Shun Ochiai         04-13-2017   1.00         Creation
# | Gunasekaran V       01-15-2018   1.01         Updated Win Features for AT_myFAMS
# +--------------------------------------------------------------------------------------------------------------------

Param(
    [Parameter(Mandatory=$true)]
    [String] $MofDir,
    [Parameter(Mandatory=$true)]
    [PSObject[]] $DSCInstructions
)

configuration ApplicationConfiguration
{   
    Param(
        [Parameter(Mandatory=$true)]
        [PSObject[]] $DSCInstructions
    )
    Import-DscResource -ModuleName DevOps_CompositeResources
    Import-DscResource -ModuleName 'PSDesiredStateConfiguration'

    foreach ($dscIns in $DSCInstructions)
    {
        $server = $dscIns.Server

        Node $server
        {
            # Common configuration
            if (@(("TOWERUI") | Where { $dscIns.Roles -contains $_ }).Count -gt 0)
            {
                # Application Server
                <#WindowsFeature ensureNetFramework45
                {
                    Ensure = "Present"
                    Name = "AS-NET-Framework"
                }
                WindowsFeature ensureWebIISSupport
                {
                    Ensure = "Present"
                    Name = "AS-Web-Support"
                    #DependsOn = "[WindowsFeature]ensureNetFramework45"
                }#>
                # File And Storage
                WindowsFeature ensureFileServer
                {
                    Ensure = "Present"
                    Name = "FS-FileServer"
                    #DependsOn = "[WindowsFeature]ensureWebIISSupport"
                }
                WindowsFeature ensureStorageServices
                {
                    Ensure = "Present"
                    Name = "Storage-Services"
                    DependsOn = "[WindowsFeature]ensureFileServer"
                }
                # IIS
                WindowsFeature ensureWebDefaultDoc
                {
                    Ensure = "Present"
                    Name = "Web-Default-Doc"
                    DependsOn = "[WindowsFeature]ensureStorageServices"
                }
                WindowsFeature ensureWebDirBrowsing
                {
                    Ensure = "Present"
                    Name = "Web-Dir-Browsing"
                    DependsOn = "[WindowsFeature]ensureWebDefaultDoc"
                }
                WindowsFeature ensureWebHttpErrors
                {
                    Ensure = "Present"
                    Name = "Web-Http-Errors"
                    DependsOn = "[WindowsFeature]ensureWebDirBrowsing"
                }
                WindowsFeature ensureWebStaticContent
                {
                    Ensure = "Present"
                    Name = "Web-Static-Content"
                    DependsOn = "[WindowsFeature]ensureWebHttpErrors"
                }
                WindowsFeature ensureWebHttpRedirect
                {
                    Ensure = "Present"
                    Name = "Web-Http-Redirect"
                    DependsOn = "[WindowsFeature]ensureWebStaticContent"
                }
                WindowsFeature ensureWebHttpLogging
                {
                    Ensure = "Present"
                    Name = "Web-Http-Logging"
                    DependsOn = "[WindowsFeature]ensureWebHttpRedirect"
                }
                WindowsFeature ensureWebCustomLog
                {
                    Ensure = "Present"
                    Name = "Web-Custom-Logging"
                    DependsOn = "[WindowsFeature]ensureWebHttpLogging"
                }
                WindowsFeature ensureWebLogTools
                {
                    Ensure = "Present"
                    Name = "Web-Log-Libraries"
                    DependsOn = "[WindowsFeature]ensureWebCustomLog"
                }
                WindowsFeature ensureWebReqMonitor
                {
                    Ensure = "Present"
                    Name = "Web-Request-Monitor"
                    DependsOn = "[WindowsFeature]ensureWebLogTools"
                }
                WindowsFeature ensureWebTracing
                {
                    Ensure = "Present"
                    Name = "Web-Http-Tracing"
                    DependsOn = "[WindowsFeature]ensureWebReqMonitor"
                }
                WindowsFeature ensureWebPerformance
                {
                    Ensure = "Present"
                    Name = "Web-Performance"
                    DependsOn = "[WindowsFeature]ensureWebTracing"
                }
                WindowsFeature ensureWebFiltering
                {
                    Ensure = "Present"
                    Name = "Web-Filtering"
                    DependsOn = "[WindowsFeature]ensureWebPerformance"
                }
                WindowsFeature ensureWebBasicAuth
                {
                    Ensure = "Present"
                    Name = "Web-Basic-Auth"
                    DependsOn = "[WindowsFeature]ensureWebFiltering"
                }
                WindowsFeature ensureWebClientCert
                {
                    Ensure = "Present"
                    Name = "Web-Client-Auth"
                    DependsOn = "[WindowsFeature]ensureWebBasicAuth"
                }
                WindowsFeature ensureWebDigestAuth
                {
                    Ensure = "Present"
                    Name = "Web-Digest-Auth"
                    DependsOn = "[WindowsFeature]ensureWebClientCert"
                }
                WindowsFeature ensureWebIISClientCert
                {
                    Ensure = "Present"
                    Name = "Web-Cert-Auth"
                    DependsOn = "[WindowsFeature]ensureWebDigestAuth"
                }
                WindowsFeature ensureWebIPDomainRest
                {
                    Ensure = "Present"
                    Name = "Web-IP-Security"
                    DependsOn = "[WindowsFeature]ensureWebIISClientCert"
                }
                WindowsFeature ensureWebURLAuth
                {
                    Ensure = "Present"
                    Name = "Web-Url-Auth"
                    DependsOn = "[WindowsFeature]ensureWebIPDomainRest"
                }
                WindowsFeature ensureWebWinAuth
                {
                    Ensure = "Present"
                    Name = "Web-Windows-Auth"
                    DependsOn = "[WindowsFeature]ensureWebURLAuth"
                }
                WindowsFeature ensureWebNetExt45
                {
                    Ensure = "Present"
                    Name = "Web-Net-Ext45"
                    DependsOn = "[WindowsFeature]ensureWebWinAuth"
                }
                WindowsFeature ensureWebASP45
                {
                    Ensure = "Present"
                    Name = "Web-Asp-Net45"
                    DependsOn = "[WindowsFeature]ensureWebNetExt45"
                }
                WindowsFeature ensureISAPIExt
                {
                    Ensure = "Present"
                    Name = "Web-ISAPI-Ext"
                    DependsOn = "[WindowsFeature]ensureWebASP45"
                }
                WindowsFeature ensureISAPIFilt
                {
                    Ensure = "Present"
                    Name = "Web-ISAPI-Filter"
                    DependsOn = "[WindowsFeature]ensureISAPIExt"
                }
                WindowsFeature ensureIISMgmtTools
                {
                    Ensure = "Present"
                    Name = "Web-Mgmt-Tools"
                    DependsOn = "[WindowsFeature]ensureISAPIFilt"
                }
                WindowsFeature ensureWASProcessModel
                {
                    Ensure = "Present"
                    Name = "WAS-Process-Model"
                    DependsOn = "[WindowsFeature]ensureIISMgmtTools"
                }
                WindowsFeature ensureWASConfigAPIs
                {
                    Ensure = "Present"
                    Name = "WAS-Config-APIs"
                    DependsOn = "[WindowsFeature]ensureWASProcessModel"
                }
                WindowsFeature ensureNETWCFServices45
                {
                    Ensure = "Present"
                    Name = "NET-WCF-Services45"
                    DependsOn = "[WindowsFeature]ensureWASConfigAPIs"
                }
                WindowsFeature ensureNETWCFHTTPActivation45
                {
                    Ensure = "Present"
                    Name = "NET-WCF-HTTP-Activation45"
                    DependsOn = "[WindowsFeature]ensureNETWCFServices45"
                }
                WindowsFeature ensureNETWCFMSMQActivation45
                {
                    Ensure = "Present"
                    Name = "NET-WCF-MSMQ-Activation45"
                    DependsOn = "[WindowsFeature]ensureNETWCFHTTPActivation45"
                }
                WindowsFeature ensureNETWCFPipeActivation45
                {
                    Ensure = "Present"
                    Name = "NET-WCF-Pipe-Activation45"
                    DependsOn = "[WindowsFeature]ensureNETWCFMSMQActivation45"
                }
                WindowsFeature ensureNETWCFTCPActivation45
                {
                    Ensure = "Present"
                    Name = "NET-WCF-TCP-Activation45"
                    DependsOn = "[WindowsFeature]ensureNETWCFPipeActivation45"
                }
                WindowsFeature ensureNETWCFTCPPortSharing45
                {
                    Ensure = "Present"
                    Name = "NET-WCF-TCP-PortSharing45"
                    DependsOn = "[WindowsFeature]ensureNETWCFTCPActivation45"
                }   
                DevOps_ServerConfiguration "$server`_serverConfig"
                {   
                    DSCInstruction = $dscIns
                    DependsOn = "[WindowsFeature]ensureNETWCFTCPPortSharing45"
                }
            }
            else
            {
                # Composite Resource configuration
                DevOps_ServerConfiguration "$server`_serverConfig"
                {   
                    DSCInstruction = $dscIns
                }
            }
        }
    }
} # End of ApplicationConfiguration configuration

 
$ConfigurationData = @{
    AllNodes = @(
    @{
        NodeName="*"
        CertificateFile="C:\Certificate\DevOps_Cert_DSCDeploy.cer"
        PSDscAllowDomainUser=$true
        Thumbprint="D78D079C374E6054A6D821825EEF672A681F48BB"
    }
    foreach ($dscIns in $DSCInstructions)
    {
        @{
            NodeName=$dscIns.Server
        }
    }
    )
}

# Create Mof file
ApplicationConfiguration -OutputPath $MofDir -DSCInstructions $DSCInstructions -ConfigurationData $configurationData | Out-Null