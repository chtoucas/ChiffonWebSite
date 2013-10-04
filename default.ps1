
properties {
  $verbosity         = 'minimal'
  $configuration     = 'Release'

  $publish_target    = 'Production'
  $milestone         = 'Patch'

  # Ne rien changer ci-dessous
  # --------------------------

  $msproject = 'Chiffon.proj'
  $msoptions = "/nologo", "/v:$verbosity", "/fl", "/flp:logfile=msbuild.log;verbosity=normal;"
  $msproperties = "/p:Configuration=$configuration";
}

Task default -depends Build

Task Clean {
  msbuild $msoptions .\$msproject /t:Clean $msproperties
}

Task Build {
  msbuild $msoptions .\$msproject $msproperties
}

Task Rebuild {
  msbuild $msoptions .\$msproject /t:Rebuild $msproperties
}

Task Publish {
  msbuild $msoptions .\$msproject /t:Publish $msproperties "/p:PublishTarget=$publish_target;Milestone=$milestone"
}

