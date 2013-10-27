#Requires -Version 3.0

#-- Variables privées --#

[string] $ToolsDirectory = $null
[xml] $Config = $null
[bool] $RequiresSavingState = $false

#-- Fonctions publiques --#

function Install-7Zip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $targetFile = Get-ToolPath '7za.exe'

  InstallFileFromZip -Source $source -SourceFile '7za.exe' -TargetFile $targetFile `
    -Name '7-Zip' -Version $version
}

function Install-GoogleClosureCompiler {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $targetFile = Get-ToolPath 'closure-compiler.jar'

  InstallFileFromZip -Source $source -SourceFile 'compiler.jar' -TargetFile $targetFile `
    -Name 'Google Closure Compiler' -Version $version
}

function Install-Node {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $targetFile = Get-ToolPath 'node.exe'

  InstallFile -Source $source -TargetFile $targetFile -Name 'NodeJS' -Version $version
}

function Install-Npm {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $targetFile = Get-ToolPath 'npm.cmd'
  $extractPath = Get-ToolsDirectory

  InstallZipFile -Source $source -TargetFile $targetFile -ExtractPath $extractPath `
    -Name 'npm' -Version $version {
    Get-ToolPath 'node_modules' | Where-Object { Test-Path $_ } | Remove-Item -Force -Recurse
  }
}

function Install-NuGet {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $targetFile = Get-ToolPath 'nuget.exe'

  InstallFile -Source $source -TargetFile $targetFile -Name 'NuGet' -Version $version
}

function Install-YuiCompressor {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $targetFile = Get-ToolPath 'yuicompressor.jar'

  InstallFileFromZip -Source $source `
    -SourceFile 'yuicompressor-*\build\yuicompressor-*.jar' `
    -TargetFile $targetFile `
    -Name 'YUI Compressor' `
    -Version $version
}

function Save-State {
  if ($RequiresSavingState) {
    Write-Host "Saving state"
    $configPath = Get-ToolPath 'tools.config'
    $config = Get-Config

    $config.Save($configPath)
  }
}

function Set-ToolsDirectory {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $value)

  $script:ToolsDirectory = $value
}

#-- Fonctions privées --#

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

function DownloadAndUnzip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [System.Uri] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $extractPath
  )

  $distDir = Get-ToolPath 'dist' | New-Directory

  $fileName = [System.IO.Path]::GetFileName($source.AbsolutePath);
  $outFile = "$distDir\$fileName"

  Download -Source $source -OutFile $outFile
  Unzip -File $outFile -ExtractPath $extractPath
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

  $script:Config = [xml] (Get-Content -Path $configPath)

  return $Config
}

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

function Get-ToolPath {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [System.Uri] $relativePath)

  $toolsDirectory = Get-ToolsDirectory

  return "$toolsDirectory\$relativePath"
}

function Get-ToolsDirectory {
  if ($ToolsDirectory -eq $null) {
    throw 'You must first initialize $ToolsDirectory via Set-ToolsDirectory.'
  }

  return $ToolsDirectory
}

function InstallFile {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $targetFile,
    [Parameter(Mandatory = $true, Position = 2)] [string] $name,
    [Parameter(Mandatory = $true, Position = 3)] [string] $version,
    [Parameter(Mandatory = $false, Position = 4)] [scriptblock] $remove
  )

  if (Test-Installed -Name $name -Path $targetFile -Version $version) {
    return
  } else {
    $targetFile | Where-Object { Test-Path $_ } | Remove-Item -Force
    if ($remove -ne $null) {
      & $remove
    }
  }

  $uri = [System.Uri] $source
  Download -Source $uri -OutFile $targetFile

  # On sauvegarde la version.
  Set-CurrentVersion $name $version
}

function InstallFileFromZip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $sourceFile,
    [Parameter(Mandatory = $true, Position = 2)] [string] $targetFile,
    [Parameter(Mandatory = $true, Position = 3)] [string] $name,
    [Parameter(Mandatory = $true, Position = 4)] [string] $version,
    [Parameter(Mandatory = $false, Position = 5)] [scriptblock] $remove
  )

  if (Test-Installed -Name $name -Path $targetFile -Version $version) {
    return
  } else {
    $targetFile | Where-Object { Test-Path $_ } | Remove-Item -Force
    if ($remove -ne $null) {
      & $remove
    }
  }

  $tmpPath = Get-ToolPath 'tmp'
  if (Test-Path $tmpPath) { Remove-Item $tmpPath -Force -Recurse }
  New-Directory $tmpPath | Out-Null

  # Extraction dans un répertoire temporaire.
  $uri = [System.Uri] $source
  DownloadAndUnzip -Source $uri -ExtractPath $tmpPath

  # Installation.
  $file = "$tmpPath\$sourceFile"
  if (!(Test-Path $file)) {
    throw "The file '$sourceFile' does not exist."
  }
  Move-Item $file $targetFile

  # On sauvegarde la version.
  Set-CurrentVersion $name $version

  # Suppression du répertoire temporaire.
  Remove-Item $tmpPath -Force -Recurse
}

function InstallZipFile {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $targetFile,
    [Parameter(Mandatory = $true, Position = 2)] [string] $extractPath,
    [Parameter(Mandatory = $true, Position = 3)] [string] $name,
    [Parameter(Mandatory = $true, Position = 4)] [string] $version,
    [Parameter(Mandatory = $false, Position = 5)] [scriptblock] $remove
  )

  if (Test-Installed -Name $name -Path $targetFile -Version $version) {
    return
  } else {
    $targetFile | Where-Object { Test-Path $_ } | Remove-Item -Force
    if ($remove -ne $null) {
      & $remove
    }
  }

  $uri = [System.Uri] $source
  DownloadAndUnzip -Source $uri -ExtractPath $extractPath

  # On sauvegarde la version.
  Set-CurrentVersion $name $version
}

function Set-CurrentVersion {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  [bool] $found = $false
  [xml] $config = Get-Config

  foreach ($elt in $config.configuration.add) {
    if ($elt.key -eq $name) {
      $elt.value = $version
      $found = $true
      break
    }
  }

  if (!$found) {
    # Si la configuration n'existe pas déjà, on la crée.
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

Export-ModuleMember -function Save-State, Set-ToolsDirectory, Install-*
