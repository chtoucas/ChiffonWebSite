
function Get-WebDeployInstallPath {
  return (Get-ChildItem "HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" | Select -last 1).GetValue("InstallPath")
}

# WebAdministration est disponible sous deux formes : module ou snapin.
# Copié depuis :
#   http://stackoverflow.com/questions/10700660/add-pssnapin-webadministration-in-windows7
function Import-WebAdministration {
  $moduleName = "WebAdministration"
  $loaded = $false

  if ($PSVersionTable.PSVersion.Major -ge 2) {
    if ((Get-Module -ListAvailable | ForEach-Object {$_.Name}) -contains $moduleName) {
      Import-Module $moduleName

      if ((Get-Module | ForEach-Object {$_.Name}) -contains $moduleName) {
        $loaded = $true
      }
    } elseif ((Get-Module | ForEach-Object {$_.Name}) -contains $moduleName) {
      $loaded = $true
    }
  }

  if (-not $loaded) {
    try {
      if ((Get-PSSnapin -Registered | ForEach-Object {$_.Name}) -contains $moduleName) {
        if ((Get-PSSnapin -Name $moduleName -ErrorAction SilentlyContinue) -eq $null) {
          Add-PSSnapin $moduleName
        }

        if ((Get-PSSnapin | ForEach-Object {$_.Name}) -contains $moduleName) {
          $loaded = $true
        }
      } elseif ((Get-PSSnapin | ForEach-Object {$_.Name}) -contains $moduleName) {
        $loaded = $true
      }
    } catch {
      Write-Error "`t`t$($MyInvocation.InvocationName): $_"
      Exit
    }
  }
}

$currentUser = New-Object Security.Principal.WindowsPrincipal $([Security.Principal.WindowsIdentity]::GetCurrent())
$isAdmin = $currentUser.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)

if ($isAdmin -eq $false)  {
  throw "You must be an Administrator to run this script."
}

Import-WebAdministration

# FIXME: Ne pas polluer PATH.
#$webDeployInstallPath = Get-WebDeployInstallPath
#$env:Path += (";", $webDeployInstallPath)

Remove-Module psake
Import-Module .\psake.psm1

Invoke-PSake .\default.ps1 @args
