# TODO:
# - assets n'est pas un site web mais plutôt une application web.
# - détection / prise en charge des lock ?
# - MSDeploy -verb:sync -source:appHostConfig="XXX" -dest:auto -enableRule:AppOffline
# - MSDeploy retryInterval ?
#   cf. http://blog.mdavies.net/2012/08/12/microsofts-hidden-gem-msdeploy/
#   cf. http://raquila.com/software/ms-deploy-basics/
# - utiliser des transactions ?
# - regarder WDeploySnapin3.0
# - cf. http://www.troyhunt.com/2010/11/you-deploying-it-wrong-teamcity_26.html

Include ".\deploy_utils.ps1"

Properties {
  $BasePath = $(Get-Location).Path

  [xml] $configXml = Get-Content -Path "$BasePath\Deploy.config"
  [System.Xml.XmlElement] $Config = $configXml.configuration

  $SourceDir = $BasePath
  $BackupDir = "$BasePath\_backup"
  $Logfile   = "$BasePath\msdeploy.log"
  $Simulate  = $true

  $MainWebSiteConfig  = $Config.MainWebSite
  $MediaWebSiteConfig = $Config.MediaWebSite
  $AssetsWebAppConfig = $Config.AssetsWebApp

  $DeployMainWebSite  = $MainWebSiteConfig.GetAttribute("deploy") -eq "true"
  $DeployMediaWebSite = $MediaWebSiteConfig.GetAttribute("deploy") -eq "true"
  $DeployAssetsWebApp = $AssetsWebAppConfig.GetAttribute("deploy") -eq "true"
}

Task default -depends Help

Task Help {
  Write-Host "Sorry, help not yet available..."
}

#TaskTearDown  {
#  if ($LastExitCode -ne 0) {
#    Write-Host "Something went wrong in a task." -BackgroundColor Red -ForegroundColor White
#    Exit 1
#  }
#}

Task Deploy -depends DeployMainWebSite, DeployMediaWebSite, DeployAssetsWebApp
Task Rollback -depends RollbackMainWebSite, RollbackMediaWebSite, RollbackAssetsWebApp

Task DeployMainWebSite -depends CreateBackupDirectory -precondition { $DeployMainWebSite } {
  Deploy-WebSite -Config $MainWebSiteConfig
}
Task RollbackMainWebSite -precondition { $DeployMainWebSite } {
  Rollback-WebSite -Config $MainWebSiteConfig
}
Task DeployMediaWebSite -depends CreateBackupDirectory -precondition { $DeployMediaWebSite } {
  Deploy-WebSite -Config $MediaWebSiteConfig
}
Task RollbackMediaWebSite -precondition { $DeployMediaWebSite } {
  Rollback-WebSite -Config $MediaWebSiteConfig
}
Task DeployAssetsWebApp -depends CreateBackupDirectory -precondition { $DeployAssetsWebApp } {
  Deploy-WebSite -Config $AssetsWebAppConfig
}
Task RollbackAssetsWebApp -precondition { $DeployAssetsWebApp } {
  Rollback-WebSite -Config $AssetsWebAppConfig
}

Task CreateBackupDirectory {
  if (Test-Path $BackupDir) {
    Return
  }

  New-Item $BackupDir -Type directory -ErrorAction SilentlyContinue | Out-Null
}
