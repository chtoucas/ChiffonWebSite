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

# Initialisation des modules.
# NB: Le module racine est chargé après les modules imbriqués. En conséquence, ces derniers n'ont
# pas accès à la variable globale $Chiffon.
& {
  Write-Verbose "Loading the Chiffon modules."

  & (Get-Module Chiffon.Tools) { Initialize }
}

$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
  Write-Verbose "Unloading the Chiffon modules."
}