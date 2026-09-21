# KeyFlip

***English** · [Русский](README.ru.md)*

A tiny Windows tray utility that switches the keyboard layout with **a single Ctrl press**
and fixes text **typed in the wrong layout** with a hotkey. Shows the current language as a
flag in the tray. One ~100 KB executable, no dependencies.

## Features

### 1. A single Ctrl press switches the layout

A low-level keyboard hook watches Ctrl press and release. The switch only fires when
**nothing else happened in between** — no other keys, no mouse clicks, no other modifiers.
So `Ctrl+C`, `Ctrl+Shift+Esc` and `Ctrl`+click keep working as usual. Holding Ctrl longer
than `maxhold` (400 ms by default) does not switch either. Synthetic keystrokes are ignored,
so KeyFlip does not fight with other layout switchers.

### 2. Convert typed text — `Ctrl+Shift+Backspace` by default

`ghbdtn` → `привет`, `Ghbdtn? vbh!` → `Привет, мир!`, and back.

* If text is **selected**, the selection is converted.
* Otherwise the **last typed word** is converted.
* The direction is chosen by the dominant alphabet; characters map by physical key
  (`,` on the English layout is `б` on the Russian one).
* After converting, the layout switches to match the result (`switchafterconvert`).

Change the combination in the tray menu → "Text conversion" → "Hotkey...": a window opens,
just press what you want. Anything works — `Alt+F9`, `F12`, `Win+Q`, `Pause`, a bare function
key. It is registered through `RegisterHotKey`, so it never reaches the focused application
(in particular `Backspace` deletes nothing). If the combination is already taken, KeyFlip
says so instead of failing silently.

