# First version:
# - start/stop IIS AppPools
#   Cf. http://technet.microsoft.com/en-us/library/ee790553.aspx
#   Cf. http://www.iis.net/learn/manage/powershell/powershell-snap-in-using-the-task-based-cmdlets-of-the-iis-powershell-snap-in
# - use transactions? sync?
# Second version: use Web Deploy
#   Use WDeploySnapin3.0
#   Cf. http://msdn.microsoft.com/en-us/library/dd394698.aspx
#   Cf. http://www.troyhunt.com/2010/11/you-deploying-it-wrong-teamcity_26.html

if ((Get-PSSnapin -Name WebAdministration -ErrorAction SilentlyContinue) -eq $null ){
    Add-PSSnapin WebAdministration -ErrorAction Stop
}

Properties {
  $backupDir = "$PWD\_backup"
}

Task default -depends Help

Task Help {
  Write-Host "Sorry, help not yet available..."
}

Task Deploy -depends DeployWebSite, DeployAssets

Task Rollback -depends RollbackWebSite, RollbackAssets

Task DeployWebSite -depends ReadConfig, CreateBackupDirectory {
  if (-not(Test-Path $newWebSite)) {
    throw "No new content to deploy..."
  }
  if (Test-Path $webSiteBackup) {
    throw "Backup already exists..."
  }

  StopIISPool($webSitePool);

  Write-Host "Deploying website..."

  Move-Item $webSite    -Destination $webSiteBackup -Force
  Move-Item $newWebSite -Destination $webSite -Force

  StartIISPool($webSitePool);
}

Task RollbackWebSite -depends ReadConfig {
  if (-not(Test-Path $webSiteBackup)) {
    throw "Backup does not exist..."
  }
  if (Test-Path $newWebSite) {
    throw "New content not yet deployed..."
  }

  StopIISPool($webSitePool);

  Write-Host "Rollbacking website..."

  Move-Item $webSite       -Destination $newWebSite -Force
  Move-Item $webSiteBackup -Destination $webSite -Force

  StartIISPool($webSitePool);
}

Task DeployAssets -depends ReadConfig, CreateBackupDirectory {
  if (-not(Test-Path $newAssets)) {
    throw "No new content to deploy..."
  }
  if (Test-Path $assetsBackup) {
    throw "Backup already exists..."
  }

  StopIISPool($assetsPool);

  Write-Host "Deploying assets..."

  Move-Item $assets    -Destination $assetsBackup -Force
  Move-Item $newAssets -Destination $assets -Force

  StartIISPool($assetsPool);
}

Task RollbackAssets -depends ReadConfig {
  if (-not(Test-Path $assetsBackup)) {
    throw "Backup does not exist..."
  }
  if (Test-Path $newAssets) {
    throw "New content not yet deployed..."
  }

  StopIISPool($assetsPool);

  Write-Host "Rollbacking assets..."

  Move-Item $assets       -Destination $newAssets -Force
  Move-Item $assetsBackup -Destination $assets -Force

  StartIISPool($assetsPool);
}

Task CreateBackupDirectory {
  if (Test-Path $backupDir) {
    return
  }

  New-Item $backupDir -Type directory -ErrorAction SilentlyContinue | Out-Null
}

Task ReadConfig {
  $sourceDir = "$PWD\_source"
  $targetDir = "$PWD\_target"

  $script:webSitePool   = "pourquelmotifsimone.com"
  $script:webSite       = "$targetDir\chiffon\www\"
  $script:newWebSite    = "$sourceDir\pourquelmotifsimone.com\"
  $script:webSiteBackup = "$backupDir\pourquelmotifsimone.com\"

  $script:assetsPool    = "wznw.org"
  $script:assets        = "$targetDir\wznw\www\chiffon\"
  $script:newAssets     = "$sourceDir\wznw.org_chiffon\"
  $script:assetsBackup  = "$backupDir\wznw.org_chiffon\"
}

function StopIISPool {
  param($appPoolName)

  Write-Host "Stopping '$appPoolName' pool."

  Stop-WebAppPool -Name $appPoolName
}

function StartIISPool {
  param($appPool)

  Write-Host "Starting '$appPoolName' pool."

  Start-WebAppPool -Name $appPoolName
}
