#Requires -Version 3.0

Set-StrictMode -Version Latest

# --------------------------------------------------------------------------------------------------
# Variables privées
# --------------------------------------------------------------------------------------------------

# True si le module a été initialisé (via Initialize), False sinon.
[bool] $script:Initialized = $false
# Chemin absolu du répertoire contenant tous les outils.
[string] $script:ToolsDirectory = $null
# Chemin absolu du fichier d'état.
[string] $script:ToolsStatePath = $null
# Document XML contenant la liste des outils installés.
[xml] $script:ToolsState = $null

function Initialize {
  param([Parameter(Mandatory = $true, Position = 0)] [string] $projectDirectory)

  $script:ToolsDirectory = "$projectDirectory\tools"
  $script:ToolsStatePath = "$($script:ToolsDirectory)\tools.config"
  $script:Initialized = $true
}

# --------------------------------------------------------------------------------------------------
# Fonctions publiques
# --------------------------------------------------------------------------------------------------

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

  Assert-Initialized

  # Si le répertoire contenant les outils n'existe pas on le crée.
  New-Directory $script:ToolsDirectory | Out-Null

  $currentVersion = Read-CurrentVersion $name
  if ($currentVersion -eq $null) {
    Write-Host "Installing $name." -ForegroundColor 'Yellow'
  } elseif ($version -ne $currentVersion) {
    Write-Host "Upgrading $name from v$currentVersion to v$version." -ForegroundColor 'Yellow'
  } else {
    Write-Host "$name v$version is already installed." -ForegroundColor 'Gray'
    return
  }

  # On supprime la source locale (important au cas où celle-ci ne contient d'information de version)
  Remove-Distfile $source

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

  Assert-Initialized

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

  Assert-Initialized

  $tmpPath = Get-ToolPath 'tmp' | Remove-Directory | New-Directory

  Expand-ZipArchive $file $tmpPath

  Move-Item "$tmpPath\$source" -Destination $destination

  Remove-Directory $tmpPath | Out-Null
}

function Get-ToolPath {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [System.Uri] $relativePath)

  Assert-Initialized

  return "$($script:ToolsDirectory)\$relativePath"
}

# --------------------------------------------------------------------------------------------------
# Fonctions privées
# --------------------------------------------------------------------------------------------------

function Assert-Initialized {
  if (!$script:Initialized) {
    throw 'You forgot to initialize Chiffon.Tools.'
  }
}

# .SYNOPSIS
# Normalise le nom d'un outil afin de pouvoir l'utiliser dans un nom de fonction.
function Format-ToolName {
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  # On supprime les espaces et les tirets.
  return $name.Replace(' ', '').Replace('-', '')
}

# .SYNOPSIS
# Crée un processus d'installation par défaut.
function New-InstallCore {
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
  if ($script:ToolsState -ne $null) {
    return $script:ToolsState
  } else {
    $script:ToolsState = Read-ToolsState

    return $script:ToolsState
  }
}

function Initialize-ToolsState {
  $template = @'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <tools>
  </tools>
</configuration>
'@

  Add-Content $script:ToolsStatePath $template
}

function Read-ToolsState {
  if (!(Test-Path $script:ToolsStatePath)) {
    Initialize-ToolsState
  }

  return [xml] (Get-Content -Path $script:ToolsStatePath)
}

function Save-ToolsState {
  [xml] $state = Get-ToolsState
  $state.Save($script:ToolsStatePath)
}

#-- Gestion des numéros de version --#

function Read-Config {
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  [xml] $state = Get-ToolsState
  return $state.SelectSingleNode("/configuration/tools/add[@key='$name']")
}

function Read-CurrentVersion {
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  $elt = Read-Config $name

  if ($elt -eq $null) {
    return $null
  } else {
    return $elt.value
  }
}

function Unregister-Tool {
  param([Parameter(Mandatory = $true, Position = 0)] [string] $name)

  $elt = Read-Config $name

  if ($elt -eq $null) { return }

  $elt.ParentNode.RemoveChild($elt) | Out-Null

  # Sauvegarde.
  Save-ToolsState
}

function Register-Tool {
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

function Remove-Distfile {
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [System.Uri] $source)

  $distDir = Get-ToolPath 'dist' | New-Directory

  $fileName = [System.IO.Path]::GetFileName($source.AbsolutePath)
  $outFile = "$distDir\$fileName"

  if (Test-Path $outFile) { Remove-File $outFile | Out-Null }
}

# --------------------------------------------------------------------------------------------------
# Directives
# --------------------------------------------------------------------------------------------------

Export-ModuleMember -Function Copy-ToolFromZip, Get-ToolPath, Install-Tool, Uninstall-Tool
