#Requires -Version 3.0

Set-StrictMode -Version Latest

# TODO: Google Closure Tools (utiliser IronPython via NuGet ?)

function Compress-Css {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  Invoke-CleanCss $source -OutFile $outFile
}

function Compress-JavaScript {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $outFile
  )

  Invoke-UglifyJS $source -OutFile $outFile
}

function LintCss {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $name
  )

  Invoke-CssLint $source -Name $name
}

function LintJavaScript {
  [CmdletBinding()]
  param(
    [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)] [string] $source,
    [Parameter(Mandatory = $true, Position = 1)] [string] $name
  )

  #LintJSViaUglifyJS -InFile $source
  Invoke-JSHint $source -Name $name
  Invoke-JSLint $source
}
