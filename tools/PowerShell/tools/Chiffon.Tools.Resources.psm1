#Requires -Version 3.0

Set-StrictMode -Version Latest

function Publish-7Zip {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  Copy-ToolFromZip $source '7za.exe' (Get-ToolPath '7za.exe')
}

function Unpublish-7Zip {
  Get-ToolPath '7za.exe' | Remove-File | Out-Null
}

function Publish-Node {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  Copy-Item $source (Get-ToolPath 'node.exe')
}

function Unpublish-Node {
  Get-ToolPath 'node.exe' | Remove-File | Out-Null
}

function Publish-NodePackageManager {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  Expand-ZipArchive $source (Get-ToolPath '')
}

function Unpublish-NodePackageManager {
  Get-ToolPath 'npm.cmd' | Remove-File | Out-Null
  Get-ToolPath 'node_modules' | Remove-Directory | Out-Null
}

function Publish-NuGet {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $source)

  Copy-Item $source (Get-ToolPath 'nuget.exe')
}

function Unpublish-NuGet {
  Get-ToolPath 'nuget.exe' | Remove-File | Out-Null
}

# NB: Toutes ces méthodes doivent rester publiques car elles peuvent partie d'un
# bloc anonyme appelé dans un autre contexte.
Export-ModuleMember -function Publish-*, Unpublish-*