**How it works.** For the last word the clipboard is not used at all: KeyFlip keeps its own
journal of typed keys (low-level hook + `ToUnicodeEx` with the focused window's layout), so it
knows the word and replaces it directly — `Backspace` as many times as needed, then the
converted text via `SendInput`. Instant, and the clipboard history stays clean. The journal is
dropped by anything that moves the caret or changes text outside the keyboard: arrows, Enter,
Tab, Esc, Home/End, a mouse click, switching windows, any Ctrl/Alt/Win combination other than
the conversion hotkey itself.

For a selection the clipboard is unavoidable — there is no other way to read selected text in
a foreign application. KeyFlip remembers the current clipboard text, sends `Ctrl+C`, waits for
`GetClipboardSequenceNumber` to change (up to 250 ms), converts the text and **types it
directly** over the selection, then restores the previous clipboard content. Its own temporary
entries are marked with the `CanIncludeInClipboardHistory` / `CanUploadToCloudClipboard`
formats, so they never show up in `Win+V` or the cloud clipboard.

### 3. Tray icon

* Flag of the current input language of the focused window, refreshed ~3 times per second.
* A greyed-out flag with a strike means switching is off.
* Double click toggles it, right click opens the menu.
* Flags are drawn in code for about 60 countries; when there is no flag for a language,
  the language code (`EN`, `RU`) is shown — that mode can also be chosen manually.

### 4. Interface language

Menu → "Language": "Same as Windows" (default), "Русский", "English". Applied immediately,
no restart. "About" shows the version, a short key reference and a link to the sources.

## Install

Run **`install.cmd`** (double click, or `install.cmd /s` for a silent install). No admin
rights needed — everything goes into the current user's profile.

The script builds the app if needed and then:

* copies the exe to `%LOCALAPPDATA%\Programs\KeyFlip\`;
* creates a Start menu shortcut;
* registers the app in **Settings → Apps → Installed apps** (with a working Uninstall button);
* enables start with Windows;
* launches the installed copy.

The script itself is deliberately ASCII-only and prints nothing localized: the console code
page on Windows is unpredictable, so all human-readable messages come from the app as a
regular window.

### Uninstall

**Settings → Apps → Installed apps → KeyFlip → Uninstall**, or run `uninstall.cmd`
(`/s` to skip the questions). Removes the install folder, the shortcut, the autostart entry
and the registry record; it asks separately about the settings.

### Command line

| Argument            | Action                                            |
|---------------------|---------------------------------------------------|
| `--install`         | install (add `--silent` for no windows)           |
| `--uninstall`       | uninstall (`--silent` — no questions)             |
| `--autostart:on`    | enable start with Windows and exit                |
| `--autostart:off`   | disable start with Windows and exit               |

## Build

Windows is all you need — the C# compiler ships with the OS (.NET Framework 4.x),
no .NET SDK required.

```bash
powershell -ExecutionPolicy Bypass -File build.ps1
```

Result: `bin\KeyFlip.exe`, a single ~100 KB file. The application icon is generated by
`tools\make-icon.ps1` into `assets\app.ico` (the build calls it when the file is missing).

## Settings

`%LOCALAPPDATA%\KeyFlip\config.ini` (menu → "Open settings file"). Menu changes are written
there too; manual edits are picked up on the next start.

```ini
enabled=1                      # 1/0 — is Ctrl switching active
method=post                    # post | winspace | altshift | ctrlshift
trigger=both                   # both | left | right — which Ctrl triggers the switch
maxhold=400                    # max Ctrl hold time in ms (0 = no limit)
icon=flag                      # flag | code
autostart=1
hotkey=Ctrl+Shift+Backspace    # text conversion; empty = disabled
switchafterconvert=1           # switch layout to match the converted text
language=auto                  # auto | ru | en
log=0                          # 1 — write log.txt next to the settings (diagnostics)
```

### Layout switching methods

| Value       | What it does                                                             |
|-------------|--------------------------------------------------------------------------|
| `post`      | (default) `WM_INPUTLANGCHANGEREQUEST` to the focused window — instant     |
| `winspace`  | emulates `Win + Space` — works wherever the built-in switch works         |
| `altshift`  | emulates `Alt + Shift`                                                    |
| `ctrlshift` | emulates `Ctrl + Shift`                                                   |

If `post` does nothing in some app (usually Store/UWP), switch to `Win + Space`.

## Limitations

* Windows running **as administrator** do not deliver events to a normal hook, so KeyFlip
  does nothing inside them. Running the per-user copy elevated is **not** a fix and is unsafe:
  `%LOCALAPPDATA%\Programs\KeyFlip` is writable by any non-elevated process of the same user,
  so anything running as you could replace the exe and have it launched with administrator
  rights. If you really need it in elevated windows, put the exe somewhere ordinary user code
  cannot write to (for example `%ProgramFiles%\KeyFlip`, which requires admin rights to create)
  and start that copy elevated.
* The hook is off on the lock screen, in UAC prompts and on the secure desktop — a Windows
  restriction.
* Converting a selection uses `Ctrl+C`, so it fails where copying is unavailable. The previous
  clipboard **text** is restored, but non-text content (an image, files) is lost — the copy
  itself overwrites it.
* Converting the last word relies on the typed journal: if the word was pasted, typed before
  KeyFlip started, or a mouse click happened in front of it, there is nothing to convert —
  select the text instead.
* At least two keyboard layouts must be installed.

## Project layout

```
src/Program.cs      — hooks, single Ctrl, layout switching, hotkey, conversion, tray, config, log
src/Strings.cs      — all interface texts, Russian and English
src/TypedJournal.cs — journal of typed keys (what counts as a word, when to drop it)
src/Translit.cs     — ЙЦУКЕН ↔ QWERTY table, last-word parsing
src/Hotkey.cs       — hotkey parsing and the "press a combination" dialog
src/About.cs        — About window
src/Installer.cs    — install/uninstall, shortcut, Installed apps entry
src/Flags.cs        — flag drawing for the tray icon
tools/make-icon.ps1 — generates assets/app.ico
build.ps1           — build
install.cmd / uninstall.cmd — install and uninstall
```

## License

MIT — see [LICENSE](LICENSE). © 2026 KeyFlip contributors.
