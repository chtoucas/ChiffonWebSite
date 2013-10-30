#Requires -Version 3.0

Set-StrictMode -Version Latest

#Get-Module Chiffon | Remove-Module
Import-Module Chiffon

Exit

$global:ReportsDir = "$PSScriptRoot\reports"
if (!(Test-Path $ReportsDir)) { New-Item $ReportsDir -Type directory | Out-Null }

$assetRoot = "$PSScriptRoot\..\..\src\Chiffon.WebSite\assets"

LintJS -InFile "$assetRoot\js\app.js" -Name 'app'
LintJS -InFile "$assetRoot\js\chiffon.js" -Name 'chiffon'

LintCss -InFile "$assetRoot\css\01-chiffon.base.css" -Name 'base'
LintCss -InFile "$assetRoot\css\02-chiffon.helpers.css" -Name 'helpers'
LintCss -InFile "$assetRoot\css\03-chiffon.css" -Name 'main'
LintCss -InFile "$assetRoot\css\04-chiffon.responsive.css" -Name 'responsive'
LintCss -InFile "$assetRoot\css\05-chiffon.print.css" -Name 'print'
