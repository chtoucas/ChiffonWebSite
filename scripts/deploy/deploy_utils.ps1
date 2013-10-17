# WARNING: MSDeploy & Powershell n'interagissent pas bien quand il y a des espaces.
# Cf. http://edgylogic.com/blog/powershell-and-external-commands-done-right/

function Get-BackupPackage {
  param(
    [Parameter(Mandatory=$true)]
    [System.Xml.XmlElement] $config
  )

  "$BackupDir\$($config.BackupPackage)"
}

function Deploy-WebSite {
  param(
    [Parameter(Mandatory=$true)]
    [System.Xml.XmlElement] $config
  )

  $package = Get-BackupPackage -Config $config
  $source  = "$SourceDir\$($config.SourceRelativePath)"

  Backup-WebSite -Website $config.IISWebSite -ToPackage $package
  Publish-WebSite -Website $config.IISWebSite -Source $source -Destination $config.IISPhysicalPath
}

function Rollback-WebSite {
  param(
    [Parameter(Mandatory=$true)]
    [System.Xml.XmlElement] $config
  )

  $package = Get-BackupPackage -Config $config
  Restore-WebSite -Website $config.IISWebSite -FromPackage $package
}

function Backup-WebSite {
  param(
    [Parameter(Mandatory=$true)]
    [string] $webSite,
    [Parameter(Mandatory=$true)]
    [string] $toPackage
  )

  if ($website.Contains(" ") -or $toPackage.Contains(" ")) {
    throw 'The parameters can not contain spaces.'
  }

  # Si un backup existe déjà, on ne le recrée pas.
  if (Test-Path $toPackage) {
    Write-Host "Skipping backup for '$website', the package already exists." `
      -ForegroundColor "Gray"
    Return
  }

  $args = "-verb=sync", "-source=appHostConfig=$webSite", "-dest=package=$toPackage"
  if ($Simulate) {
    $args += "-WhatIf"
  }

  Write-Host "Backing up '$webSite'." -ForegroundColor "Yellow"
  MSDeploy $args >> '.\msdeploy-backup.log'
}

function Publish-WebSite {
  param(
    [Parameter(Mandatory=$true)]
    [string] $webSite,
    [Parameter(Mandatory=$true)]
    [string] $source,
    [Parameter(Mandatory=$true)]
    [string] $destination
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
  param(
    [Parameter(Mandatory=$true)]
    [string] $webSite,
    [Parameter(Mandatory=$true)]
    [string] $fromPackage
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
