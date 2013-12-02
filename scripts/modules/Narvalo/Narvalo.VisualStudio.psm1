#Requires -Version 3.0

Set-StrictMode -Version Latest

# .SYNOPSIS
# Supprime les répertoires 'bin' and 'obj' créés par Visual Studio.
#
# .PARAMETER path
# Répertoire dans lequel résident les projets Visual Studio.
function Remove-VisualStudioTmpFiles {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $path)

  Get-ChildItem $path -Include bin,obj -Recurse |
    Where-Object { Remove-Item $_.FullName -Force -Recurse }
}

Export-ModuleMember -Function *