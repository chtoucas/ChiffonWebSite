@echo off
@cls

:init
@call "%VS120COMNTOOLS%vsvars32.bat"

:build
@echo Building...
MSBuild "%~dp0\..\src\Chiffon.WebSite\Chiffon.WebSite.csproj" /nologo /v:normal /p:Configuration=Debug;MvcBuildViews=true /t:MvcBuildViews /flp:logfile=.\msbuild.log;verbosity=normal;encoding=utf-8

@if %errorlevel% NEQ 0 (
  @goto error
)

:end
@echo Build successful
@pause
@goto :eof

:error
@echo *** An error occured ***
@pause
@exit /B %errorlevel%