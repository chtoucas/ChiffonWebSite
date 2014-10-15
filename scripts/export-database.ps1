#Requires -Version 3.0

# Usage:
#   .\export-database.ps1 'SERVERNAME' 'DATABASE' '.\exports\'
#   .\export-database.ps1 '(localdb)\v11.0' 'Chiffon' '.\exports\'

param(
  [Parameter(Mandatory = $true, Position = 0)] [string] $serverName,
  [Parameter(Mandatory = $true, Position = 1)] [string] $databaseName,
  [Parameter(Mandatory = $false, Position = 2)] [string] $outDir = $null
)

Set-StrictMode -Version Latest

#Get-Module Narvalo | Remove-Module
Import-Module Narvalo

if (!$outDir) {
  $outPath = "$PSScriptRoot\..\_work\stage\sql"
} else {
  $outPath = (Get-Item $outDir).FullName
}

if (!(Test-Path $outPath)) {
  New-Directory $outPath | Out-Null
}

$server = New-Object Microsoft.SqlServer.Management.Smo.Server $serverName
# La ligne suivante permet d'accélérer l'exécution de SMO.
$server.SetDefaultInitFields([Microsoft.SqlServer.Management.Smo.Table], "IsSystemObject")
$server.SetDefaultInitFields([Microsoft.SqlServer.Management.Smo.StoredProcedure], "IsSystemObject")

$db = New-Object Microsoft.SqlServer.Management.Smo.Database
$db = $server.Databases[$databaseName]

Write-Output '-> Exporting data.'
Export-Data -Server $server -Database $db -OutFile ("$outPath\Data.sql")
Write-Output '-> Exporting tables.'
Export-Tables -Server $server -Database $db -OutFile ("$outPath\Tables.sql")
Write-Output '-> Exporting stored procedures.'
Export-StoredProcedures -Server $server -Database $db -OutFile ("$outPath\StoredProcedures.sql")

#Export-DbCreation -Database $db -OutFile ("$outPath\Create.sql")
#Export-Views -Server $server -Database $db -OutFile ("$outPath\Views.sql")
#Export-UserDefinedFunctions -Server $server -Database $db -OutFile ("$outPath\UserDefinedFunctions.sql")
#Export-Triggers -Server $server -Database $db -OutFile ("$outPath\Triggers.sql")
#Export-TableTriggers -Server $server -Database $db -OutFile ("$outPath\TableTriggers.sql")
