
properties {
  $verbosity      = 'minimal'
  $configuration  = 'Release'
  $publish_target = 'Production'

  # Ne rien changer ci-dessous

  $msproject = 'Chiffon.proj'
  $msoptions = "/nologo", "/v:`"$verbosity`"", "/fl", "/flp:logfile=msbuild.log;verbosity=normal;"
  $msproperties = "/p:Configuration=`"$configuration`";PublishTarget=`"$publish_target`"";
}

Task default -depends Build

Task Clean {
  msbuild $msoptions .\$msproject /t:Clean $msproperties
}

Task Build {
  msbuild $msoptions .\$msproject $msproperties
}

Task Publish {
  msbuild $msoptions .\$msproject /t:Publish $msproperties
}

Task Package {
  msbuild $msoptions .\$msproject /t:Package $msproperties
}
