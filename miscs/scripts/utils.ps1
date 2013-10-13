# First version:
# - start/stop IIS AppPools
#   Cf. http://technet.microsoft.com/en-us/library/ee790553.aspx
#   Cf. http://www.iis.net/learn/manage/powershell/powershell-snap-in-using-the-task-based-cmdlets-of-the-iis-powershell-snap-in
# - use transactions? sync?
# Second version: use Web Deploy
#   Use WDeploySnapin3.0
#   Cf. http://msdn.microsoft.com/en-us/library/dd394698.aspx
#   Cf. http://www.troyhunt.com/2010/11/you-deploying-it-wrong-teamcity_26.html

function Get-WebDeployInstallPath {
     return (Get-ChildItem "HKLM:\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" | Select -last 1).GetValue("InstallPath")
}

#if ((Get-PSSnapin -Name WebAdministration -ErrorAction SilentlyContinue) -eq $null ) {
#    Add-PSSnapin WebAdministration -ErrorAction Stop
#}
# Verbatim copy of :
#   http://stackoverflow.com/questions/10700660/add-pssnapin-webadministration-in-windows7
# Web administration is loaded as a module on Windows 2008 R2 but as a set of snapins
# for Windows 2008 (not R2)
function Import-WebAdministration {
  $ModuleName = "WebAdministration"
  $ModuleLoaded = $false
  $LoadAsSnapin = $false

  if ($PSVersionTable.PSVersion.Major -ge 2) {
    if ((Get-Module -ListAvailable | ForEach-Object {$_.Name}) -contains $ModuleName) {
      Import-Module $ModuleName

      if ((Get-Module | ForEach-Object {$_.Name}) -contains $ModuleName) {
        $ModuleLoaded = $true
      } else {
        $LoadAsSnapin = $true
      }
    } elseif ((Get-Module | ForEach-Object {$_.Name}) -contains $ModuleName) {
      $ModuleLoaded = $true
    } else {
      $LoadAsSnapin = $true
    }
  } else {
    $LoadAsSnapin = $true
  }

  if ($LoadAsSnapin) {
    try {
      if ((Get-PSSnapin -Registered | ForEach-Object {$_.Name}) -contains $ModuleName) {
        if ((Get-PSSnapin -Name $ModuleName -ErrorAction SilentlyContinue) -eq $null) {
          Add-PSSnapin $ModuleName
        }

        if ((Get-PSSnapin | ForEach-Object {$_.Name}) -contains $ModuleName) {
          $ModuleLoaded = $true
        }
      } elseif ((Get-PSSnapin | ForEach-Object {$_.Name}) -contains $ModuleName) {
        $ModuleLoaded = $true
      }
    } catch {
      Write-Error "`t`t$($MyInvocation.InvocationName): $_"
      Exit
    }
  }
}
