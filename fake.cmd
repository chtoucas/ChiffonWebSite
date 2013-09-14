@echo off

:fake
"packages\FAKE.1.74.256.0\tools\Fake.exe" build.fsx "%1"
@if errorlevel 1 (
    @goto error
)

:end
@popd
@goto :eof

:error
@popd
@exit /B %errorlevel%