@echo off

@call "%VS120COMNTOOLS%vsvars32.bat"

MSBuild "%~dp0\..\src\Chiffon.WebSite\Chiffon.WebSite.csproj" /nologo /v:normal /p:Configuration=Debug;MvcBuildViews=true /t:MvcBuildViews /flp:logfile=.\msbuild.log;verbosity=normal;encoding=utf-8
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%