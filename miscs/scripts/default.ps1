# TODO:
# - assets n'est pas un site web mais plutôt une application web.
# - MSDeploy -verb:sync -source:appHostConfig="XXX" -dest:auto -enableRule:AppOffline
# - MSDeploy retryInterval ?
#   cf. http://blog.mdavies.net/2012/08/12/microsofts-hidden-gem-msdeploy/
# - utiliser des transactions ?
# - regarder WDeploySnapin3.0
# - cf. http://www.troyhunt.com/2010/11/you-deploying-it-wrong-teamcity_26.html

Include ".\build_utils.ps1"

Properties {
  $basePath = $(Get-Location).Path

  [xml] $configXml = Get-Content -Path "$basePath\Deploy.config"
  [System.Xml.XmlElement] $config = $configXml.configuration

  $SourceDir = $basePath
  $BackupDir = "$basePath\_backup"
  $Logfile   = "$basePath\msdeploy.log"

  $WebSiteConfig   = $config.WebSite
  $MediaSiteConfig = $config.WebSite
  $AssetsConfig    = $config.Assets

  $DeployWebSite   = $WebSiteConfig.GetAttribute("deploy") -eq "true"
  $DeployMediaSite = $MediaSiteConfig.GetAttribute("deploy") -eq "true"
  $DeployAssets    = $AssetsConfig.GetAttribute("deploy") -eq "true"
}

Task default -depends Help

Task Help {
  Write-Host "Sorry, help not yet available..."
}

Task Backup -depends BackupWebSite, BackupMediaSite, BackupAssets
Task Deploy -depends DeployWebSite, DeployMediaSite, DeployAssets
Task Rollback -depends RollbackWebSite, RollbackMediaSite, RollbackAssets

Task DeployWebSite -depends BackupWebSite -precondition { $DeployWebSite } {
  Publish-FromConfig -Config $WebSiteConfig
}
Task BackupWebSite -depends CreateBackupDirectory {
  Backup-FromConfig -Config $WebSiteConfig
}
Task RollbackWebSite -precondition { $DeployWebSite } {
  Rollback-FromConfig -Config $WebSiteConfig
}

Task DeployMediaSite -depends BackupMediaSite -precondition { $DeployMediaSite } {
  Publish-FromConfig -Config $MediaSiteConfig
}
Task BackupMediaSite -depends CreateBackupDirectory {
  Backup-FromConfig -Config $MediaSiteConfig
}
Task RollbackMediaSite -precondition { $DeployMediaSite } {
  Rollback-FromConfig -Config $MediaSiteConfig
}

Task DeployAssets -depends BackupAssets -precondition { $DeployAssets } {
  Publish-FromConfig -Config $AssetsConfig
}
Task BackupAssets -depends CreateBackupDirectory {
  Backup-FromConfig -Config $AssetsConfig
}
Task RollbackAssets -precondition { $DeployAssets } {
  Rollback-FromConfig -Config $AssetsConfig
}

Task CreateBackupDirectory {
  if (Test-Path $BackupDir) {
    Return
  }

  New-Item $BackupDir -Type directory -ErrorAction SilentlyContinue | Out-Null
}
