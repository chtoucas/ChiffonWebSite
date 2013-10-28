#Requires -Version 3.0

Get-Module Chiffon | Remove-Module
Import-Module Chiffon

New-Directory "$PSScriptRoot\tools" | Set-ToolsDirectory

#-- Installation ou mise à jour des outils --#

Install-Tool -Name '7-Zip' -Version '9.20' 'http://downloads.sourceforge.net/sevenzip/7za920.zip'

# FIXME: Malheureusement, je ne trouve pas de lien de téléchargement vers une version spécifique.
Install-Tool -Name 'NuGet' -Version '2.7.1' 'http://www.nuget.org/nuget.exe'

Install-Tool -Name 'Node' -Version '0.10.21' 'http://nodejs.org/dist/v0.10.21/node.exe'

Install-Tool -Name 'Npm' -Version '1.3.9' 'http://nodejs.org/dist/npm/npm-1.3.9.zip'

# Cf. https://code.google.com/p/closure-compiler/wiki/BinaryDownloads
Install-Tool 'Google Closure Compiler' -Version '20131014' `
  'http://dl.google.com/closure-compiler/compiler-20131014.zip' `

# WARNING: La version 2.4.8 ne gère pas correctement les chemins Windows.
#          Cf. https://github.com/yui/yuicompressor/issues/78
Install-Tool 'YUI Compressor' -Version '2.4.7' `
  'https://github.com/downloads/yui/yuicompressor/yuicompressor-2.4.7.zip'

#-- Restauration des packages NuGet --#

Write-Host 'Restoring NuGet packages.' -ForegroundColor 'Yellow'
.\tools\NuGet.exe install .nuget\packages.config -OutputDirectory packages -Verbosity quiet