import-module WebAdministration

Remove-WebAppPool -name imagetoolpools -erroraction 'silentlycontinue'
Remove-WebApplication -site "Default Web Site" -name imagetool   -erroraction 'silentlycontinue'

rd -path c:\inetpub\wwwroot\imagetool -erroraction 'silentlycontinue'

new-item -itemtype directory -path c:\inetpub\wwwroot\imagetool  -erroraction 'silentlycontinue'

#Copy-Item -Path "\\corp.firstam.com\Apps\SCM\Repo\LVIS\Master\LVIS_image_extraction_tool_frankjia_10-06-2020_1\ImageExtraction\*" -Destination "c:\inetpub\wwwroot\imagetool\" -recurse -Force -Verbose


new-webapppool -Name "imagetoolpool"

Set-ItemProperty IIS:\apppools\imagetoolpool managedRuntimeVersion ""

$credentials = (Get-Credential -UserName "CORP\fahq-sa-LVISProd" -Message "Please enter the Login credentials including Domain Name").GetNetworkCredential()

$userName = $credentials.Domain + '\' + $credentials.UserName

set-itemproperty IIS:\apppools\imagetoolpool -name processModel.userName -Value $username

set-itemproperty IIS:\apppools\imagetoolpool -name processModel.password -Value $credentials.password

set-itemproperty IIS:\apppools\imagetoolpool -name processModel.identityType -Value SpecificUser

new-webapplication -site "Default Web Site" -name "imagetool" -PhysicalPath "C:\inetpub\wwwroot\imagetool" -ApplicationPool imagetoolpool


