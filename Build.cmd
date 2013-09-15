@echo off

:initenv
@call "%VS110COMNTOOLS%vsvars32.bat"

:build
MSBuild Assets.msbuild /nologo /verbosity:normal
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%
