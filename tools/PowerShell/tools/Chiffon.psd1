@{
  ModuleVersion = '1.0'
  GUID = '98c97367-ce95-4400-8314-1e5d2d7b5ed0'
  Author = 'chtoucas'
  CompanyName = 'Narvalo.Org'
  Copyright = '(c) 2013 chtoucas. Tous droits réservés.'
  Description = 'Chiffon PowerShell modules.'
  PowerShellVersion = '3.0'
  DotNetFrameworkVersion = '4.5'
  CLRVersion = '4.0'
  RequiredModules = @('Narvalo')
  RootModule = 'Chiffon.psm1'
  NestedModules = @(
    'Chiffon.Tools.psm1',
    'Chiffon.Tools.Resources.psm1'
  )
}
