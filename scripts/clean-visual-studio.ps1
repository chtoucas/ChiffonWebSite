#Requires -Version 3.0

Set-StrictMode -Version Latest

#Get-Module Narvalo | Remove-Module
Import-Module Narvalo

# On supprime les fichiers temporaires créés par Visual Studio.
Remove-VisualStudioTmpFiles "$PSScriptRoot\..\src"
Remove-VisualStudioTmpFiles "$PSScriptRoot\..\test"
