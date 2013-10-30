#Requires -Version 3.0

Set-StrictMode -Version Latest

#Get-Module Chiffon | Remove-Module
Import-Module Chiffon

# On supprime les fichiers temporaires créés par Visual Studio.
Remove-VisualStudioTmpFiles "$PSScriptRoot\..\src"
Remove-VisualStudioTmpFiles "$PSScriptRoot\..\test"
