@echo off

"%~dp0\tools\tsc.cmd" -d --sourcemap -t ES3 --sourceRoot "./" "%~dp0\src\Chiffon.WebSite\assets\js\jquery.plugins.ts"

