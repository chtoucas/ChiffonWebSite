# MSDeploy -verb:sync -source:appHostConfig="XXX" -dest:auto -enableRule:AppOffline

Properties {
  $scriptPath = $(Get-Location).Path

  $backupDir = "$scriptPath\_backup"
  $configPath = "$scriptPath\Deploy.config"

  [xml] $configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  $webSite = $config.WebSite
  $assets  = $config.Assets
}

Task default -depends Help

Task Help {
  Write-Host "Sorry, help not yet available..."
}

Task Deploy -depends DeployWebSite

Task Rollback -depends RollbackWebSite

Task DeployWebSite -depends BackupWebSite `
  -precondition { $webSite.GetAttribute("deploy") -eq "true" } {

  Publish-WebSite -Website $webSite.IISWebSite `
    -Source "$scriptPath\$($webSite.SourceRelativePath)" `
    -Destination $webSite.IISPhysicalPath
}

Task BackupWebSite -depends CreateBackupDirectory {
  $backupPackage = "$backupDir\$($webSite.BackupPackage)"

  if (Test-Path $backupPackage) {
    Write-ColoredOutput "Backup skipped, it already exists..."  -foregroundcolor "Yellow"
    Return
  }

  Backup-WebSite -Website $webSite.IISWebSite -BackupPackage $backupPackage
}

Task RollbackWebSite {
  $backupPackage = "$backupDir\$($webSite.BackupPackage)"

  if (-not(Test-Path $backupPackage)) {
    Write-ColoredOutput "Backup package does not exist..."  -foregroundcolor "Yellow"
    Return
  }

  Rollback-WebSite -Website $webSite.IISWebSite -BackupPackage $backupPackage
}

Task DeployAssets -depends BackupAssets `
  -precondition { $assets.GetAttribute("deploy") -eq "true" } {
}

Task BackupAssets -depends CreateBackupDirectory {
}

Task RollbackAssets -depends ReadConfig {
}

Task CreateBackupDirectory {
  if (Test-Path $backupDir) {
    return
  }

  New-Item $backupDir -Type directory -ErrorAction SilentlyContinue | Out-Null
}

function Publish-WebSite {
  param(
    [string] $webSite,
    [string] $source,
    [string] $destination
  )

  Write-ColoredOutput "Stopping $webSite..." -foregroundcolor "Yellow"
  Stop-WebSite -Name $webSite

  Exec { MSDeploy "-verb=sync", "-source=dirPath=$source", "-dest=dirPath=$destination" }
  #if ($LastExitCode -ne 0) { throw "MSDeploy failed." }

  Write-ColoredOutput "Starting $webSite..." -foregroundcolor "Yellow"
  Start-WebSite -Name $webSite
}

function Backup-WebSite {
  param(
    [string] $webSite,
    [string] $backupPackage
  )

  Write-ColoredOutput "Backing up $webSite..." -foregroundcolor "Yellow"

  Exec { MSDeploy "-verb:sync", "-source:appHostConfig=$webSite" "-dest:package=$backupPackage" }
  #if ($LastExitCode -ne 0) { throw "MSDeploy failed." }
}

function Rollback-WebSite {
  param(
    [string] $webSite,
    [string] $backupPackage
  )

  Write-ColoredOutput "Stopping $webSite..." -foregroundcolor "Yellow"
  Stop-WebSite -Name $webSite

  Write-ColoredOutput "Rolling back $webSite..." -foregroundcolor "Yellow"

  Exec { MSDeploy "-verb:sync", "-source:package=$backupPackage", "-dest:appHostConfig=$webSite" }
  #if ($LastExitCode -ne 0) { throw "MSDeploy failed." }

  Write-ColoredOutput "Starting $webSite..." -foregroundcolor "Yellow"
  Start-WebSite -Name $webSite
}
