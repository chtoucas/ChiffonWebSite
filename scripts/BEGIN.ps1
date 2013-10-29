
$ErrorActionPreference = 'Stop'

function Initialize-Env {
  $toolsModule = Import-Module "$PSScriptRoot\modules\Chiffon\Chiffon.Tools.psm1" -PassThru
  $nodeModulesModule = Import-Module "$PSScriptRoot\modules\Chiffon\Chiffon.NodeModules.psm1" -PassThru

  & $toolsModule { $script:ToolsDirectory = "$PSScriptRoot\..\tools" }
  & $nodeModulesModule { $script:NodeModulesDirectory = "$PSScriptRoot\node_modules" }
}

#Initialize-Env

#New-Directory "$PSScriptRoot\..\tools" | Set-ToolsDirectory

