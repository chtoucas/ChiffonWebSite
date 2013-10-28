#Requires -Version 3.0

#-- Variables privées --#

[string] $ToolsDirectory = $null
[xml] $Config = $null

#-- Fonctions publiques --#

function Install-7Zip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  Invoke-Install -Name '7-Zip' -Version $version -Source $source {
    Remove-7Zip -Soft
    Copy-FileFromZip $_ '7za.exe' (Get-ToolPath '7za.exe')
  }
}

function Remove-7Zip {
  [CmdletBinding()]
  param([Parameter(Mandatory = $false, Position = 0)] [switch] $soft = $false)

  Get-ToolPath '7za.exe' | Remove-File | Out-Null

  if (!$soft) {
    Write-Host 'Removing 7-Zip.' -ForegroundColor 'Yellow'
    Delete-CurrentVersion '7-Zip'
  }
}

function Install-GoogleClosureCompiler {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  Invoke-Install -Name 'Google Closure Compiler' -Version $version -Source $source {
    Remove-GoogleClosureCompiler -Soft
    Copy-FileFromZip $_ 'compiler.jar' (Get-ToolPath 'closure-compiler.jar')
  }
}

function Remove-GoogleClosureCompiler {
  [CmdletBinding()]
  param([Parameter(Mandatory = $false, Position = 0)] [switch] $soft = $false)

  Get-ToolPath 'closure-compiler.jar' | Remove-File | Out-Null

  if (!$soft) {
    Write-Host 'Removing Google Closure Compiler.' -ForegroundColor 'Yellow'
    Delete-CurrentVersion 'Google Closure Compiler'
  }
}

function Install-Node {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  Invoke-Install -Name 'NodeJS' -Version $version -Source $source {
    Remove-Node -Soft
    Copy-Item $_ (Get-ToolPath 'node.exe')
  }
}

function Remove-Node {
  [CmdletBinding()]
  param([Parameter(Mandatory = $false, Position = 0)] [switch] $soft = $false)

  Get-ToolPath 'node.exe' | Remove-File | Out-Null

  if (!$soft) {
    Write-Host 'Removing NodeJS.' -ForegroundColor 'Yellow'
    Delete-CurrentVersion 'NodeJS'
  }
}

function Install-Npm {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  Invoke-Install -Name 'npm' -Version $version -Source $source {
    Remove-Npm -Soft
    Unzip $_ -ExtractPath (Get-ToolsDirectory)
  }
}

function Remove-Npm {
  [CmdletBinding()]
  param([Parameter(Mandatory = $false, Position = 0)] [switch] $soft = $false)

  Get-ToolPath 'npm.cmd' | Remove-File | Out-Null
  Get-ToolPath 'node_modules' | Remove-Directory | Out-Null

  if (!$soft) {
    Write-Host 'Removing npm.' -ForegroundColor 'Yellow'
    Delete-CurrentVersion 'npm'
  }
}

function Install-NuGet {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  Invoke-Install -Name 'NuGet' -Version $version -Source $source {
    Remove-NuGet -Soft
    Copy-Item $_ (Get-ToolPath 'nuget.exe')
  }
}

function Remove-NuGet {
  [CmdletBinding()]
  param([Parameter(Mandatory = $false, Position = 0)] [switch] $soft = $false)

  Get-ToolPath 'nuget.exe' | Remove-File | Out-Null

  if (!$soft) {
    Write-Host 'Removing Nuget.' -ForegroundColor 'Yellow'
    Delete-CurrentVersion 'NuGet'
  }
}

