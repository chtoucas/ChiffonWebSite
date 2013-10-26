#Requires -Version 3.0

#-- Variables publiques --#

[string] $ToolsDirectory = $null

#-- Fonctions publiques --#

function Set-ToolsDirectory {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $value)

  $script:ToolsDirectory = $value

  return $ToolsDirectory
}

function Install-7Zip {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  $path = Get-ToolPath '7-zip'

  if (Test-Installed -Name '7-Zip' -Path $path) { return }

  Install-ZipFile -Source $source -ExtractPath $path
}

function Install-GoogleClosureCompiler {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  $path = Get-ToolPath 'closure-compiler'

  if (Test-Installed -Name 'Google Closure Compiler' -Path $path) { return }

  Install-ZipFile -Source $source -ExtractPath $path
}

function Install-Node {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  $path = Get-ToolPath 'node.exe'

  if (Test-Installed -Name 'NodeJS' -Path $path) { return }

  Install-File -Source $source -TargetFile $path
}

function Install-Npm {
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  $path = Get-ToolPath 'npm.cmd'

  if (Test-Installed -Name 'npm' -Path $path) { return }

  Install-ZipFile -Source $source -ExtractPath $ToolsDirectory
}

function Install-NuGet {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  $path = Get-ToolPath 'nuget.exe'

  if (Test-Installed -Name 'NuGet' -Path $path) { return }

  Install-File -Source $source -TargetFile $path
}

function Install-YuiCompressor {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  $path = Get-ToolPath 'yuicompressor.jar'

  if (Test-Installed -Name 'YUI Compressor' -Path $path) { return }

  $tmpPath = Get-ToolPath -RelativePath 'yuicompressor-*\build\yuicompressor-*.jar'

  if (!(Test-Path $tmpPath)) {
    Install-ZipFile -Source $source -ExtractPath $ToolsDirectory
  }

  Move-Item $tmpPath $path
  #Remove-Item $_.FullName -Force -Recurse
}

#-- Fonctions privées --#

function Get-ToolsDirectory {
  if ($ToolsDirectory -eq $null) {
    Write-Error 'You must initialize $ToolsDirectory'
    Exit
  }

  return $ToolsDirectory
}

function Get-ToolPath {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [System.Uri] $relativePath)

  $toolsDirectory = Get-ToolsDirectory
  "$toolsDirectory\$relativePath"
}

function Download {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [System.Uri] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  if (Test-Path $outFile) { return }

  Write-Host -NoNewline 'Downloading...'
  $wc = New-Object System.Net.WebClient
  $wc.DownloadFile($source, $outFile)
  Write-Host 'done'
}

function Unzip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $file,
    [Parameter(Mandatory = $true, Position = 1)] [string] $extractPath
  )

  Write-Host -NoNewline 'Unzipping...'
  [System.IO.Compression.ZipFile]::ExtractToDirectory($file, $extractPath)
  Write-Host 'done'
}

function Install-File {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $targetFile
  )

  $uri = [System.Uri] $source
  Download -Source $uri -OutFile $targetFile
}

function Install-ZipFile {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $extractPath
  )

  $distDir = New-Directory -Path "$ToolsDirectory\dist"

  $uri = [System.Uri] $source
  $fileName = [System.IO.Path]::GetFileName($uri.AbsolutePath);
  $outFile = "$distDir\$fileName"

  Download -Source $uri -OutFile $outFile
  Unzip -File $outFile -ExtractPath $extractPath
}

function Test-Installed {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $path
  )

  if (Test-Path $path) {
    Write-Host "'$name' already installed." -ForegroundColor "Gray"
    return $true
  } else {
    Write-Host "Installing '$name'..." -ForegroundColor "Yellow"
    return $false
  }
}

#-- Directives --#

Export-ModuleMember -function Set-ToolsDirectory, Install-7Zip, Install-GoogleClosureCompiler, `
  Install-Node, Install-Npm, Install-NuGet, Install-YuiCompressor, Test-Tools
