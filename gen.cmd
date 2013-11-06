@echo off

::"%~dp0\tools\tsc.cmd" -d --sourcemap -t ES3 --sourceRoot "./" --outDir "%~dp0\_work" "%~dp0\src\Chiffon.WebSite\assets\js\jquery.plugins.ts"
"%~dp0\tools\tsc.cmd" -d --sourcemap -t ES3 --sourceRoot "./" --outDir "%~dp0\_work" "%~dp0\src\Chiffon.WebSite\assets\js\app.ts"
:: "%~dp0\tools\tsc.cmd" -d --sourcemap -t ES3 --sourceRoot "./" --outDir "%~dp0\_work" "%~dp0\src\Chiffon.WebSite\assets\js\chiffon.ts"
