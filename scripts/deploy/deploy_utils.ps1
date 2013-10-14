
function Deploy-WebSite {
  param([System.Xml.XmlElement] $config)

  $package = Get-BackupPackage -Config $config
  $source  = "$SourceDir\$($config.SourceRelativePath)"

  Backup-WebSite -Website $config.IISWebSite -ToPackage $package
  Publish-WebSite -Website $config.IISWebSite -Source $source -Destination $config.IISPhysicalPath
}

function Rollback-WebSite {
  param([System.Xml.XmlElement] $config)

  $package = Get-BackupPackage -Config $config
  Restore-WebSite -Website $config.IISWebSite -FromPackage $package
}

function Get-BackupPackage {
  param([System.Xml.XmlElement] $config)

  "$BackupDir\$($config.BackupPackage)"
}

function Backup-WebSite {
  param(
    [string] $webSite,
    [string] $toPackage
  )

  # Si un backup existe déjà, on ne le recrée pas.
  if (Test-Path $toPackage) {
    Write-Host "Skipping backup for '$website', the package already exists." `
      -ForegroundColor "Gray"
    Return
  }

  # http://edgylogic.com/blog/powershell-and-external-commands-done-right/
  # WARNING: Cela ne fonctionnera pas si il y a des espaces dans un des paramètres.
  $args = "-verb=sync", "-source=appHostConfig=$webSite", "-dest=package=$toPackage"
  if ($Simulate) {
    $args += "-WhatIf"
  }

  Write-Host "Backing up '$webSite'." -ForegroundColor "Yellow"
  Exec { MSDeploy $args >> $Logfile }
}

function Publish-WebSite {
  param(
    [string] $webSite,
    [string] $source,
    [string] $destination
  )

  Write-Host "Stopping '$webSite'." -ForegroundColor "Yellow"
  Stop-WebSite -Name $webSite

  # WARNING: Cela ne fonctionnera pas si il y a des espaces dans un des paramètres.
  $args = "-verb=sync", "-source=dirPath=$source", "-dest=dirPath=$destination", "-useCheckSum"
  if ($Simulate) {
    $args += '-WhatIf'
  }

  Write-Host "Publishing '$webSite'." -ForegroundColor "Yellow"
  Exec { MSDeploy $args >> $Logfile }

  Write-Host "Starting '$webSite'." -ForegroundColor "Yellow"
  Start-WebSite -Name $webSite
}

function Restore-WebSite {
  param(
    [string] $webSite,
    [string] $fromPackage
  )

  if (-not(Test-Path $fromPackage)) {
    throw "Can not restore '$webSite', no backup available."
  }

  Write-Host "Stopping '$webSite'." -ForegroundColor "Yellow"
  Stop-WebSite -Name $webSite

  # WARNING: Cela ne fonctionnera pas si il y a des espaces dans un des paramètres.
  $args = "-verb:sync", "-source:package=$fromPackage", "-dest:appHostConfig=$webSite", "-useCheckSum"
  if ($Simulate) {
    $args += '-WhatIf'
  }

  Write-Host "Restoring '$webSite'." -ForegroundColor "Yellow"
  Exec { MSDeploy $args >> $Logfile }

  Write-Host "Starting '$webSite'." -ForegroundColor "Yellow"
  Start-WebSite -Name $webSite
}
