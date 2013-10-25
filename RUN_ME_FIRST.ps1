# Outil de téléchargement des outils.

[System.Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem') | Out-Null

function Download {
  param(
    [Parameter(Mandatory = $true)] [string] $source,
    [Parameter(Mandatory = $true)] [string] $outFile
  )

  $wc = New-Object System.Net.WebClient
  $wc.DownloadFile($source, $outFile)
}

function Unzip {
  param(
    [Parameter(Mandatory = $true)] [string] $file,
    [Parameter(Mandatory = $true)] [string] $outDir
  )

  [System.IO.Compression.ZipFile]::ExtractToDirectory($file, $outDir)
}

function InstallExe {
  param(
    [Parameter(Mandatory = $true)] [string] $name,
    [Parameter(Mandatory = $true)] [string] $source,
    [Parameter(Mandatory = $true)] [string] $outFile
  )

  if (Test-Path $outFile) {
    Write-Host "'$name' already installed."
  } else {
    Write-Host "Installing '$name'..."
    Download -Source $source -OutFile $outFile
  }
}

function InstallZip {
  param(
    [Parameter(Mandatory = $true)] [string] $name,
    [Parameter(Mandatory = $true)] [string] $source,
    [Parameter(Mandatory = $true)] [string] $outFile,
    [Parameter(Mandatory = $true)] [string] $outDir
  )

  if (Test-Path $outFile) {
    Write-Host "'$name' already installed."
  } else {
    Write-Host "Installing '$name'..."
    Download -Source $source -OutFile $outFile
    Unzip -File $outFile -OutDir $outDir
  }
}

# Création des répertoires.

$toolsDir = "$PSScriptRoot\tools"
$distDir = "$toolsDir\dist"
$nodeDir = "$toolsDir\nodejs"

if (!(Test-Path $toolsDir)) {
  New-Item $toolsDir -Type directory | Out-Null
}
if (!(Test-Path $distDir)) {
  New-Item $distDir -Type directory | Out-Null
}
if (!(Test-Path $nodeDir)) {
  New-Item $nodeDir -Type directory | Out-Null
}

# 7-Zip v9.20.
InstallZip -Name '7-Zip' `
  -Source 'http://downloads.sourceforge.net/sevenzip/7za920.zip' `
  -OutFile "$distDir\7za920.zip" `
  -OutDir "$toolsDir\7-zip"

# Dernière version de NuGet.
InstallExe -Name 'NuGet' `
  -Source 'http://www.nuget.org/nuget.exe' `
  -OutFile "$toolsDir\nuget.exe"

# Node v0.10.21.
InstallExe -Name 'Node' `
  -Source 'http://nodejs.org/dist/v0.10.21/node.exe' `
  -OutFile "$nodeDir\node.exe"

# Npm v1.3.9.
InstallZip -Name 'Npm' `
  -Source 'http://nodejs.org/dist/npm/npm-1.3.9.zip' `
  -OutFile "$distDir\npm-1.3.9.zip" `
  -OutDir $nodeDir

# Google Closure Compiler 2013/10/14.
# Cf. https://code.google.com/p/closure-compiler/wiki/BinaryDownloads
InstallZip -Name 'Google Closure Compiler' `
  -Source 'http://dl.google.com/closure-compiler/compiler-20131014.zip' `
  -OutFile "$distDir\compiler-20131014.zip" `
  -OutDir "$toolsDir\closure-compiler"

# YUI Compressor v2.4.7.
# WARNING: Si on change de version, il faut mettre à jour 'scripts\build\Chiffon.props'.
InstallZip -Name 'YUI Compressor' `
  -Source 'https://github.com/downloads/yui/yuicompressor/yuicompressor-2.4.7.zip' `
  -OutFile "$distDir\yuicompressor-2.4.7.zip" `
  -OutDir $toolsDir
