$currentUser = New-Object Security.Principal.WindowsPrincipal $([Security.Principal.WindowsIdentity]::GetCurrent())
$isAdmin = $currentUser.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)

if ($isAdmin -eq $false)  {
  throw "You must be an Administrator to run this script."
}

Import-Module ".\utils.ps1"

Import-WebAdministration

$webDeployInstallPath = Get-WebDeployInstallPath
$env:Path += (";", $webDeployInstallPath)

Remove-Module psake
Import-Module .\psake.psm1

Invoke-PSake .\default.ps1 @args
