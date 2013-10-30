#Requires -Version 3.0

Set-StrictMode -Version Latest

# Pré-requis : nodejs
# Modules nodejs utilisés : clean-css, csslint, jshint, jslint, uglify-js

# --------------------------------------------------------------------------------------------------
# Variables privées
# --------------------------------------------------------------------------------------------------

[string] $script:NodeModulesDirectory = $null

function Initialize {
  param([Parameter(Mandatory = $true, Position = 0)] [string] $projectDirectory)

  $script:NodeModulesDirectory = "$projectDirectory\scripts\node_modules"
}

# --------------------------------------------------------------------------------------------------
# Fonctions publiques
# --------------------------------------------------------------------------------------------------

function Invoke-CleanCss {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  $cleancss = Get-NodeModuleBinPath 'cleancss.cmd'

  & $cleancss -d -o $outFile $source
}

function Invoke-CssLint {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $name
  )

  $logfile = "$ReportsDir\csslint-$name.log"
  if (Test-Path $logfile) { Remove-Item $logfile }

  $csslint = Get-NodeModuleBinPath 'csslint.cmd'

  & $csslint --quiet $source | Out-File $logfile
}

function Invoke-JSHint {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $name
  )

  $logfile = "$ReportsDir\jshint-$name.log"
  if (Test-Path $logfile) { Remove-Item $logfile }

  $jshint = Get-NodeModuleBinPath 'jshint.cmd'

  & $jshint $source | Out-File $logfile -Append
}

function Invoke-JSLint {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source)

  # NB: Pour une explication des erreurs jslint, cf. http://jslinterrors.com/

  # WARNING: node-jslint utilise console.log et lorsqu'on redirige la sortie dans une fenêtre
  # PowerShell, le fichier cible est vide :-( Apparemment je ne suis pas le seul à rencontrer
  # ce problème :
  #   http://stackoverflow.com/questions/9846326/node-console-log-behavior-and-windows-stdout
  # Pour contourner ce problème, on utilise donc un script intermédiaire :

  #FIXME .\jslint.cmd --maxerr 100 $source
}

#function LintJSViaUglifyJS {
#  [CmdletBinding()]
#  param([Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source)
#
#  $uglifyjs = Get-NodeModuleBinPath 'uglifyjs.cmd'
#
#  & $uglifyjs $source -b --lint | Out-Null
#}

function Invoke-UglifyJS {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  $uglifyjs = Get-NodeModuleBinPath 'uglifyjs.cmd'

  # TODO: source-map
  & $uglifyjs $source --compress --mangle --output $outFile
}

# --------------------------------------------------------------------------------------------------
# Fonctions privées
# --------------------------------------------------------------------------------------------------

function Get-NodeModuleBinPath {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [System.Uri] $relativePath)

  return "$($script:NodeModulesDirectory)\.bin\$relativePath"
}

# --------------------------------------------------------------------------------------------------
# Directives
# --------------------------------------------------------------------------------------------------

Export-ModuleMember -function Invoke-*
