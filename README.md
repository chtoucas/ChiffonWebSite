ChiffonWebSite
==============

A multilingual WebSite for a textile design collective.
Available in French & English.

The result (with SEO disabled) can be seen here:
[http://narvalo.org/chiffon/](http://narvalo.org/chiffon/).

**Status: Stable**

### Technology footprint ###

- ASP.NET MVC 5
- Autofac
- MEF
- jQuery
- PowerShell
- MSBuild
- PSake
- Grunt

### Requirements ###

- Visual Studio Express 2013 Update 3
- .NET 4.5.1
- PowerShell v3
- Nodejs

### Setup ###

You will need to clone my PowerShell repository
[Narvalo.PowerShell](https://github.com/chtoucas/Narvalo.PowerShell)
and add it to your PowerShell module path.

Add the `scripts\modules` directory to your PowerShell module path, then launch
`RUN_ME_FIRST.ps1` in a PowerShell session. This script will download then
install the nessary third-party tools, namely:

- 7-Zip
- NuGet
- Node packages
