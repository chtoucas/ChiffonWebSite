@echo off

@call "%VS110COMNTOOLS%vsvars32.bat"

MSBuild "%~dp0\src\Chiffon.WebSite\assets\Chiffon.Assets.proj" /nologo /v:normal /t:Build /fl
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%
