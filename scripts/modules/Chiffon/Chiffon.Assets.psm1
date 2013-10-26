#Requires -Version 3.0

# Pré-requis : nodejs
# Modules nodejs utilisés : clean-css, csslint, jshint, jslint, uglify-js
# TODO: Google Closure Tools (utiliser IronPython via NuGet ?)

#-- Variables publiques --#

$script:assets = @{}
$assets.binPath = $null
$assets.reportsDir = $null

#-- Fonctions publiques --#

function LintCss {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $inFile,
    [Parameter(Mandatory = $true, Position = 1)] [string] $name
  )

  Write-Output "-> Processing $name.css"

  LintCssViaCssLint -InFile $inFile -Name $name
}

function LintJS {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $inFile,
    [Parameter(Mandatory = $true, Position = 1)] [string] $name
  )

  Write-Output "-> Processing $name.js"

  #LintJSViaUglifyJS -InFile $inFile
  LintJSViaJSHint -InFile $inFile -Name $name
  LintJSViaJSLint -InFile $inFile
}

function MinifyCss {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $inFile,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile,
    [Parameter(Mandatory = $true, Position = 2)] [string] $name
  )

  Write-Output "-> Processing $name.css"

  MinifyCssViaCleanCss -InFile $inFile -OutFile $outFile
}

function MinifyJS {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $inFile,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile,
    [Parameter(Mandatory = $true, Position = 2)] [string] $name
  )

  Write-Output "-> Processing $name.js"

  MinifyJSViaUglifyJS -InFile $inFile -OutFile $outFile
}

#-- Fonctions privées --#

function GetModulePath {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [System.Uri] $relativePath)

  "$($assets.binPath)\$($relativePath)"
}

function LintCssViaCssLint {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $inFile,
    [Parameter(Mandatory = $true, Position = 1)] [string] $name
  )

  $logfile = "$ReportsDir\csslint-$name.log"
  if (Test-Path $logfile) { Remove-Item $logfile }

  $csslint = GetModulePath -RelativePath 'csslint.cmd'

  & $csslint --quiet $inFile | Out-File $logfile
}

function LintJSViaJSHint {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $inFile,
    [Parameter(Mandatory = $true, Position = 1)] [string] $name
  )

  $logfile = "$ReportsDir\jshint-$name.log"
  if (Test-Path $logfile) { Remove-Item $logfile }

  $jshint = GetModulePath -RelativePath 'jshint.cmd'

  & $jshint $inFile | Out-File $logfile -Append
}

function LintJSViaJSLint {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $inFile)

  # NB: Pour une explication des erreurs jslint, cf. http://jslinterrors.com/

  # WARNING: node-jslint utilise console.log et lorsqu'on redirige la sortie dans une fenêtre
  # PowerShell, le fichier cible est vide :-( Apparemment je ne suis pas le seul à rencontrer
  # ce problème :
  #   http://stackoverflow.com/questions/9846326/node-console-log-behavior-and-windows-stdout
  # Pour contourner ce problème, on utilise donc un script intermédiaire :

  #FIXME .\jslint.cmd --maxerr 100 $inFile
}

function LintJSViaUglifyJS {
  [CmdletBinding()]
  param([Parameter(Mandatory = $true, Position = 0)] [string] $inFile)

  $uglifyjs = GetModulePath -RelativePath 'uglifyjs.cmd'

  & $uglifyjs $inFile -b --lint | Out-Null
}

function MinifyCssViaCleanCss {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $inFile,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  $cleancss = GetModulePath -RelativePath 'cleancss.cmd'

  & $cleancss -d -o $outFile $inFile
}

function MinifyJSViaUglifyJS {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0)] [string] $inFile,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  $uglifyjs = GetModulePath -RelativePath 'uglifyjs.cmd'

  # TODO: source-map
  & $uglifyjs $inFile --compress --mangle --output $outFile
}

#-- Directives --#

Export-ModuleMember `
  -function LintCss, LintJS, MinifyCss, MinifyJS `
  -variable assets
