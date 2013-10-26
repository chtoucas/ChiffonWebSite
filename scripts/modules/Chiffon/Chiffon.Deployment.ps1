#Requires -Version 3.0

# WARNING: MSDeploy & Powershell n'interagissent pas bien quand il y a des espaces.
# Cf. http://edgylogic.com/blog/powershell-and-external-commands-done-right/

#-- Fonctions publiques --#

function Get-BackupPackage {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [System.Xml.XmlElement] $config
  )

  "$BackupDir\$($config.BackupPackage)"
}

function Deploy-WebSite {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [System.Xml.XmlElement] $config
  )

  $package = Get-BackupPackage -Config $config
  $source  = "$SourceDir\$($config.SourceRelativePath)"

  Backup-WebSite -Website $config.IISWebSite -ToPackage $package
  Publish-WebSite -Website $config.IISWebSite -Source $source -Destination $config.IISPhysicalPath
}

function Rollback-WebSite {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [System.Xml.XmlElement] $config
  )

  $package = Get-BackupPackage -Config $config
  Restore-WebSite -Website $config.IISWebSite -FromPackage $package
}

function Backup-WebSite {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $webSite,
    [Parameter(Mandatory = $true, Position = 1)] [string] $toPackage
  )

  if ($website.Contains(" ") -or $toPackage.Contains(" ")) {
    throw 'The parameters can not contain spaces.'
  }

  # Si un backup existe déjà, on ne le recrée pas.
  if (Test-Path $toPackage) {
    Write-Host "Skipping backup for '$website', the package already exists." `
      -ForegroundColor "Gray"
    return
  }

  $args = "-verb=sync", "-source=appHostConfig=$webSite", "-dest=package=$toPackage"
  if ($Simulate) {
    $args += "-WhatIf"
  }

  Write-Host "Backing up '$webSite'." -ForegroundColor "Yellow"
  MSDeploy $args >> '.\msdeploy-backup.log'
}

function Publish-WebSite {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $webSite,
    [Parameter(Mandatory = $true, Position = 1)] [string] $source,
    [Parameter(Mandatory = $true, Position = 2)] [string] $destination
  )

  if ($source.Contains(" ") -or $destination.Contains(" ")) {
    throw 'The parameters can not contain spaces.'
  }

  Write-Host "Stopping '$webSite'." -ForegroundColor "Yellow"
  Stop-WebSite -Name $webSite

  $args = "-verb=sync", "-source=dirPath=$source", "-dest=dirPath=$destination", "-useCheckSum"
  if ($Simulate) {
    $args += '-WhatIf'
  }

  Write-Host "Publishing '$webSite'." -ForegroundColor "Yellow"
  MSDeploy $args >> '.\msdeploy-publish.log'

  Write-Host "Starting '$webSite'." -ForegroundColor "Yellow"
  Start-WebSite -Name $webSite
}

function Restore-WebSite {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $webSite,
    [Parameter(Mandatory = $true, Position = 1)] [string] $fromPackage
  )

  if ($website.Contains(" ") -or $fromPackage.Contains(" ")) {
    throw 'The parameters can not contain spaces.'
  }

  if (-not(Test-Path $fromPackage)) {
    throw "Can not restore '$webSite', no backup available."
  }

  Write-Host "Stopping '$webSite'." -ForegroundColor "Yellow"
  Stop-WebSite -Name $webSite

  $args = "-verb:sync", "-source:package=$fromPackage", "-dest:appHostConfig=$webSite", "-useCheckSum"
  if ($Simulate) {
    $args += '-WhatIf'
  }

  Write-Host "Restoring '$webSite'." -ForegroundColor "Yellow"
  MSDeploy $args >> '.\msdeploy-restore.log'

  Write-Host "Starting '$webSite'." -ForegroundColor "Yellow"
  Start-WebSite -Name $webSite
}

#-- Directives --#
