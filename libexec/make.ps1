
properties {
  $project = '.\Make.proj'

  $options = "/nologo", "/v:minimal", "/fl",
    "/flp:logfile=..\msbuild.log;verbosity=normal;encoding=utf-8",
    "/m", "/nodeReuse:false"
}

Task default -depends Build

Task Help {
  Write-Host "Help not yet done..."
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
  MSBuild $options $project /t:Milestone $properties
}

Task Package -depends ReadPackageConfig {
  MSBuild $options $project /t:Package $properties
}

Task ReadMilestoneConfig {
  $configPath = $(Get-Location).Path + "\..\etc\Milestone.config"

  [xml] $configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  [string] $Milestone = $config.Milestone

  $script:properties = "/p:Milestone=$Milestone";
}

Task ReadPackageConfig {
  $configPath = $(Get-Location).Path + "\..\etc\Package.config"

  [xml] $configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  [string] $PackageTarget    = $config.PackageTarget
  [string] $PackageAssets    = $config.PackageAssets
  [string] $PackageMediaSite = $config.PackageMediaSite
  [string] $PackageWebSite   = $config.PackageWebSite

  $script:properties = "/p:PackageTarget=$PackageTarget",
    "/p:PackageAssets=$PackageAssets",
    "/p:PackageMediaSite=$PackageMediaSite",
    "/p:PackageWebSite=$PackageWebSite";
}
