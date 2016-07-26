@setlocal enableextensions enabledelayedexpansion
@echo off
set ipaddr="advan.thermaltech.com"
set state=down
for /f "tokens=5,7" %%a in ('ping -n 1 !ipaddr!') do (
    if "x%%a"=="xReceived" if "x%%b"=="x1," set state=up
)
echo.!state!> isup.txt
ping -n 1 127.0.0.1 >nul: 2>nul:
endlocal

