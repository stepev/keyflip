@echo off
rem KeyFlip installer. Messages are ASCII on purpose: the console code page
rem is not predictable, so all human-readable text is shown by the app itself.
rem Usage: install.cmd [/s]   - /s installs silently, without the confirmation window.
setlocal
set "EXE=%~dp0bin\KeyFlip.exe"
set "MODE="
if /i "%~1"=="/s" set "MODE= --silent"

if not exist "%EXE%" (
  echo Building KeyFlip...
  powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build.ps1"
)

if not exist "%EXE%" (
  echo.
  echo Build failed: "%EXE%" not found.
  echo.
  pause
  exit /b 1
)

if defined MODE (
  "%EXE%" --install%MODE%
) else (
  start "" "%EXE%" --install
)
exit /b 0
