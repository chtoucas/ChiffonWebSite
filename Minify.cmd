@echo off

:initenv
@call "%VS110COMNTOOLS%vsvars32.bat"

:build
MSBuild Chiffon.proj /nologo /verbosity:normal /p:MinifyOnly=true /t:Build /fl
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%
