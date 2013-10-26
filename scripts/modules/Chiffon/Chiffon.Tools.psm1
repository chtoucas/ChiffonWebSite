
[System.Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem') | Out-Null

function GetToolPath {
  param([Parameter(Mandatory = $true)] [System.Uri] $relativePath)

  "$($tools.directory)\$($relativePath)"
}

function Download {
  param(
    [Parameter(Mandatory = $true)] [System.Uri] $source,
    [Parameter(Mandatory = $true)] [string] $outFile
  )

  if (Test-Path $outFile) { return }

  Write-Host -NoNewline 'Downloading...'
  $wc = New-Object System.Net.WebClient
  $wc.DownloadFile($source, $outFile)
  Write-Host 'done'
}

function Unzip {
  param(
    [Parameter(Mandatory = $true)] [string] $file,
    [Parameter(Mandatory = $true)] [string] $extractPath
  )

  Write-Host -NoNewline 'Unzipping...'
  [System.IO.Compression.ZipFile]::ExtractToDirectory($file, $extractPath)
  Write-Host 'done'
}

function Install-File {
  param(
    [Parameter(Mandatory = $true)] [string] $source,
    [Parameter(Mandatory = $true)] [string] $targetFile
  )

  $uri = [System.Uri] $source
  Download -Source $uri -OutFile $targetFile
}

function Install-ZipFile {
  param(
    [Parameter(Mandatory = $true)] [string] $source,
    [Parameter(Mandatory = $true)] [string] $extractPath
  )

  $distDir = New-Directory -Path "$($tools.directory)\dist"

  $uri = [System.Uri] $source
  $fileName = [System.IO.Path]::GetFileName($uri.AbsolutePath);
  $outFile = "$distDir\$fileName"

  Download -Source $uri -OutFile $outFile
  Unzip -File $outFile -ExtractPath $extractPath
}

function Test-Installed {
  param(
    [Parameter(Mandatory = $true)] [string] $name,
    [Parameter(Mandatory = $true)] [string] $path
  )

  if (Test-Path $path) {
    Write-Host "'$name' already installed." -ForegroundColor "Gray"
    return $true
  } else {
    Write-Host "Installing '$name'..." -ForegroundColor "Yellow"
    return $false
  }
}

function Install-7Zip {
  param([Parameter(Mandatory = $true)] [string] $source)

  $path = GetToolPath -RelativePath '7-zip'

  if (Test-Installed -Name '7-Zip' -Path $path) { return }

  Install-ZipFile -Source $source -ExtractPath $path
}

function Install-GoogleClosureCompiler {
  param([Parameter(Mandatory = $true)] [string] $source)

  $path = GetToolPath -RelativePath 'closure-compiler'

  if (Test-Installed -Name 'Google Closure Compiler' -Path $path) { return }

  Install-ZipFile -Source $source -ExtractPath $path
}

function Install-Node {
  param([Parameter(Mandatory = $true)] [string] $source)

  $path = GetToolPath -RelativePath 'node.exe'

  if (Test-Installed -Name 'nodejs' -Path $path) { return }

  Install-File -Source $source -TargetFile $path
}

function Install-Npm {
  param([Parameter(Mandatory = $true)] [string] $source)

  $path = GetToolPath -RelativePath 'npm.cmd'

  if (Test-Installed -Name 'npm' -Path $path) { return }

  Install-ZipFile -Source $source -ExtractPath $tools.directory
}

function Install-NuGet {
  param([Parameter(Mandatory = $true)] [string] $source)

  $path = GetToolPath -RelativePath 'nuget.exe'

  if (Test-Installed -Name 'NuGet' -Path $path) { return }

  Install-File -Source $source -TargetFile $path
}

function Install-YuiCompressor {
  param([Parameter(Mandatory = $true)] [string] $source)

  $path = GetToolPath -RelativePath 'yuicompressor.jar'

  if (Test-Installed -Name 'YUI Compressor' -Path $path) { return }

  $tmpPath = GetToolPath -RelativePath 'yuicompressor-*\build\yuicompressor-*.jar'

  if (!(Test-Path $tmpPath)) {
    Install-ZipFile -Source $source -ExtractPath $tools.directory
  }

  Copy-Item $tmpPath $path
}

$script:tools = @{}
$tools.directory = $null

Export-ModuleMember `
  -function Install-7Zip, Install-GoogleClosureCompiler, Install-Node, Install-Npm, `
            Install-NuGet, Install-YuiCompressor, Test-Tools `
  -variable tools
