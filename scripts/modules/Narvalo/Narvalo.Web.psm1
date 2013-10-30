#Requires -Version 3.0

Set-StrictMode -Version Latest

# .SYNOPSIS
# Retourne le chemin vers MSDeploy.
function Get-WebDeployInstallPath {
  return (Get-ChildItem "HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" | Select -last 1).GetValue("InstallPath")
}

# .SYNOPSIS
# Importe WebAdministration, en tant que module ou snapin.
#
# .NOTES
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

Export-ModuleMember -Function *