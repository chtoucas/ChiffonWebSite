
function Get-BackupPackage {
  param([System.Xml.XmlElement] $config)

  "$BackupDir\$($config.BackupPackage)"
}

function Backup-FromConfig {
  param([System.Xml.XmlElement] $config)

  $package = Get-BackupPackage -Config $config
  Backup-WebSite -Website $config.IISWebSite -ToPackage $package
}

function Publish-FromConfig {
  param([System.Xml.XmlElement] $config)

  $source = "$SourceDir\$($config.SourceRelativePath)"
  Publish-WebSite -Website $config.IISWebSite -Source $source -Destination $config.IISPhysicalPath
}

function Rollback-FromConfig {
  param([System.Xml.XmlElement] $config)

  $package = Get-BackupPackage -Config $config
  Rollback-WebSite -Website $config.IISWebSite -FromPackage $package
}

function Backup-WebSite {
  param(
    [string] $webSite,
    [string] $toPackage
  )

  # Si un backup existe déjà, on ne le recrée pas.
  if (Test-Path $toPackage) {
    Write-ColoredOutput "Skipping backup for '$website', the package already exists."  -foregroundcolor "Gray"
    Return
  }

  Write-ColoredOutput "Backing up '$webSite'." -foregroundcolor "Yellow"
  Exec { MSDeploy "-verb:sync", "-source:appHostConfig=$webSite", "-dest:package=$toPackage" >> $Logfile }
}

function Publish-WebSite {
  param(
    [string] $webSite,
    [string] $source,
    [string] $destination
  )

  Write-ColoredOutput "Stopping '$webSite'." -foregroundcolor "Yellow"
  Stop-WebSite -Name $webSite

  Write-ColoredOutput "Publishing '$webSite'." -foregroundcolor "Yellow"
  Exec { MSDeploy "-verb=sync", "-source=dirPath=$source", "-dest=dirPath=$destination", "-useCheckSum" >> $Logfile  }

  Write-ColoredOutput "Starting '$webSite'." -foregroundcolor "Yellow"
  Start-WebSite -Name $webSite
}

function Rollback-WebSite {
  param(
    [string] $webSite,
    [string] $fromPackage
  )

  if (-not(Test-Path $fromPackage)) {
    throw "Can not rollback '$webSite', no backup available."
  }

  Write-ColoredOutput "Stopping '$webSite'." -foregroundcolor "Yellow"
  Stop-WebSite -Name $webSite

  Write-ColoredOutput "Rolling back '$webSite'." -foregroundcolor "Yellow"
  Exec { MSDeploy "-verb:sync", "-source:package=$fromPackage", "-dest:appHostConfig=$webSite", "-useCheckSum" >> $Logfile }

  Write-ColoredOutput "Starting '$webSite'." -foregroundcolor "Yellow"
  Start-WebSite -Name $webSite
}
