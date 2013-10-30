#Requires -Version 3.0

Set-StrictMode -Version Latest

#Get-Module Chiffon | Remove-Module
Import-Module Chiffon

param($task = 'default')

Get-Module psake | Remove-Module
Import-Module (Get-ChildItem "$PSScriptRoot\..\packages\psake.*\tools\psake.psm1" | Select-Object -First 1)

exec { Invoke-psake "$PSScriptRoot\build\build.ps1" $task }