
#$env:PSModulePath = $env:PSModulePath + ";$PSScriptRoot\scripts\modules"

$ErrorActionPreference = 'Stop'

if ($PSVersionTable.PSVersion.Major -lt '3') {
  Write-Host 'Sorry, PowerShell v3.0 is required to run this script.' -ForegroundColor 'Red'
  Exit
}

Import-Module 'Chiffon'

$tools.directory = New-Directory -Path "$PSScriptRoot\tools"

# 7-Zip v9.20.
Install-7Zip -Source 'http://downloads.sourceforge.net/sevenzip/7za920.zip'

# NuGet, dernière version.
Install-NuGet -Source 'http://www.nuget.org/nuget.exe'

# Node v0.10.21.
Install-Node -Source 'http://nodejs.org/dist/v0.10.21/node.exe'
# Node npm v1.3.9.
Install-Npm -Source 'http://nodejs.org/dist/npm/npm-1.3.9.zip'

# Google Closure Compiler 2013/10/14.
# Cf. https://code.google.com/p/closure-compiler/wiki/BinaryDownloads
Install-GoogleClosureCompiler -Source 'http://dl.google.com/closure-compiler/compiler-20131014.zip'

# YUI Compressor v2.4.7.
# WARNING: La version 2.4.8 ne gère pas correctement les chemins Windows.
#          Cf. https://github.com/yui/yuicompressor/issues/78
Install-YuiCompressor `
  -Source 'https://github.com/downloads/yui/yuicompressor/yuicompressor-2.4.7.zip'
