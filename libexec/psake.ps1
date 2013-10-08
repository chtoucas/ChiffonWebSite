
properties {
  $BuildProject = '.\Build.proj'
  $PackageProject = '.\Package.proj'

  $MSOptions = "/nologo", "/m:2", "/verbosity:minimal", "/fl", "/flp:logfile=..\msbuild.log;verbosity=normal;encoding=utf-8;"

  $Configuration   = "Release"
  $Platform        = "Any CPU"
  $BuildInParallel = "true"
}

Task default -depends Build

Task DeepClean {
  MSBuild $MSOptions $BuildProject /t:DeepClean
}

Task Clean {
  MSBuild $MSOptions $BuildProject /t:Clean
}

Task Build -depends ReadBuildConfig {
  MSBuild $MSOptions $BuildProject /t:Build $MSProperties
}

Task Rebuild -depends ReadBuildConfig {
  MSBuild $MSOptions $BuildProject /t:Rebuild $MSProperties
}

Task FastBuild -depends ReadBuildConfig {
  MSBuild $MSOptions $BuildProject /t:Build $MSProperties "/p:MvcBuildViews=false;RunTests=false"
}

Task Integrate {
  MSBuild $MSOptions $PackageProject /t:Integrate
}

Task Package -depends ReadPackageConfig {
  MSBuild $MSOptions $PackageProject /t:Package $MSProperties
}

Task Repackage -depends ReadPackageConfig {
  MSBuild $MSOptions $PackageProject /t:Repackage $MSProperties
}

Task ReadBuildConfig {
  $configPath = $(Get-Location).Path + "\..\etc\Build.config"

  [xml]$configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  [string] $BuildAssets = $config.BuildAssets
  [string] $BuildSolution = $config.BuildSolution

  $script:MSProperties = "/p:Configuration=$Configuration",
    "/p:BuildInParallel=$BuildInParallel",
    "/p:BuildAssets=$BuildAssets",
    "/p:BuildSolution=$BuildSolution";
}

Task ReadPackageConfig {
  $configPath = $(Get-Location).Path + "\..\etc\Package.config"

  [xml] $configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  [string] $Milestone = $config.Milestone
  [string] $PackageTarget = $config.PackageTarget

  [string] $PackageAssets = $config.PackageAssets
  [string] $PackageMediaSite = $config.PackageMediaSite
  [string] $PackageWebSite = $config.PackageWebSite

  $script:MSProperties = "/p:Configuration=$Configuration",
    "/p:BuildInParallel=$BuildInParallel",
    "/p:Milestone=$Milestone",
    "/p:PackageTarget=$PackageTarget",
    "/p:PackageAssets=$PackageAssets",
    "/p:PackageMediaSite=$PackageMediaSite",
    "/p:PackageWebSite=$PackageWebSite";
}


