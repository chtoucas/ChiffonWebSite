#Requires -Version 3.0

#-- Variables privées --#

[string] $ToolsDirectory = $null
[xml] $ToolsState = $null

#-- Fonctions publiques --#

# .SYNOPSIS
# Installe un outil dans le projet.
function Install-Tool  {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version,
    [Parameter(Mandatory = $true, Position = 2)] [string] $source,
    [Parameter(Mandatory = $false, Position = 3)] [scriptblock] $installCore = $null
  )

  $currentVersion = Read-CurrentVersion $name
  if ($currentVersion -eq $null) {
    Write-Host "Installing $name." -ForegroundColor 'Yellow'
  } elseif ($version -ne $currentVersion) {
    Write-Host "Upgrading $name from v$currentVersion to v$version." -ForegroundColor 'Yellow'
  } else {
    Write-Host "$name v$version is already installed." -ForegroundColor 'Gray'
    return
  }

  if (!$installCore) { $installCore = New-InstallCore $name }
  [System.Uri] $source | Download-Tool | %{ & $installCore $_ }

  Register-Tool $name $version
}

# .SYNOPSIS
# Supprime un outil du projet.
function Uninstall-Tool {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $false, Position = 1)] [scriptblock] $uninstallCore = $null
  )

  Write-Host "Removing $name." -ForegroundColor 'Yellow'

  if (!$uninstallCore) { $uninstallCore = New-UninstallCore $name }
  & $uninstallCore

  Unregister-Tool $name
}

# .SYNOPSIS
# Copie un fichier inclus dans un fichier ZIP.
function Copy-ToolFromZip {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $file,
    [Parameter(Mandatory = $true, Position = 1)] [string] $source,
    [Parameter(Mandatory = $true, Position = 2)] [string] $destination
  )

  $tmpPath = Get-ToolPath 'tmp' | Remove-Directory | New-Directory

  Expand-ZipFile $file $tmpPath

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

#-- Fonctions privées --#

# .SYNOPSIS
# Normalise le nom d'un outil afin de pouvoir l'utiliser dans un nom de fonction.
function Format-ToolName {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  # On supprime les espaces et les tirets.
  return $name.Replace(' ', '').Replace('-', '')
}

# .SYNOPSIS
# Crée un processus d'installation par défaut.
function New-InstallCore {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  $qname = Format-ToolName $name

  return {
    param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source)

    # WARNING: Les tests suivants doivent rester dans ce bloc car ils dépendent du contexte.
    $publish = "Publish-$qname"
    if (!(Test-Path Function:\$publish)) { throw "No 'publish' function found for $name." }
    $unpublish = "Unpublish-$qname"
    if (!(Test-Path Function:\$unpublish)) { throw "No 'unpublish' function found for $name." }

    & $unpublish
    & $publish $source
  }.GetNewClosure()
}

# .SYNOPSIS
# Crée un processus de suppression par défaut.
function New-UninstallCore {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  $qname = Format-ToolName $name

  return {
    # WARNING: Les tests suivants doivent rester dans ce bloc car ils dépendent du contexte.
    $unpublish = "Unpublish-$qname"
    if (!(Test-Path Function:\$unpublish)) { throw "No 'unpublish' method found for $name." }

    & $unpublish
  }.GetNewClosure()
}

#-- Gestion de la persistence --#

function Get-ToolsState {
  if ($ToolsState -ne $null) {
    return $ToolsState
  } else {
    $script:ToolsState = Read-ToolsState

    return $ToolsState
  }
}

function Get-ToolsStatePath {
  return Get-ToolPath 'tools.config'
}

function Initialize-ToolsState {
  $path = Get-ToolsStatePath

  $template = @'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <tools>
  </tools>
</configuration>
'@

  Add-Content $path $template
}

function Read-ToolsState {
  $path = Get-ToolsStatePath

  if (!(Test-Path $path)) {
    Initialize-ToolsState
  }

  return [xml] (Get-Content -Path $path)
}

function Save-ToolsState {
  [xml] $state = Get-ToolsState
  $state.Save((Get-ToolsStatePath))
}

#-- Gestion des numéros de version --#

function Read-Config {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  [xml] $state = Get-ToolsState
  return $state.SelectSingleNode("/configuration/tools/add[@key='$name']")
}

function Read-CurrentVersion {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  $elt = Read-Config $name

  if ($elt -eq $null) {
    return $null
  } else {
    return $elt.value
  }
}

function Unregister-Tool {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  $elt = Read-Config $name

  if ($elt -eq $null) { return }

  $elt.ParentNode.RemoveChild($elt) | Out-Null

  # Sauvegarde.
  Save-ToolsState
}

function Register-Tool {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $name,
    [Parameter(Mandatory = $true, Position = 1)] [string] $version
  )

  $elt = Read-Config $name

  if ($elt -eq $null) {
    # Création.
    [xml] $state = Get-ToolsState
    $elt = $state.CreateElement('add')
    $elt.SetAttribute('key', $name)
    $elt.SetAttribute('value', $version)
    $state.SelectSingleNode('/configuration/tools').AppendChild($elt) | Out-Null
  } else {
    # Mise à jour.
    $elt.value = $version
  }

  # Sauvegarde.
  Save-ToolsState
}

#-- Utilitaires --#

function Download-Tool {
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
