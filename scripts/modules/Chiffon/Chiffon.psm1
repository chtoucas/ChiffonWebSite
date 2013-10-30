#Requires -Version 3.0

Set-StrictMode -Version Latest

# TODO: S'inspirer de Pscx pour offrir la possibilité de ne charger que certains modules.

# On ne définit qu'une seule variable globale.
$script:Chiffon = @{}

$Chiffon.ProjectDirectory = "$PSScriptRoot\..\..\.."

Set-Variable -Name Chiffon -Value $Chiffon -Scope GLOBAL -Option ReadOnly

Export-ModuleMember -Alias * -Function * -Cmdlet *

# Initialisation des modules imbriqués.
# NB: Le module racine est chargé après les modules imbriqués. En conséquence, ces derniers n'ont
# pas accès à la variable globale $Chiffon lorsqu'ils sont importés.
& {
  & (Get-Module Chiffon.NodeModules) { Initialize $Chiffon.ProjectDirectory }
  & (Get-Module Chiffon.Tools) { Initialize $Chiffon.ProjectDirectory }
}

# Déchargement du module Chiffon.
$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
  Remove-Variable Chiffon -Scope GLOBAL -Force
}