#Requires -Version 3.0

param($task = 'default')

Set-StrictMode -Version Latest

#Get-Module Narvalo | Remove-Module
Import-Module Narvalo

$currentUser = New-Object Security.Principal.WindowsPrincipal $([Security.Principal.WindowsIdentity]::GetCurrent())
$isAdmin = $currentUser.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)

if ($isAdmin -eq $false)  {
  throw "You must be an Administrator to run this script."
}

Import-WebAdministration
Import-Module '.\Deployment.psm1'

# FIXME: Ne pas polluer PATH.
#$webDeployInstallPath = Get-WebDeployInstallPath
#$env:Path += (";", $webDeployInstallPath)

Get-Module psake | Remove-Module
Import-Module .\psake.psm1

Invoke-psake .\default.ps1 @args
