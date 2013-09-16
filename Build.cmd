@echo off

:initenv
@call "%VS110COMNTOOLS%vsvars32.bat"

:build
MSBuild Chiffon.msbuild /nologo /verbosity:normal /t:Build /p:Configuration=Release;Platform=AnyCPU;TargetPlatform=Development
@if errorlevel 1 (
  @goto error
)

:end
@goto :eof

:error
@exit /B %errorlevel%
