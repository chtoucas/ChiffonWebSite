#Requires -Version 3.0

Set-StrictMode -Version Latest

$script:Chiffon = @{}

$Chiffon.ProjectDirectory = $null

if ($args.Length -gt 0) {
  if ($args[0] -is [hashtable]) {
    $Chiffon.ProjectDirectory = $args[0].ProjectDirectory
  } else {
    Write-Error "FIXME: Option not found"
  }
} else {
  Write-Error "FIXME: No arg given"
}

$Chiffon.ToolsDirectory = "$($Chiffon.ProjectDirectory)\tools"

Export-ModuleMember -Alias * -Function * -Cmdlet * -Variable Chiffon