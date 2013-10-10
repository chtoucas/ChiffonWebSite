@echo off

if '%1'=='/?' goto help
if '%1'=='-help' goto help
if '%1'=='-h' goto help

PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\build.ps1' %*; if ($psake.build_success -eq $false) { exit 1 } else { exit 0 }"
goto :eof

:help
PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\build.ps1' Help"
