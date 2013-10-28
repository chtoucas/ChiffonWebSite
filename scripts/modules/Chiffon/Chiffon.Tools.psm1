#Requires -Version 3.0

#-- Variables privées --#

[string] $ToolsDirectory = $null
[xml] $Config = $null

#-- Fonctions publiques --#

function Install-Tool  {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version,
    [Parameter(Mandatory = $true, Position = 2)] [string] $source,
    [Parameter(Mandatory = $false, Position = 3)] [scriptblock] $installCore = $null
  )

  $currentVersion = Get-CurrentVersion $name
  if ($currentVersion -eq $null) {
    Write-Host "Installing $name." -ForegroundColor 'Yellow'
  } elseif ($version -ne $currentVersion) {
    Write-Host "Upgrading $name from v$currentVersion to v$version." -ForegroundColor 'Yellow'
  } else {
    Write-Host "$name v$version is already installed." -ForegroundColor 'Gray'
    return
  }

  if (!$installCore) { $installCore = Get-InstallCore $name }
  [System.Uri] $source | Download | %{ & $installCore $_ }

  Set-CurrentVersion $name $version
}

function Uninstall-Tool {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $false, Position = 1)] [scriptblock] $uninstallCore = $null
  )

  Write-Host "Removing $name." -ForegroundColor 'Yellow'

  if (!$uninstallCore) { $uninstallCore = Get-UninstallCore $name }
  & $uninstallCore

  Remove-CurrentVersion $name
}

function Copy-ToolFromZip {
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

function Set-ToolsDirectory {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $value)

  $script:ToolsDirectory = $value
}

#-- Fonctions primaires de publication --#

function NormalizeName {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  return $name.Replace(' ', '').Replace('-', '')
}

function Get-InstallCore {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  $qname = NormalizeName $name

  return {
    param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source)

    $publish = "Publish-$qname"
    if (!(Test-Path Function:\$publish)) { throw "No publish method found for $name." }
    $unpublish = "Unpublish-$qname"
    if (!(Test-Path Function:\$unpublish)) { throw "No unpublish method found for $name." }

    & $unpublish
    & $publish $source
  }.GetNewClosure()
}

function Get-UninstallCore {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  $qname = NormalizeName $name

  $unpublish = "Unpublish-$qname"

  if (!(Test-Path Function:\$unpublish)) { throw "No unpublish method found for $name" }

  return {
    $unpublish = "Unpublish-$qname"
    if (!(Test-Path Function:\$unpublish)) { throw "No unpublish method found for $name." }

    & $unpublish
  }.GetNewClosure()
}

#-- Gestion des numéros de version --#

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

function Remove-CurrentVersion {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  [xml] $config = Get-Config

  $elt = $config.SelectSingleNode("/configuration/tools/add[@key='$name']")

  if ($elt -eq $null) { return }

  $elt.ParentNode.RemoveChild($elt) | Out-Null
  $configPath = Get-ToolPath 'tools.config'
  $config.Save($configPath)
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

#-- Utilitaires --#

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

#-- Directives --#

Export-ModuleMember -function Get-ToolsDirectory, Set-ToolsDirectory, Get-ToolPath, `
  Copy-ToolFromZip, Install-Tool, Uninstall-Tool
