
function New-Directory {
  param(
    [Parameter(Mandatory = $true)] [string] $path
  )

  if (!(Test-Path $path)) {
    New-Item $path -Type directory | Out-Null
  }

  return $path
}

function MergeFiles {
  param(
    [Parameter(Mandatory = $true)] [array] $inFiles,
    [Parameter(Mandatory = $true)] [string] $outFile
  )

  # Si le fichier existe déjà, on le supprime.
  if (Test-Path $outFile) { Remove-Item $outFile }

  $encoding = New-Object System.Text.UTF8Encoding($false)

  foreach ($filePath in $inFiles) {
    $content = [System.IO.File]::ReadAllLines($filePath)
    [System.IO.File]::AppendAllLines($outFile, $content, $encoding)
  }
}

Export-ModuleMember -function MergeFiles, New-Directory
