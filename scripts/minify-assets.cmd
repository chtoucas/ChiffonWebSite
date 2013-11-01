@echo off

@call "%VS110COMNTOOLS%vsvars32.bat"

MSBuild "%~dp0\..\src\Chiffon.WebSite\assets\Chiffon.Assets.proj" /nologo /v:normal /t:Build /flp:logfile=.\msbuild.log;verbosity=normal;encoding=utf-8
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%
