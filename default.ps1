
properties {
  $verbosity        = 'minimal'
  $configuration    = 'Release'
  $target_env       = 'Production'

  # Ne rien changer ci-dessous

  $msproject = 'Chiffon.proj'
  $msoptions = "/nologo", "/v:`"$verbosity`"", "/fl", "/flp:logfile=msbuild.log;verbosity=normal;"
  $msproperties = "/p:Configuration=`"$configuration`";TargetEnv=`"$target_env`"";
}

Task default -depends Build

Task Clean {
  msbuild $msoptions .\$msproject /t:Clean $msproperties
}

Task Build {
  msbuild $msoptions .\$msproject $msproperties
}

Task BuildViews {
  msbuild $msoptions .\$msproject /t:BuildViews $msproperties
}

Task Minify {
  msbuild $msoptions .\$msproject /p:MinifyOnly=true $msproperties
}

Task Publish {
  msbuild $msoptions .\$msproject /t:Publish $msproperties
}

Task Package {
  msbuild $msoptions .\$msproject /t:Package $msproperties
}
