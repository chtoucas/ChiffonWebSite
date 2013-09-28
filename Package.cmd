@echo off

:initenv
@call "%VS110COMNTOOLS%vsvars32.bat"

:build
MSBuild Chiffon.proj /nologo /verbosity:quiet /p:VisualStudioVersion=11.0 /p:PlatformPackage=Production /t:Package /fl
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%
