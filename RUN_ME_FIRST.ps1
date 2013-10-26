#Requires -Version 3.0

Import-Module 'Chiffon'

New-Directory "$PSScriptRoot\tools" | Set-ToolsDirectory

Exit

# 7-Zip v9.20.
Install-7Zip 'http://downloads.sourceforge.net/sevenzip/7za920.zip'

# NuGet, dernière version.
Install-NuGet 'http://www.nuget.org/nuget.exe'

# Node v0.10.21.
Install-Node 'http://nodejs.org/dist/v0.10.21/node.exe'
# Node npm v1.3.9.
Install-Npm 'http://nodejs.org/dist/npm/npm-1.3.9.zip'

# Google Closure Compiler 2013/10/14.
# Cf. https://code.google.com/p/closure-compiler/wiki/BinaryDownloads
Install-GoogleClosureCompiler 'http://dl.google.com/closure-compiler/compiler-20131014.zip'

# YUI Compressor v2.4.7.
# WARNING: La version 2.4.8 ne gère pas correctement les chemins Windows.
#          Cf. https://github.com/yui/yuicompressor/issues/78
Install-YuiCompressor 'https://github.com/downloads/yui/yuicompressor/yuicompressor-2.4.7.zip'
