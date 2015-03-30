
Properties {
  $project = '.\Make.proj'

  $options = '/nologo', '/v:minimal', '/p:DefineConstants="SHOWCASE"',
    '/maxcpucount', '/nodeReuse:false'
}

Task default -depends Build

Task Clean {
  MSBuild $options $project /t:Clean
}

Task Build {
  MSBuild $options $project /t:Build
}

Task Package {
  MSBuild $options $project /t:Package /p:VersionNumber=1.15.0.6
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
