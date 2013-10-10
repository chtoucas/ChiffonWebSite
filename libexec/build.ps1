
properties {
  $project = '.\Build.proj'

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

Task FastBuild {
  MSBuild $options $project /t:Build "/p:MvcBuildViews=false;RunTests=false;Analyze=false"
}
