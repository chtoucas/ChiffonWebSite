#Requires -Version 3.0

Import-Module Chiffon

# On supprime les fichiers temporaires créés par Visual Studio.
Remove-VisualStudioTmpFiles "$PSScriptRoot\..\src"
Remove-VisualStudioTmpFiles "$PSScriptRoot\..\test"
