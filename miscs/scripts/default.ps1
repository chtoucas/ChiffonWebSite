# MSDeploy -verb:sync -source:appHostConfig="XXX" -dest:auto -enableRule:AppOffline
# http://blog.mdavies.net/2012/08/12/microsofts-hidden-gem-msdeploy/
# MSDeploy retryInterval ?

Properties {
  $basePath = $(Get-Location).Path

  [xml] $configXml = Get-Content -Path "$basePath\Deploy.config"
  [System.Xml.XmlElement] $config = $configXml.configuration

  $SourceDir = $basePath
  $BackupDir = "$basePath\_backup"
  $Logfile = "$basePath\deploy.log"

  $WebSiteConfig = $config.WebSite
  $AssetsConfig  = $config.Assets

  $DeployWebSite = $WebSiteConfig.GetAttribute("deploy") -eq "true"
  $DeployAssets = $AssetsConfig.GetAttribute("deploy") -eq "true"
}

Task default -depends Help

Task Help {
  Write-Host "Sorry, help not yet available..."
}

Task Backup -depends BackupWebSite, BackupAssets

Task Deploy -depends DeployWebSite, DeployAssets

Task Rollback -depends RollbackWebSite, RollbackAssets


Task BackupWebSite -depends CreateBackupDirectory {
  Backup-FromConfig -Config $WebSiteConfig
}

Task DeployWebSite -depends BackupWebSite -precondition { $DeployWebSite } {
  Publish-FromConfig -Config $WebSiteConfig
}

Task RollbackWebSite -precondition { $DeployWebSite } {
  Rollback-FromConfig -Config $WebSiteConfig
}


Task BackupAssets -depends CreateBackupDirectory {
  Backup-FromConfig -Config $AssetsConfig
}

Task DeployAssets -depends BackupAssets -precondition { $DeployAssets } {
  Publish-FromConfig -Config $AssetsConfig
}

Task RollbackAssets -precondition { $DeployAssets } {
  Rollback-FromConfig -Config $AssetsConfig
}


Task CreateBackupDirectory {
  if (Test-Path $BackupDir) {
    return
  }

  New-Item $BackupDir -Type directory -ErrorAction SilentlyContinue | Out-Null
}


# Utilitaires

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

  if (Test-Path $toPackage) {
    Write-ColoredOutput "Skipping backup for '$website', the package already exists..."  -foregroundcolor "Gray"
    Return
  }

  Write-ColoredOutput "Backing up '$webSite'..." -foregroundcolor "Yellow"
  Exec { MSDeploy "-verb:sync", "-source:appHostConfig=$webSite" "-dest:package=$toPackage" >> $Logfile }
}

function Publish-WebSite {
  param(
    [string] $webSite,
    [string] $source,
    [string] $destination
  )

  Write-ColoredOutput "Stopping '$webSite'..." -foregroundcolor "Yellow"
  Stop-WebSite -Name $webSite

  Write-ColoredOutput "Publishing '$webSite'..." -foregroundcolor "Yellow"
  Exec { MSDeploy "-verb=sync", "-source=dirPath=$source", "-dest=dirPath=$destination", "-useCheckSum" >> $Logfile  }

  Write-ColoredOutput "Starting '$webSite'..." -foregroundcolor "Yellow"
  Start-WebSite -Name $webSite
}

function Rollback-WebSite {
  param(
    [string] $webSite,
    [string] $fromPackage
  )

  if (-not(Test-Path $fromPackage)) {
    throw "Can not rollback, '$fromPackage' does not exist..."
  }

  Write-ColoredOutput "Stopping '$webSite'..." -foregroundcolor "Yellow"
  Stop-WebSite -Name $webSite

  Write-ColoredOutput "Rolling back '$webSite'..." -foregroundcolor "Yellow"
  Exec { MSDeploy "-verb:sync", "-source:package=$fromPackage", "-dest:appHostConfig=$webSite", "-useCheckSum" >> $Logfile }

  Write-ColoredOutput "Starting '$webSite'..." -foregroundcolor "Yellow"
  Start-WebSite -Name $webSite
}
