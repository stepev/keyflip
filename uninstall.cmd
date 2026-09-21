@echo off
rem KeyFlip uninstaller. Usage: uninstall.cmd [/s]
setlocal
set "TARGET=%LOCALAPPDATA%\Programs\KeyFlip\KeyFlip.exe"

if not exist "%TARGET%" (
  echo KeyFlip is not installed.
  echo.
  pause
  exit /b 0
)

if /i "%~1"=="/s" (
  "%TARGET%" --uninstall --silent
) else (
  start "" "%TARGET%" --uninstall
)
exit /b 0
