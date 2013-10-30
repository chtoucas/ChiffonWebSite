#Requires -Version 3.0

Set-StrictMode -Version Latest

Add-Type -AssemblyName 'System.IO.Compression.FileSystem'

# .SYNOPSIS
# Décompresse un fichier au format ZIP.
#
# .PARAMETER path
# Chemin du fichier à décompresser.
function Expand-ZipArchive {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $file,
    [Parameter(Mandatory = $true, Position = 1)] [string] $extractPath
  )

  Write-Host -NoNewline 'Unzipping...'
  [System.IO.Compression.ZipFile]::ExtractToDirectory($file, $extractPath)
  Write-Host 'done'
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

# .SYNOPSIS
# Supprime un répertoire.
#
# .DESCRIPTION
# Supprime un répertoire (et son contenu) si il existe.
#
# .PARAMETER path
# Chemin du répertoire à supprimer.
#
# .OUTPUTS
# System.String. Chemin du répertoire.
function Remove-Directory {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  if (Test-Path $path) {
    Remove-Item $path -Force -Recurse
  }

  return $path
}

# .SYNOPSIS
# Supprime un fichier.
#
# .DESCRIPTION
# Supprime un fichier si il existe.
#
# .PARAMETER path
# Chemin du fichier à supprimer.
#
# .OUTPUTS
# System.String. Chemin du fichier.
function Remove-File {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  if (Test-Path $path) {
    Remove-Item $path -Force
  }

  return $path
}

Export-ModuleMember -Function *