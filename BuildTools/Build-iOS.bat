@echo off
setlocal
powershell -ExecutionPolicy Bypass -File "%~dp0Build-iOS.ps1"
pause
