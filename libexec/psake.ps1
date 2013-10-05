
properties {
  $MSProject = '.\Chiffon.proj'
  $MSOptions = "/nologo", "/v:minimal", "/fl", "/flp:logfile=..\msbuild.log;verbosity=normal;encoding=utf-8;"

  $Configuration   = "Release"
  $Platform        = "Any CPU"
  $BuildInParallel = "true"
}

Task default -depends Build

Task Clean -depends ReadBuildConfig {
  MSBuild $MSOptions $MSProject /t:Clean $MSProperties
}

Task Build -depends ReadBuildConfig {
  MSBuild $MSOptions $MSProject /t:Build $MSProperties
}

Task Rebuild -depends ReadBuildConfig {
  MSBuild $MSOptions $MSProject /t:Rebuild $MSProperties
}

Task RunTests -depends ReadBuildConfig {
  MSBuild $MSOptions $MSProject /t:RunTests $MSProperties
}

Task Integrate -depends ReadBuildConfig {
  MSBuild $MSOptions $MSProject /t:Integrate $MSProperties
}

Task Publish -depends ReadPublishConfig {
  MSBuild $MSOptions $MSProject /t:Publish $MSProperties
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

Task ReadPublishConfig {
  $configPath = $(Get-Location).Path + "\..\etc\Publish.config"

  [xml]$configXml = Get-Content -Path $configPath

  [System.Xml.XmlElement] $config = $configXml.configuration

  [string] $Milestone = $config.Milestone
  [string] $PublishTarget = $config.PublishTarget

  [string] $PublishAssets = $config.PublishAssets
  [string] $PublishMediaSite = $config.PublishMediaSite
  [string] $PublishWebSite = $config.PublishWebSite

  $script:MSProperties = "/p:Configuration=$Configuration",
    "/p:BuildInParallel=$BuildInParallel",
    "/p:Milestone=$Milestone",
    "/p:PublishTarget=$PublishTarget",
    "/p:PublishAssets=$PublishAssets",
    "/p:PublishMediaSite=$PublishMediaSite",
    "/p:PublishWebSite=$PublishWebSite";
}


