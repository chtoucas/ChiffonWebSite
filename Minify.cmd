@echo off

:initenv
@call "%VS110COMNTOOLS%vsvars32.bat"

:build
MSBuild .\src\Chiffon.WebSite\assets\Chiffon.Assets.proj /nologo /verbosity:minimal /t:Build /fl
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%
