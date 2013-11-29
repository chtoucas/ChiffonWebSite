
Properties {
  $project = '.\Make.proj'

  $options = '/nologo', '/v:minimal', '/fl',
    '/flp:logfile=..\..\msbuild.log;verbosity=normal;encoding=utf-8',
    '/m', '/nodeReuse:false'
}

Task default -depends Build

Task Help {
  Write-Host 'Sorry, help not yet available...'
}

Task Clean {
  MSBuild $options $project /t:Clean
}

Task Build {
  MSBuild $options $project /t:Build
}

Task Rebuild {
  MSBuild $options $project /t:Rebuild
}

Task Milestone -depends ReadMilestoneConfig {
  MSBuild $options $project /t:Milestone $msproperties
}

Task ExportDatabase {
  Import-Module Narvalo

  $outPath = "$PSScriptRoot\..\..\_work\stage\sql"

  if (!(Test-Path $outPath)) {
    New-Directory $outPath | Out-Null
  }

  $serverName   = '(localdb)\v11.0'
  $databaseName = 'Chiffon'

  $server = New-Object Microsoft.SqlServer.Management.Smo.Server $serverName
  # La ligne suivante permet d'accélérer l'exécution de SMO.
  $server.SetDefaultInitFields([Microsoft.SqlServer.Management.Smo.Table], "IsSystemObject")
  $server.SetDefaultInitFields([Microsoft.SqlServer.Management.Smo.StoredProcedure], "IsSystemObject")

  $db = New-Object Microsoft.SqlServer.Management.Smo.Database
  $db = $server.Databases[$databaseName]

  Write-Output '-> Exporting tables.'
  Export-Tables -Server $server -Database $db -OutFile ("$outPath\Tables.sql")
  Write-Output '-> Exporting stored procedures.'
  Export-StoredProcedures -Server $server -Database $db -OutFile ("$outPath\StoredProcedures.sql")
}

Task Package -depends ReadPackageConfig {
  MSBuild $options $project /t:Package $msproperties
}

Task ReadMilestoneConfig {
  $configPath = $(Get-Location).Path + "\..\..\etc\Milestone.config"

  [xml] $configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  [string] $milestone = $config.Milestone

  $script:msproperties = "/p:Milestone=$milestone";
}

Task ReadPackageConfig {
  $configPath = $(Get-Location).Path + '\..\..\etc\Package.config'

  [xml] $configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  [string] $packageTarget    = $config.PackageTarget
  [string] $packageAssets    = $config.PackageAssets
  [string] $packageMediaSite = $config.PackageMediaSite
  [string] $packageWebSite   = $config.PackageWebSite

  $script:msproperties = "/p:PackageTarget=$packageTarget",
    "/p:PackageAssets=$packageAssets",
    "/p:PackageMediaSite=$packageMediaSite",
    "/p:PackageWebSite=$packageWebSite";
}
