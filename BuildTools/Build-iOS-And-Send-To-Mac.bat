@echo off
setlocal
powershell -ExecutionPolicy Bypass -File "%~dp0Build-iOS-And-Send-To-Mac.ps1"
pause
