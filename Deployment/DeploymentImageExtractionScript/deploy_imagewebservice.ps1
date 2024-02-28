import-module WebAdministration

Remove-WebAppPool -name imagewebservicepool -erroraction 'silentlycontinue'
Remove-WebApplication -site "Default Web Site" -name imagewebservice   -erroraction 'silentlycontinue'

rd -path c:\inetpub\wwwroot\imagewebservice -erroraction 'silentlycontinue'

new-item -itemtype directory -path c:\inetpub\wwwroot\imagewebservice  -erroraction 'silentlycontinue'

#Copy-Item -Path "\\corp.firstam.com\Apps\SCM\Repo\LVIS\Master\LVIS_image_extraction_tool_frankjia_10-06-2020_1\_PublishedWebsites\ImageWebService\*" -Destination "c:\inetpub\wwwroot\imagewebservice\" -recurse -Force -Verbose


new-webapppool -Name "imagewebservicepool"

$credentials = (Get-Credential -UserName "CORP\fahq-sa-LVISProd" -Message "Please enter the Login credentials including Domain Name").GetNetworkCredential()

$userName = $credentials.Domain + '\' + $credentials.UserName

set-itemproperty IIS:\apppools\imagewebservicepool -name processModel.userName -Value $username

set-itemproperty IIS:\apppools\imagewebservicepool -name processModel.password -Value $credentials.password

set-itemproperty IIS:\apppools\imagewebservicepool -name processModel.identityType -Value SpecificUser

new-webapplication -site "Default Web Site" -name "imagewebservice" -PhysicalPath "C:\inetpub\wwwroot\imagewebservice" -ApplicationPool imagewebservicepool


