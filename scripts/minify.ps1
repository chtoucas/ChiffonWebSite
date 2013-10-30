#Requires -Version 3.0

Set-StrictMode -Version Latest

#Get-Module Chiffon | Remove-Module
Import-Module Chiffon

Exit

$global:ReportsDir = "$PSScriptRoot\logs"
if (!(Test-Path $ReportsDir)) { New-Item $ReportsDir -Type directory | Out-Null }

$assetRoot = "$PSScriptRoot\..\..\src\Chiffon.WebSite\assets"
$tmpDir = "$PSScriptRoot\tmp"

$cssfiles = @(
  "$assetRoot\css\normalize-1.1.3.css",
  "$assetRoot\css\01-chiffon.base.css",
  "$assetRoot\css\02-chiffon.helpers.css",
  "$assetRoot\css\03-chiffon.css"
)

$appjsfiles = @(
  "$assetRoot\js\vendor\yepnope-1.5.4.js",
  "$assetRoot\js\vendor\lodash.compat-2.0.0.js",
  "$assetRoot\js\app.js"
)

$jsfiles = @(
  "$assetRoot\js\jquery.plugins.js",
  "$assetRoot\js\vendor\l10n-2013.09.19.js",
  "$assetRoot\js\localization.js",
  "$assetRoot\js\chiffon.js"
)

MergeFiles -InFiles $appjsfiles -OutFile "$tmpDir\app.js"
MergeFiles -InFiles $jsfiles -OutFile "$tmpDir\chiffon.js"
MergeFiles -InFiles $cssfiles -OutFile "$tmpDir\chiffon.css"

Compress-JavaScript "$tmpDir\app.js" -OutFile "$tmpDir\app.min.js"
Compress-JavaScript "$tmpDir\chiffon.js" -OutFile "$tmpDir\chiffon.min.js"
Compress-Css "$tmpDir\chiffon.css" -OutFile "$tmpDir\chiffon.min.css"
