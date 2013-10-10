@echo off

:init
@call "%VS110COMNTOOLS%vsvars32.bat"

:build
MSBuild "%~dp0\src\Chiffon.WebSite\assets\Chiffon.Assets.proj" /nologo /v:normal /t:Build /fl
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%