function Install-YuiCompressor {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  Invoke-Install -Name 'YUI Compressor' -Version $version -Source $source {
    Remove-YuiCompressor -Soft
    Copy-FileFromZip $_ 'yuicompressor-*\build\yuicompressor-*.jar' `
      (Get-ToolPath 'yuicompressor.jar')
  }
}

function Remove-YuiCompressor {
  [CmdletBinding()]
  param([Parameter(Mandatory = $false, Position = 0)] [switch] $soft = $false)

  Get-ToolPath 'yuicompressor.jar' | Remove-File | Out-Null

  if (!$soft) {
    Write-Host 'Removing YUI Compressor.' -ForegroundColor 'Yellow'
    Delete-CurrentVersion 'YUI Compressor'
  }
}

function Set-ToolsDirectory {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $value)

  $script:ToolsDirectory = $value
}

#-- Fonctions privées --#

function Copy-FileFromZip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $file,
    [Parameter(Mandatory = $true, Position = 1)] [string] $source,
    [Parameter(Mandatory = $true, Position = 2)] [string] $destination
  )

  $tmpPath = Get-ToolPath 'tmp' | Remove-Directory | New-Directory

  Write-Host -NoNewline 'Unzipping...'
  [System.IO.Compression.ZipFile]::ExtractToDirectory($file, $tmpPath)
  Write-Host 'done'

  "$tmpPath\$source" | Move-Item -Destination $destination

  Remove-Directory $tmpPath | Out-Null
}

function Delete-CurrentVersion {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  [xml] $config = Get-Config

  $elt = $config.SelectSingleNode("/configuration/tools/add[@key='$name']")

  if ($elt -eq $null) { return }

  $elt.ParentNode.RemoveChild($elt) | Out-Null
  $configPath = Get-ToolPath 'tools.config'
  $config.Save($configPath)
}

function Download {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [System.Uri] $source)

  $distDir = Get-ToolPath 'dist' | New-Directory

  $fileName = [System.IO.Path]::GetFileName($source.AbsolutePath)
  $outFile = "$distDir\$fileName"

  if (Test-Path $outFile) { return $outFile }

  Write-Host -NoNewline 'Downloading...'
  $wc = New-Object System.Net.WebClient
  $wc.DownloadFile($source, $outFile)
  Write-Host 'done'

  return $outFile
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
  <tools>
  </tools>
</configuration>
'@

    Add-Content $configPath $template
  }

  $script:Config = [xml] (Get-Content -Path $configPath)

  return $Config
}

function Get-CurrentVersion {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  [xml] $config = Get-Config

  $elt = $config.SelectSingleNode("/configuration/tools/add[@key='$name']")

  if ($elt -eq $null) {
    return $null
  } else {
    return $elt.value
  }
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

function Invoke-Install  {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version,
    [Parameter(Mandatory = $true, Position = 2)] [string] $source,
    [Parameter(Mandatory = $true, Position = 3)] [scriptblock] $installCore
  )

  if (Test-Installed -Name $name -Version $version) { return }

  [System.Uri] $source | Download | %{ & $installCore $_ }

  Set-CurrentVersion $name $version
}

function Set-CurrentVersion {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  [xml] $config = Get-Config

  $elt = $config.SelectSingleNode("/configuration/tools/add[@key='$name']")

  if ($elt -eq $null) {
    $elt = $config.CreateElement('add')
    $elt.SetAttribute('key', $name)
    $elt.SetAttribute('value', $version)
    $tools = $config.SelectSingleNode('/configuration/tools')
    $tools.AppendChild($elt) | Out-Null
  } else {
    $elt.value = $version
  }

  $configPath = Get-ToolPath 'tools.config'
  $config.Save($configPath)
}

function Test-Installed {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $currentVersion = Get-CurrentVersion $name

  if ($currentVersion -eq $null) {
    Write-Host "Installing $name." -ForegroundColor 'Yellow'
    return $false
  } elseif ($version -ne $currentVersion) {
    Write-Host "Upgrading $name from v$currentVersion to v$version." -ForegroundColor 'Yellow'
    return $false
  } else {
    Write-Host "$name v$version is already installed." -ForegroundColor 'Gray'
    return $true
  }
}

function Unzip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $file,
    [Parameter(Mandatory = $true, Position = 1)] [string] $extractPath
  )

  Write-Host -NoNewline 'Unzipping...'
  [System.IO.Compression.ZipFile]::ExtractToDirectory($file, $extractPath)
  Write-Host 'done'
}

#-- Directives --#

Export-ModuleMember -function Set-ToolsDirectory, Install-*, Remove-*
