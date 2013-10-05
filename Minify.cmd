@echo off

:initenv
@call "%VS110COMNTOOLS%vsvars32.bat"

:build
MSBuild .\libexec\Chiffon.proj /nologo /verbosity:minimal /p:BuildAssets=true;BuildSolution=false /t:Build /fl
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%
