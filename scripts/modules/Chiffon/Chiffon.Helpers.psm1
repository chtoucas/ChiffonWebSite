#Requires -Version 3.0

# .SYNOPSIS
# Retourne le chemin vers MSDeploy.
function Get-WebDeployInstallPath {
  return (Get-ChildItem "HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" | Select -last 1).GetValue("InstallPath")
}

# .SYNOPSIS
# Importe WebAdministration, en tant que module ou snapin.
#
# .NOTES
# Copié depuis :
#   http://stackoverflow.com/questions/10700660/add-pssnapin-webadministration-in-windows7
function Import-WebAdministration {
  $moduleName = "WebAdministration"
  $loaded = $false

  if ($PSVersionTable.PSVersion.Major -ge 2) {
    if ((Get-Module -ListAvailable | ForEach-Object {$_.Name}) -contains $moduleName) {
      Import-Module $moduleName

      if ((Get-Module | ForEach-Object {$_.Name}) -contains $moduleName) {
        $loaded = $true
      }
    } elseif ((Get-Module | ForEach-Object {$_.Name}) -contains $moduleName) {
      $loaded = $true
    }
  }

  if (-not $loaded) {
    try {
      if ((Get-PSSnapin -Registered | ForEach-Object {$_.Name}) -contains $moduleName) {
        if ((Get-PSSnapin -Name $moduleName -ErrorAction SilentlyContinue) -eq $null) {
          Add-PSSnapin $moduleName
        }

        if ((Get-PSSnapin | ForEach-Object {$_.Name}) -contains $moduleName) {
          $loaded = $true
        }
      } elseif ((Get-PSSnapin | ForEach-Object {$_.Name}) -contains $moduleName) {
        $loaded = $true
      }
    } catch {
      Write-Error "`t`t$($MyInvocation.InvocationName): $_"
      Exit
    }
  }
}

# .SYNOPSIS
# Fusionne des fichiers UTF-8.
#
# .DESCRIPTION
# Cette fonction permet de fusionner des fichiers UTF-8(Y).
#
# Si un des fichiers à fusionner contient un BOM, ce dernier
# n'apparaîtra pas au milieu du fichier fusionné.
#
# Si le fichier fusionné existe déjà il est écrasé.
#
# .PARAMETER inFiles
# Liste des fichiers à fusionner.
#
# .PARAMETER outFile
# Nom du fichier fusionné.
function Merge-Utf8Files {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [array] $inFiles,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  if (Test-Path $outFile) { Remove-Item $outFile }

  $encoding = New-Object System.Text.UTF8Encoding($false)

  foreach ($filePath in $inFiles) {
    $content = [System.IO.File]::ReadAllLines($filePath)
    [System.IO.File]::AppendAllLines($outFile, $content, $encoding)
  }
}

# .SYNOPSIS
# Crée un répertoire.
#
# .DESCRIPTION
# Crée un répertoire si il n'existe pas déjà.
#
# .PARAMETER path
# Chemin du répertoire à créer.
#
# .OUTPUTS
# System.String. Chemin du répertoire.
function New-Directory {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  if (!(Test-Path $path)) {
    Write-Verbose "Creating directory '$path'."
    New-Item $path -Type directory | Out-Null
  }

  return $path
}

function Remove-Directory {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  if (Test-Path $path) {
    Remove-Item $path -Force -Recurse
  }

  return $path
}

function Remove-File {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  if (Test-Path $path) {
    Remove-Item $path -Force
  }

  return $path
}

# .SYNOPSIS
# Supprime les répertoires 'bin' and 'obj' créés par Visual Studio.
#
# .PARAMETER path
# Répertoire dans lequel résident les projets Visual Studio.
function Remove-VisualStudioTmpFiles {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  Get-ChildItem $path -Include bin,obj -Recurse |
    foreach ($_) { Remove-Item $_.FullName -Force -Recurse }
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
