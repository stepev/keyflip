# Build KeyFlip with the C# compiler shipped with Windows (.NET Framework 4.x) - no SDK needed
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$csc  = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path $csc)) { $csc = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe' }
if (-not (Test-Path $csc)) { throw "csc.exe (.NET Framework 4.x) not found" }

$bin = Join-Path $root 'bin'
if (-not (Test-Path $bin)) { New-Item -ItemType Directory -Path $bin | Out-Null }
$out = Join-Path $bin 'KeyFlip.exe'

$icon = Join-Path $root 'assets\app.ico'
if (-not (Test-Path $icon)) { & (Join-Path $root 'tools\make-icon.ps1') }

$src = @(
  (Join-Path $root 'src\Program.cs'),
  (Join-Path $root 'src\Flags.cs'),
  (Join-Path $root 'src\Strings.cs'),
  (Join-Path $root 'src\About.cs'),
  (Join-Path $root 'src\Translit.cs'),
  (Join-Path $root 'src\TypedJournal.cs'),
  (Join-Path $root 'src\Hotkey.cs'),
  (Join-Path $root 'src\Installer.cs')
)

& $csc /nologo /codepage:65001 /target:winexe /platform:anycpu /optimize+ /warn:3 `
  /win32icon:$icon `
  /out:$out `
  /r:System.dll /r:System.Core.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll `
  $src

if ($LASTEXITCODE -ne 0) { throw "Build failed ($LASTEXITCODE)" }
Write-Host "Built: $out"
