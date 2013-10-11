
properties {
  $project = '.\Make.proj'

  $options = "/nologo", "/v:minimal", "/fl",
    "/flp:logfile=..\msbuild.log;verbosity=normal;encoding=utf-8",
    "/m", "/nodeReuse:false"
}

Task default -depends Build

Task Help {
  Write-Host "Sorry, help not yet available..."
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

Task Package -depends ReadPackageConfig {
  MSBuild $options $project /t:Package $msproperties
}

Task ReadMilestoneConfig {
  $configPath = $(Get-Location).Path + "\..\etc\Milestone.config"

  [xml] $configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  [string] $milestone = $config.Milestone

  $script:properties = "/p:Milestone=$milestone";
}

Task ReadPackageConfig {
  $configPath = $(Get-Location).Path + "\..\etc\Package.config"

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
