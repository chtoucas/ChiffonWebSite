# Usage:
#   .\exportdb.ps1 "SERVERNAME" "DATABASE"
#   .\exportdb.ps1 "(localdb)\v11.0" "Chiffon"

param($serverName, $databaseName)

Import-Module ".\DBManip.psm1"

# Cf. http://technet.microsoft.com/en-us/library/hh847796.aspx
$errorActionPreference = "Stop"

$outDir = $(Get-Location).Path + '\exports\'
if (Test-Path -Path $outDir) {
  Remove-Item -Recurse $outDir | Out-Null
}
md $outDir | Out-Null

Export-DB -ServerName $serverName -DatabaseName $databaseName -OutDir $outDir
