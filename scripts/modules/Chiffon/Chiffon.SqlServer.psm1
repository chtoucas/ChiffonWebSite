#Requires -Version 3.0

# http://technet.microsoft.com/en-us/library/hh245202.aspx
#Add-Type -AssemblyName 'System.IO.Compression.FileSystem'

#-- Fonctions publiques --#

function Export-DbCreation {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 1)]
    [string] $outFile
  )

  Write-Output "-> Exporting database creation."
  $database.Script() | Out-File $outFile
}

function Export-Data {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $tables = $database.Tables | Where { $_.IsSystemObject -eq $false }

  if ($tables.count -eq 0) {
    Write-Output '!! There are no data to export.'
    Return
  }

  Write-Output "-> Exporting data."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  $opts = $scripter.Options
  $opts.ScriptData = $true
  $opts.ScriptSchema = $false
  $opts.NoCommandTerminator = $true;

  $scripter.EnumScript($tables)
}

function Export-Tables {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $tables = $database.Tables | Where { $_.IsSystemObject -eq $false }

  if ($tables.count -eq 0) {
    Write-Output '!! There are no tables to export.'
    Return
  }

  Write-Output "-> Exporting $($tables.Count) tables."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  $opts = $scripter.Options
  $opts.ClusteredIndexes = $true
  $opts.Default = $true
  $opts.DriAll = $true
  $opts.DriIncludeSystemNames = $false
  $opts.IncludeIfNotExists = $false
  $opts.Indexes = $true
  $opts.NoCollation = $true
  $opts.NonClusteredIndexes = $true
  $opts.Permissions = $false
  $opts.SchemaQualify = $false
  $opts.SchemaQualifyForeignKeysReferences = $false
  $opts.ScriptDrops = $false
  $opts.ScriptOwner = $false
  $opts.WithDependencies = $false

  foreach ($tbl in $tables) {
    $scripter.Script($tbl)
  }
}

function Export-StoredProcedures {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $procedures = $database.StoredProcedures | Where { $_.IsSystemObject -eq $false }

  if ($procedures.Count -eq 0) {
    Write-Output '!! There are no stored procedures to export.'
    Return
  }

  Write-Output "-> Exporting $($procedures.Count) stored procedures."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  $opts = $scripter.Options

  foreach ($procedure in $procedures) {
    # Si la procédure stockée existe déjà, on la supprime.
    $opts.IncludeIfNotExists = $true
    $opts.ScriptDrops = $true
    $scripter.Script($procedure)

    # Création de la procédure stockée.
    $opts.IncludeIfNotExists = $false
    $opts.ScriptDrops = $false
    $scripter.Script($procedure)
  }
}

function Export-Views {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $views = $database.Views | Where { $_.IsSystemObject -eq $false }

  if ($views.count -eq 0) {
    Write-Output '!! There are no views to export.'
    Return
  }

  Write-Output "-> Exporting $($views.Count) views."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  foreach ($view in $views) {
    $scripter.Script($view)
  }
}

function Export-UserDefinedFunctions {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $udfs = $database.UserDefinedFunctions | Where { $_.IsSystemObject -eq $false }

  if ($udfs.count -eq 0) {
    Write-Output '!! There are no user defined functions to export.'
    Return
  }

  Write-Output "-> Exporting $($udfs.Count) views."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  foreach ($udf in $udfs) {
    $scripter.Script($udf)
  }
}

function Export-Triggers {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  $triggers = $database.Triggers | Where { $_.IsSystemObject -eq $false }

  if ($triggers.count -eq 0) {
    Write-Output '!! There are no triggers to export.'
    Return
  }

  Write-Output "-> Exporting $($triggers.Count) views."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  foreach ($trigger in $triggers) {
    $scripter.Script($trigger)
  }
}

function Export-TableTriggers {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [Microsoft.SqlServer.Management.Smo.Database] $database,
    [Parameter(Mandatory = $true, Position = 2)]
    [string] $outFile
  )

  Write-Output "-> Exporting table triggers."

  $scripter = New-Scripter -Server $server -OutFile $outFile

  foreach ($tbl in $db.Tables) {
    foreach ($trigger in $tbl.triggers) {
      $scripter.Script($trigger)
      }
  }
}

#-- Fonctions privées --#

function New-Scripter {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)]
    [Microsoft.SqlServer.Management.Smo.Server] $server,
    [Parameter(Mandatory = $true, Position = 1)]
    [string] $outFile
  )

  # Cf. http://msdn.microsoft.com/en-us/library/microsoft.sqlserver.management.smo.scriptingoptions.aspx
  $opts = New-Object Microsoft.SqlServer.Management.Smo.ScriptingOptions
  $opts.AllowSystemObjects = $false
  $opts.AppendToFile = $true
  $opts.FileName = $outFile
  $opts.ToFileOnly = $true

  $scripter = New-Object Microsoft.SqlServer.Management.Smo.Scripter
  $scripter.Server = $server
  $scripter.Options = $opts

  Return $scripter
}

#-- Directives --#

Export-ModuleMember -function Export-DbCreation, Export-Data, Export-Tables, `
  Export-StoredProcedures, Export-Views, Export-UserDefinedFunctions, Export-Triggers, `
  Export-TableTriggers