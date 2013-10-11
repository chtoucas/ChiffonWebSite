# - transactions
# - start/stop IIS service
# - check directories' path are absolute

properties {
  $backupDir = "$PWD\_backup"
}

Task default -depends Help

Task Help {
  Write-Host "Sorry, help not yet available..."
}

Task Clean {
  Remove-Item $backupDir
}

Task Build -depends Backup, Deploy

Task Backup -depends ReadFakeConfig, CreateBackupDirectory {
  Copy-Item $oldWebsite -Destination $backupDir -Recurse -Force
  Copy-Item $oldAssets  -Destination $backupDir -Recurse -Force
}

Task Restore -depends ReadFakeConfig {

}

Task Deploy -depends ReadFakeConfig, CopyFiles {
  Rename-Item $oldWebsite -NewName "$oldWebsite.OLD" -Force
  Rename-Item $oldAssets -NewName "$oldAssets.OLD" -Force
}

Task CopyFiles -depends ReadFakeConfig {
  Copy-Item $newWebsite -Destination $backupDir -Recurse -Force
  Copy-Item $newAssets  -Destination $backupDir -Recurse -Force
}

Task CreateBackupDirectory {
  if (Test-Path $BackupDir) {
    throw "Backup directory already exists. Run 'Deploy' before."
  }

  New-Item $backupDir -Type directory -ErrorAction SilentlyContinue | Out-Null
}

Task ReadFakeConfig {
  $sourceDir = "$PWD\_source"
  $targetDir = "$PWD\_target"

  $script:oldWebsite = "$targetDir\pourquelmotifsimone.com\"
  $script:newWebsite = "$sourceDir\pourquelmotifsimone.com\"

  $script:oldAssets = "$targetDir\wznw.org_chiffon\"
  $script:newAssets = "$sourceDir\wznw.org_chiffon\"
}

Task ReadConfig {
  $configPath = $(Get-Location).Path + "\Publish.config"

  [xml] $configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  $script:assetsDir  = $config.AssetsDir
  $script:webSiteDir = $config.WebSiteDir
}
