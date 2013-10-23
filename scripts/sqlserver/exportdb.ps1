# Usage:
#   .\exportdb.ps1 "SERVERNAME" "DATABASE"
#   .\exportdb.ps1 "(localdb)\v11.0" "Chiffon"

param(
  [Parameter(Mandatory = $true)] [string] $serverName,
  [Parameter(Mandatory = $true)] [string] $databaseName
)

Import-Module ".\SqlServer-Export.psm1"

# Cf. http://technet.microsoft.com/en-us/library/hh847796.aspx
$ErrorActionPreference = "Stop"

$outDir = $(Get-Location).Path + '\exports\'
if (Test-Path -Path $outDir) {
  Remove-Item -Recurse $outDir | Out-Null
}
md $outDir | Out-Null

[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.Smo') | Out-Null
[System.Reflection.Assembly]::LoadWithPartialName('System.Data') | Out-Null

$server = New-Object Microsoft.SqlServer.Management.Smo.Server $serverName
# La ligne suivante permet d'accélérer l'exécution de SMO.
$server.SetDefaultInitFields([Microsoft.SqlServer.Management.Smo.Table], "IsSystemObject")
$server.SetDefaultInitFields([Microsoft.SqlServer.Management.Smo.StoredProcedure], "IsSystemObject")

$db = New-Object Microsoft.SqlServer.Management.Smo.Database
$db = $server.Databases[$databaseName]

Export-Data -Server $server -Database $db -OutFile ($outDir + '\Data.sql')
Export-Tables -Server $server -Database $db -OutFile ($outDir + '\Tables.sql')
Export-StoredProcedures -Server $server -Database $db -OutFile ($outDir + '\StoredProcedures.sql')

#Export-DbCreation -Database $db -OutFile ($outDir + '\Create.sql')
#Export-Views -Server $server -Database $db -OutFile ($outDir + '\Views.sql')
#Export-UserDefinedFunctions -Server $server -Database $db -OutFile ($outDir + '\UserDefinedFunctions.sql')
#Export-Triggers -Server $server -Database $db -OutFile ($outDir + '\Triggers.sql')
#Export-TableTriggers -Server $server -Database $db -OutFile ($outDir + '\TableTriggers.sql')
