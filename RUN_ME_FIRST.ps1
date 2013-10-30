#Requires -Version 3.0

Set-StrictMode -Version Latest

#Get-Module Chiffon | Remove-Module
Import-Module Chiffon

#-- Installation ou mise à jour des outils --#

Write-Host 'Restoring tools.' -ForegroundColor 'Yellow'

Install-Tool '7-Zip' '9.20' 'http://downloads.sourceforge.net/sevenzip/7za920.zip'

# FIXME: Malheureusement, je ne trouve pas de lien de téléchargement vers une version spécifique.
Install-Tool 'NuGet' '2.7.1' 'http://www.nuget.org/nuget.exe'

Install-Tool 'Node' '0.10.21' 'http://nodejs.org/dist/v0.10.21/node.exe'

Install-Tool 'Node Package Manager' '1.3.13' 'http://nodejs.org/dist/npm/npm-1.3.13.zip'

# Cf. https://code.google.com/p/closure-compiler/wiki/BinaryDownloads
Install-Tool 'Google Closure Compiler' '20131014' `
  'http://dl.google.com/closure-compiler/compiler-20131014.zip' `

# WARNING: La version 2.4.8 ne gère pas correctement les chemins Windows.
#          Cf. https://github.com/yui/yuicompressor/issues/78
Install-Tool 'YUI Compressor' '2.4.7' `
  'https://github.com/downloads/yui/yuicompressor/yuicompressor-2.4.7.zip'

#-- Restauration des packages NuGet --#

Write-Host 'Restoring NuGet packages.' -ForegroundColor 'Yellow'
.\tools\NuGet.exe install .nuget\packages.config -OutputDirectory packages -Verbosity quiet

#-- Installation ou mise à jour des modules Node.js --#

Write-Host 'Restoring Node.js modules.' -ForegroundColor 'Yellow'
.\tools\npm.cmd install