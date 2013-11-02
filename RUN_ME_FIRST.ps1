#Requires -Version 3.0

# TODO: Restauration des modules Node et des composants Bower dans le site web.

Set-StrictMode -Version Latest

#Get-Module Chiffon | Remove-Module
Import-Module Chiffon

# Configuration

$tools = @(
  @{
    'Name' = '7-zip'
    'Version' = '9.20'
    'Source' = 'http://downloads.sourceforge.net/sevenzip/7za920.zip'
  }
  # FIXME: Malheureusement, je ne trouve pas de lien de téléchargement vers une version spécifique.
  @{
    'Name' = 'NuGet'
    'Version' = '2.7.1'
    'Source' = 'http://www.nuget.org/nuget.exe'
  }
  @{
    'Name' = 'Node'
    'Version' = '0.10.21'
    'Source' = 'http://nodejs.org/dist/v0.10.21/node.exe'
  }
  @{
    'Name' = 'Node Package Manager'
    'Version' = '1.3.13'
    'Source' = 'http://nodejs.org/dist/npm/npm-1.3.13.zip'
  }
  # Cf. https://code.google.com/p/closure-compiler/wiki/BinaryDownloads
  @{
    'Name' = 'Google Closure Compiler'
    'Version' = '20131014'
    'Source' = 'http://dl.google.com/closure-compiler/compiler-20131014.zip'
  }
  # WARNING: La version 2.4.8 ne gère pas correctement les chemins Windows.
  #          Cf. https://github.com/yui/yuicompressor/issues/78
  @{
    'Name' = 'YUI Compressor'
    'Version' = '2.4.7'
    'Source' = 'https://github.com/downloads/yui/yuicompressor/yuicompressor-2.4.7.zip'
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

Write-Host 'Restoring Node.js modules.' -ForegroundColor 'Yellow'
.\tools\npm.cmd install
