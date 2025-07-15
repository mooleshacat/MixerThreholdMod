@echo off
title Git Commit Signing Tool
echo Starting PowerShell script...
echo.

cd C:\Users\DESKTOP\Desktop\Modding\MixerThreholdMod-1_0_0

REM Run PowerShell with explicit parameters to keep window open
powershell.exe -NoExit -ExecutionPolicy Bypass -File "Sign-AllCommits.ps1"

REM If PowerShell exits, pause anyway
echo.
echo PowerShell script ended.
pause