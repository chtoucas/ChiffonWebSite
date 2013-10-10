@echo off

:initenv
@call "%VS110COMNTOOLS%vsvars32.bat"

:build
MSBuild .\src\Chiffon.WebSite\Chiffon.WebSite.csproj /nologo /v:normal /p:Configuration=Debug;MvcBuildViews=true /t:MvcBuildViews /fl
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%