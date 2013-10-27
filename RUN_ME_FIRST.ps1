#Requires -Version 3.0

Get-Module Chiffon | Remove-Module
Import-Module Chiffon

New-Directory "$PSScriptRoot\tools" | Set-ToolsDirectory

#-- Installation ou mise à jour des outils --#

# 7-Zip v9.20.
Install-7Zip 'http://downloads.sourceforge.net/sevenzip/7za920.zip' -Version '9.20'
# NuGet, v2.7.1.
# FIXME: Malheureusement, je ne trouve pas de lien de téléchargement vers une version spécifique.
Install-NuGet 'http://www.nuget.org/nuget.exe' -Version '2.7.1'
# Node v0.10.21.
Install-Node 'http://nodejs.org/dist/v0.10.21/node.exe' -Version '0.10.21'
# Node npm v1.3.9.
Install-Npm 'http://nodejs.org/dist/npm/npm-1.3.9.zip' -Version '1.3.9'
# Google Closure Compiler 2013/10/14.
# Cf. https://code.google.com/p/closure-compiler/wiki/BinaryDownloads
Install-GoogleClosureCompiler 'http://dl.google.com/closure-compiler/compiler-20131014.zip' `
  -Version '20131014'
# YUI Compressor v2.4.7.
# WARNING: La version 2.4.8 ne gère pas correctement les chemins Windows.
#          Cf. https://github.com/yui/yuicompressor/issues/78
Install-YuiCompressor 'https://github.com/downloads/yui/yuicompressor/yuicompressor-2.4.7.zip' `
  -Version '2.4.7'

Save-State

#-- Restoration des packages NuGet --#

.\tools\NuGet.exe install .nuget\packages.config -OutputDirectory packages -Verbosity quiet