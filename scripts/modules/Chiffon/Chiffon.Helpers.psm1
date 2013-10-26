# NB: Toutes les fonctions dans ce module sont publiques.

#Requires -Version 3.0

# .SYNOPSIS
# Returns the path to MSDeploy.
function Get-WebDeployInstallPath {
  return (Get-ChildItem "HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" | Select -last 1).GetValue("InstallPath")
}

# .SYNOPSIS
# Import WebAdministration Module OR Snapin.
# Borrowed from:
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
# Merge UTF-8 files.
#
# .DESCRIPTION
# This function merge UTF-8 even if they contain a BOM.
# If the result file already exists, override it.
#
# .PARAMETER inFiles
# List of files to merge.
#
# .PARAMETER outFile
# Specified the merged file name.
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
# Create a directory.
#
# .DESCRIPTION
# Create a directory if it does not already exist.
#
# .PARAMETER path
# Specified the path of the directory to create.
#
# .OUTPUTS
# System.String. The path of the directory.
function New-Directory {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  if (!(Test-Path $path)) {
    Write-Verbose 'Creating directory'
    New-Item $path -Type directory | Out-Null
  }

  return $path
}

# .SYNOPSIS
# Remove 'bin' and 'obj' directories created by Visual Studio.
#
# .PARAMETER path
# Specifies root path to Visual Studio projects.
function Remove-VisualStudioTmpFiles {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  Get-ChildItem $path -Include bin,obj -Recurse | foreach ($_) { Remove-Item $_.FullName -Force -Recurse }
}
