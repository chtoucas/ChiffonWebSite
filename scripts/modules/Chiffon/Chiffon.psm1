#Requires -Version 3.0

Set-StrictMode -Version Latest

# On définit une seule variable globale.
$GLOBAL:Chiffon = @{}

if ($args.Length -gt 0) {
  if ($args[0] -is [hashtable]) {
    # FIXME: Vérifier le répertoire en question.
    $Chiffon.ProjectDirectory = $args[0].ProjectDirectory
  } else {
    throw "FIXME: Option not found"
  }
} else {
  throw "FIXME: No arg given"
}

$Chiffon.ToolsDirectory = "$($Chiffon.ProjectDirectory)\tools"
$Chiffon.NodeModulesDirectory = "$($Chiffon.ProjectDirectory)\scripts\node_modules"

Export-ModuleMember -Alias * -Function * -Cmdlet *

# Initialisation des modules
& {
  Write-Host "Loading the Chiffon modules."

  $mod = Get-Module Chiffon.Tools

  & $mod { Initialize }
}

$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
  Write-Host "Unloading the Chiffon modules."
}