#Requires -Version 3.0

# TODO: Restauration des composants Bower dans le site web.
# FIXME: Suppression des packages sans version avant la mise à jour (par ex. nuget).

Set-StrictMode -Version Latest

Get-Module Chiffon | Remove-Module
Import-Module Chiffon

# Configuration

$tools = @(
  @{
    'Name' = '7-zip'
    'Version' = '9.20'
    'Source' = 'http://downloads.sourceforge.net/sevenzip/7za920.zip'
  }
  # FIXME: Malheureusement, je ne trouve pas de lien de téléchargement
  # vers une version spécifique.
  @{
    'Name' = 'NuGet'
    'Version' = '2.8.3'
    'Source' = 'http://www.nuget.org/nuget.exe'
  }
)

#-- Installation ou mise à jour des outils --#

Write-Host 'Restoring tools.' -ForegroundColor 'Yellow'

foreach ($tool in $tools) {
  Install-Tool $tool.Name $tool.Version $tool.Source
}

#-- Restauration des packages NuGet --#

Write-Host 'Restoring NuGet packages.' -ForegroundColor 'Yellow'
.\tools\NuGet.exe install .nuget\packages.config -OutputDirectory packages -Verbosity quiet

#-- Installation ou mise à jour des modules Node.js --#

#Write-Host 'Restoring Node.js modules.' -ForegroundColor 'Yellow'
#.\tools\npm.cmd install

# Installation des exécutables nodes.

$modules = @(
  @{ 'Name' = 'grunt'; 'Command' = 'grunt-cli\bin\grunt' }
  @{ 'Name' = 'npm-check-updates'; 'Command' = 'npm-check-updates\bin\npm-check-updates' }
#  @{ 'Name' = 'tsc';   'Command' = 'typescript\bin\tsc' }
)

$template = @"
:: WARNING: Ne pas modifier ce fichier car il est généré automatiquement.
@echo off

node.exe "%~dp0\..\node_modules\{{command}}" %*
"@

foreach ($module in $modules) {
  $path = ".\tools\$($module.Name).cmd"

  if (!(Test-Path $path)) {
    Add-Content ".\tools\$($module.Name).cmd" $template.Replace('{{command}}', $module.Command)
  }
}
