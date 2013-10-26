# Pré-requis : nodejs
# Modules nodejs utilisés : clean-css, csslint, jshint, jslint, uglify-js
# TODO: Google Closure Tools (utiliser IronPython via NuGet ?)

function GetModulePath {
  param([Parameter(Mandatory = $true)] [System.Uri] $relativePath)

  "$($nodejs.binPath)\$($relativePath)"
}

function LintCss {
  param(
    [Parameter(Mandatory = $true)] [string] $inFile,
    [Parameter(Mandatory = $true)] [string] $name
  )

  Write-Output "-> Processing $name.css"

  LintCssViaCssLint -InFile $inFile -Name $name
}

function LintJS {
  param(
    [Parameter(Mandatory = $true)] [string] $inFile,
    [Parameter(Mandatory = $true)] [string] $name
  )

  Write-Output "-> Processing $name.js"

  #LintJSViaUglifyJS -InFile $inFile
  LintJSViaJSHint -InFile $inFile -Name $name
  LintJSViaJSLint -InFile $inFile
}

function MinifyCss {
  param(
    [Parameter(Mandatory = $true)] [string] $inFile,
    [Parameter(Mandatory = $true)] [string] $outFile,
    [Parameter(Mandatory = $true)] [string] $name
  )

  Write-Output "-> Processing $name.css"

  MinifyCssViaCleanCss -InFile $inFile -OutFile $outFile
}

function MinifyJS {
  param(
    [Parameter(Mandatory = $true)] [string] $inFile,
    [Parameter(Mandatory = $true)] [string] $outFile,
    [Parameter(Mandatory = $true)] [string] $name
  )

  Write-Output "-> Processing $name.js"

  MinifyJSViaUglifyJS -InFile $inFile -OutFile $outFile
}


### Fonction privées.

function LintCssViaCssLint {
  param(
    [Parameter(Mandatory = $true)] [string] $inFile,
    [Parameter(Mandatory = $true)] [string] $name
  )

  $logfile = "$ReportsDir\csslint-$name.log"
  if (Test-Path $logfile) { Remove-Item $logfile }

  $csslint = GetModulePath -RelativePath 'csslint.cmd'

  & $csslint --quiet $inFile | Out-File $logfile
}

function LintJSViaJSHint {
  param(
    [Parameter(Mandatory = $true)] [string] $inFile,
    [Parameter(Mandatory = $true)] [string] $name
  )

  $logfile = "$ReportsDir\jshint-$name.log"
  if (Test-Path $logfile) { Remove-Item $logfile }

  $jshint = GetModulePath -RelativePath 'jshint.cmd'

  & $jshint $inFile | Out-File $logfile -Append
}

function LintJSViaJSLint {
  param([Parameter(Mandatory = $true)] [string] $inFile)

  # NB: Pour une explication des erreurs jslint, cf. http://jslinterrors.com/

  # WARNING: node-jslint utilise console.log et lorsqu'on redirige la sortie dans une fenêtre
  # PowerShell, le fichier cible est vide :-( Apparemment je ne suis pas le seul à rencontrer
  # ce problème :
  #   http://stackoverflow.com/questions/9846326/node-console-log-behavior-and-windows-stdout
  # Pour contourner ce problème, on utilise donc un script intermédiaire :

  #FIXME .\jslint.cmd --maxerr 100 $inFile
}

function LintJSViaUglifyJS {
  param([Parameter(Mandatory = $true)] [string] $inFile)

  $uglifyjs = GetModulePath -RelativePath 'uglifyjs.cmd'

  & $uglifyjs $inFile -b --lint | Out-Null
}

function MinifyCssViaCleanCss {
  param(
    [Parameter(Mandatory = $true)] [string] $inFile,
    [Parameter(Mandatory = $true)] [string] $outFile
  )

  $cleancss = GetModulePath -RelativePath 'cleancss.cmd'

  & $cleancss -d -o $outFile $inFile
}

function MinifyJSViaUglifyJS {
  param(
    [Parameter(Mandatory = $true)] [string] $inFile,
    [Parameter(Mandatory = $true)] [string] $outFile
  )

  $uglifyjs = GetModulePath -RelativePath 'uglifyjs.cmd'

  # TODO: source-map
  & $uglifyjs $inFile --compress --mangle --output $outFile
}

$script:nodejs = @{}
$nodejs.binPath = $null
$nodejs.reportsDir = $null

### Exports.

Export-ModuleMember `
  -function LintCss, LintJS, MinifyCss, MinifyJS `
  -variable assets
