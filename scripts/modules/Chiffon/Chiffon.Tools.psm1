#Requires -Version 3.0

#-- Variables privées --#

[string] $ToolsDirectory = $null
[xml] $Config = $null
[bool] $RequiresSavingState = $false

#-- Fonctions publiques --#

function Set-ToolsDirectory {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $value)

  $script:ToolsDirectory = $value

  return $ToolsDirectory
}

function Install-7Zip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $name = '7-Zip'
  $path = Get-ToolPath '7za.exe'

  if (Test-Installed -Name $name -Path $path -Version $version) { return }
  else { Remove-Item $path -Force }

  Install-FileFromZip -Source $source -SourceFile '7za.exe' -TargetFile $path

  Set-CurrentVersion $name $version
}

function Install-GoogleClosureCompiler {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $name = 'Google Closure Compiler'
  $path = Get-ToolPath 'closure-compiler.jar'

  if (Test-Installed -Name $name -Path $path -Version $version) { return }
  else { Remove-Item $path -Force }

  Install-FileFromZip -Source $source -SourceFile 'compiler.jar' -TargetFile $path

  Set-CurrentVersion $name $version
}

function Install-Node {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $name = 'NodeJS'
  $path = Get-ToolPath 'node.exe'

  if (Test-Installed -Name $name -Path $path -Version $version) { return }
  else { Remove-Item $path -Force }

  Install-File -Source $source -TargetFile $path

  Set-CurrentVersion $name $version
}

function Install-Npm {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $name = 'npm'
  $path = Get-ToolPath 'npm.cmd'

  if (Test-Installed -Name $name -Path $path -Version $version) { return }
  else {
    Remove-Item $path -Force
    $node_modules = Get-ToolPath 'node_modules'
    Remove-Item $node_modules -Force -Recurse
  }

  $extractPath = Get-ToolsDirectory
  Install-ZipFile -Source $source -ExtractPath $extractPath

  Set-CurrentVersion $name $version
}

function Install-NuGet {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $name = 'NuGet'
  $path = Get-ToolPath 'nuget.exe'

  if (Test-Installed -Name $name -Path $path -Version $version) { return }
  else { Remove-Item $path -Force }

  Install-File -Source $source -TargetFile $path

  Set-CurrentVersion $name $version
}

function Install-YuiCompressor {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $name = 'YUI Compressor'
  $path = Get-ToolPath 'yuicompressor.jar'

  if (Test-Installed -Name $name -Path $path -Version $version) { return }
  else { Remove-Item $path -Force }

  Install-FileFromZip -Source $source `
    -SourceFile 'yuicompressor-*\build\yuicompressor-*.jar' `
    -TargetFile $path

  Set-CurrentVersion $name $version
}

#-- Fonctions privées --#

function Get-CurrentVersion {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name
  )

  $config = Get-Config

  foreach ($elt in $config.configuration.add) {
    if ($elt.key -eq $name) {
      return $elt.value
    }
  }

  return $null
}

function Get-Config {
  if ($Config -ne $null) {
    return $Config
  }

  $configPath = Get-ToolPath 'tools.config'

  if (!(Test-Path $configPath)) {
    # Si le fichier de configuration n'existe pas, on le crée.
    $template = @'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
</configuration>
'@

    Add-Content $configPath $template
  }

  $script:Config = Get-Content -Path $configPath

  return $script:Config
}

function Download {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [System.Uri] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  if (Test-Path $outFile) { return }

  Write-Verbose "Downloading '$source'."

  Write-Host -NoNewline 'Downloading...'
  $wc = New-Object System.Net.WebClient
  $wc.DownloadFile($source, $outFile)
  Write-Host 'done'
}

function Get-ToolPath {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [System.Uri] $relativePath)

  $toolsDirectory = Get-ToolsDirectory

  return "$toolsDirectory\$relativePath"
}

function Get-ToolsDirectory {
  if ($script:ToolsDirectory -eq $null) {
    throw 'You must first initialize $ToolsDirectory via Set-ToolsDirectory.'
  }

  return $script:ToolsDirectory
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

function Install-FileFromZip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $sourceFile,
    [Parameter(Mandatory = $true, Position = 2)] [string] $targetFile
  )

  $tmpPath = Get-ToolPath 'tmp'
  if (Test-Path $tmpPath) { Remove-Item $tmpPath -Force -Recurse }
  New-Directory $tmpPath | Out-Null

  # Extraction dans un répertoire temporaire.
  Install-ZipFile -Source $source -ExtractPath $tmpPath
  # Installation.
  $file = "$tmpPath\$sourceFile"
  if (!(Test-Path $file)) {
    throw "The file '$sourceFile' does not exist."
  }
  Move-Item $file $targetFile
  # Suppression du répertoire temporaire.
  Remove-Item $tmpPath -Force -Recurse
}

function Install-ZipFile {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $extractPath
  )

  $distDir = Get-ToolPath 'dist' | New-Directory

  $uri = [System.Uri] $source
  $fileName = [System.IO.Path]::GetFileName($uri.AbsolutePath);
  $outFile = "$distDir\$fileName"

  Download -Source $uri -OutFile $outFile
  Unzip -File $outFile -ExtractPath $extractPath
}

function Save-State {
  if ($RequiresSavingState) {
    Write-Host "Saving state"
    $configPath = Get-ToolPath 'tools.config'
    $config = Get-Config

    $config.Save($configPath)
  }
}

function Set-CurrentVersion {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  [bool] $found = $false

  $config = Get-Config

  foreach ($elt in $config.configuration.add) {
    if ($elt.key -eq $name) {
      $elt.value = $version
      $found = $true
      break
    }
  }

  if (!$found) {
    # Si la configuration n'existait pas déjà, on la crée.
    $elt = $config.CreateElement('add')
    $elt.SetAttribute('key', $name)
    $elt.SetAttribute('value', $version)
    $config.configuration.AppendChild($elt) | Out-Null
  }

  $script:RequiresSavingState = $true
}

function Test-Installed {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $path,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  if (Test-Path $path) {
    $currentVersion = Get-CurrentVersion $name

    if ($currentVersion -eq $null) {
      Write-Host "No record found for $name. Forcing reinstallation..." -ForegroundColor 'Yellow'
      return $false
    } elseif ($version -ne $currentVersion) {
      Write-Host "Upgrading $name from v$currentVersion to v$version..." -ForegroundColor 'Yellow'
      return $false
    } else {
      Write-Host "$name v$version is already installed." -ForegroundColor 'Gray'
      return $true
    }
  } else {
    Write-Host "Installing $name..." -ForegroundColor 'Yellow'
    return $false
  }
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

#-- Directives --#

Export-ModuleMember -function Save-State, Set-ToolsDirectory, `
  Install-7Zip, Install-GoogleClosureCompiler, `
  Install-Node, Install-Npm, Install-NuGet, Install-YuiCompressor, Test-Tools
