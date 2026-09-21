using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using WinTimer = System.Windows.Forms.Timer;

namespace KeyFlip
{
    internal static class Native
    {
        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE_LL = 14;

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;
        public const int WM_HOTKEY = 0x0312;

        public const uint LLKHF_INJECTED = 0x10;

        public const int VK_LCONTROL = 0xA2;
        public const int VK_RCONTROL = 0xA3;
        public const int VK_CONTROL = 0x11;
        public const int VK_SHIFT = 0x10;
        public const int VK_MENU = 0x12;
        public const int VK_LSHIFT = 0xA0;
        public const int VK_RSHIFT = 0xA1;
        public const int VK_LMENU = 0xA4;
        public const int VK_RMENU = 0xA5;
        public const int VK_LWIN = 0x5B;
        public const int VK_RWIN = 0x5C;
        public const int VK_SPACE = 0x20;
        public const int VK_BACK = 0x08;
        public const int VK_TAB = 0x09;
        public const int VK_RETURN = 0x0D;
        public const int VK_CAPITAL = 0x14;
        public const int VK_ESCAPE = 0x1B;
        public const int VK_PRIOR = 0x21;
        public const int VK_NEXT = 0x22;
        public const int VK_END = 0x23;
        public const int VK_HOME = 0x24;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int VK_DOWN = 0x28;
        public const int VK_INSERT = 0x2D;
        public const int VK_DELETE = 0x2E;
        public const int VK_C = 0x43;
        public const int VK_V = 0x56;

        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_XBUTTONDOWN = 0x020B;

        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_UNICODE = 0x0004;
        public const uint INPUT_KEYBOARD = 1;

        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSLLHOOKSTRUCT
        {
            public int x;
            public int y;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public InputUnion u;
        }

        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        public static extern uint GetKeyboardLayoutList(int nBuff, [Out] IntPtr[] lpList);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        public static extern uint GetClipboardSequenceNumber();

        public static bool IsDown(int vk)
        {
            return (GetAsyncKeyState(vk) & 0x8000) != 0;
        }
    }

    /// <summary>Диагностический лог (включается log=1 в настройках).</summary>
    internal static class Log
    {
        public static bool Enabled;
        private static readonly object Sync = new object();

        public static string FilePath { get { return Path.Combine(Config.Dir, "log.txt"); } }

        public static void Write(string message)
        {
            if (!Enabled) return;
            try
            {
                lock (Sync)
                {
                    Directory.CreateDirectory(Config.Dir);
                    File.AppendAllText(FilePath,
                        DateTime.Now.ToString("HH:mm:ss.fff") + "  " + message + Environment.NewLine,
                        Encoding.UTF8);
                }
            }
            catch { }
        }
    }

    /// <summary>Настройки приложения (простой ini-файл в %LOCALAPPDATA%\KeyFlip).</summary>
    internal class Config
    {
        public bool Enabled = true;
        public string Method = "post";      // post | winspace | altshift | ctrlshift
        public string Trigger = "both";     // both | left | right
        public int MaxHoldMs = 400;         // 0 = без ограничения
        public string IconStyle = "flag";   // flag | code
        public bool Autostart = true;
        public string Hotkey = "Ctrl+Shift+Backspace"; // конвертация текста; пусто = выключено
        public bool SwitchAfterConvert = true;         // менять раскладку под результат конвертации
        public bool LogEnabled = false;                // писать log.txt рядом с настройками
        public string Language = "auto";               // auto | ru | en

        public static string Dir
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KeyFlip");
            }
        }

        public static string FilePath { get { return Path.Combine(Dir, "config.ini"); } }

        public static bool Exists { get { return File.Exists(FilePath); } }

        public static Config Load()
        {
            Config c = new Config();
            try
            {
                if (!File.Exists(FilePath)) return c;
                foreach (string raw in File.ReadAllLines(FilePath))
                {
                    string line = raw.Trim();
                    if (line.Length == 0 || line.StartsWith("#") || line.StartsWith(";")) continue;
                    int eq = line.IndexOf('=');
                    if (eq <= 0) continue;
                    string key = line.Substring(0, eq).Trim().ToLowerInvariant();
                    string val = line.Substring(eq + 1).Trim();
                    switch (key)
                    {
                        case "enabled": c.Enabled = val == "1" || val.ToLowerInvariant() == "true"; break;
                        case "method": c.Method = val.ToLowerInvariant(); break;
                        case "trigger": c.Trigger = val.ToLowerInvariant(); break;
                        case "maxhold": int.TryParse(val, out c.MaxHoldMs); break;
                        case "icon": c.IconStyle = val.ToLowerInvariant(); break;
                        case "autostart": c.Autostart = val == "1" || val.ToLowerInvariant() == "true"; break;
                        case "hotkey": c.Hotkey = val; break;
                        case "switchafterconvert": c.SwitchAfterConvert = val == "1" || val.ToLowerInvariant() == "true"; break;
                        case "log": c.LogEnabled = val == "1" || val.ToLowerInvariant() == "true"; break;
                        case "language": c.Language = val.ToLowerInvariant(); break;
                    }
                }
            }
            catch { }
            return c;
        }

        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Dir);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("# KeyFlip — настройки");
                sb.AppendLine("# enabled: 1/0 — работает ли переключение");
                sb.AppendLine("enabled=" + (Enabled ? "1" : "0"));
                sb.AppendLine("# method: post | winspace | altshift | ctrlshift");
                sb.AppendLine("method=" + Method);
                sb.AppendLine("# trigger: both | left | right");
                sb.AppendLine("trigger=" + Trigger);
                sb.AppendLine("# maxhold: максимальное время удержания Ctrl в мс (0 = без ограничения)");
                sb.AppendLine("maxhold=" + MaxHoldMs.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("# icon: flag | code");
                sb.AppendLine("icon=" + IconStyle);
                sb.AppendLine("autostart=" + (Autostart ? "1" : "0"));
                sb.AppendLine("# hotkey: сочетание для конвертации раскладки текста, например");
                sb.AppendLine("#   Ctrl+Shift+Backspace, Alt+F9, F12, Win+Q; пусто — выключено");
                sb.AppendLine("hotkey=" + Hotkey);
                sb.AppendLine("# switchafterconvert: 1 — после конвертации переключить раскладку под результат");
                sb.AppendLine("switchafterconvert=" + (SwitchAfterConvert ? "1" : "0"));
                sb.AppendLine("# log: 1 — писать диагностический log.txt в эту же папку");
                sb.AppendLine("log=" + (LogEnabled ? "1" : "0"));
                sb.AppendLine("# language: auto | ru | en — язык интерфейса");
                sb.AppendLine("language=" + Language);
                File.WriteAllText(FilePath, sb.ToString(), Encoding.UTF8);
            }
            catch { }
        }
    }

    internal static class Autostart
    {
        private const string RunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string ValueName = "KeyFlip";

        public static string ExePath
        {
            get { return Installer.NormalizePath(Application.ExecutablePath); }
        }

        public static bool IsEnabled()
        {
            try
            {
                using (RegistryKey k = Registry.CurrentUser.OpenSubKey(RunKey))
                {
                    if (k == null) return false;
                    object v = k.GetValue(ValueName);
                    return v != null && !string.IsNullOrEmpty(v.ToString());
                }
            }
            catch { return false; }
        }

        public static bool PointsToCurrentExe()
        {
            try
            {
                using (RegistryKey k = Registry.CurrentUser.OpenSubKey(RunKey))
                {
                    if (k == null) return false;
                    object v = k.GetValue(ValueName);
                    if (v == null) return false;
                    string s = v.ToString().Trim().Trim('"');
                    return Installer.SamePath(s, ExePath);
                }
            }
            catch { return false; }
        }

        public static void Set(bool enabled)
        {
            try
            {
                using (RegistryKey k = Registry.CurrentUser.CreateSubKey(RunKey))
                {
                    if (k == null) return;
                    if (enabled) k.SetValue(ValueName, "\"" + ExePath + "\"");
                    else k.DeleteValue(ValueName, false);
                }
            }
            catch { }
        }
    }

    /// <summary>Невидимое окно: принимает WM_HOTKEY и служит для перехода в UI-поток.</summary>
    internal class MsgWindow : Form
    {
        public event EventHandler HotkeyPressed;

        public MsgWindow()
        {
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(-32000, -32000);
            Size = new Size(1, 1);
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false); // окно никогда не показывается
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Native.WM_HOTKEY && HotkeyPressed != null)
                HotkeyPressed(this, EventArgs.Empty);
            base.WndProc(ref m);
        }
    }

    /// <summary>
    /// Минимальная работа с буфером обмена: только текст, короткие таймауты и пометки,
    /// исключающие наши временные записи из истории буфера (Win+V) и облачного буфера.
    /// </summary>
    internal static class ClipboardHelper
    {
        private const string NoHistory = "CanIncludeInClipboardHistory";
        private const string NoCloud = "CanUploadToCloudClipboard";

        public static string GetText()
        {
            for (int i = 0; i < 4; i++)
            {
                try { return Clipboard.ContainsText() ? Clipboard.GetText() : null; }
                catch { Thread.Sleep(10); }
            }
            return null;
        }

        /// <summary>Кладёт текст в буфер, не засоряя историю буфера обмена.</summary>
        public static void SetTextPrivate(string text)
        {
            if (text == null) return;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    DataObject data = new DataObject();
                    data.SetText(text);
                    data.SetData(NoHistory, new MemoryStream(new byte[] { 0, 0, 0, 0 }));
                    data.SetData(NoCloud, new MemoryStream(new byte[] { 0, 0, 0, 0 }));
                    Clipboard.SetDataObject(data, true);
                    return;
                }
                catch { Thread.Sleep(10); }
            }
        }
    }

    internal class TrayApp : ApplicationContext
    {
        private readonly Config _cfg;
        private readonly NotifyIcon _tray;
        private ContextMenuStrip _menu;
        private readonly WinTimer _timer;
        private readonly MsgWindow _pump;
        private const int HotkeyId = 0xC401;
        private bool _hotkeyRegistered;
        private HotkeySpec _hotkeySpec;
        private int _converting;

        private IntPtr _kbHook = IntPtr.Zero;
        private IntPtr _msHook = IntPtr.Zero;
        private Native.HookProc _kbProc;
        private Native.HookProc _msProc;

        // Состояние отслеживания одиночного Ctrl
        private bool _ctrlDown;
        private int _ctrlVk;
        private bool _dirty;
        private int _ctrlDownTick;

        private readonly TypedJournal _typed = new TypedJournal();     // журнал набранного в текущем поле
        private IntPtr _lastForeground = IntPtr.Zero;
        private IntPtr _lastHkl = IntPtr.Zero;
        private string _lastIconKey = "";
        private Icon _currentIcon;

        public TrayApp()
        {
            _cfg = Config.Load();
            Strings.Apply(_cfg.Language);
            Log.Enabled = _cfg.LogEnabled;
            Log.Write("=== старт, exe=" + Installer.NormalizePath(Application.ExecutablePath) + " ===");
            bool firstRun = !Config.Exists;
            _cfg.Save(); // приводим файл к актуальной схеме: дописываем ключи, появившиеся в новых версиях
            if (firstRun)
            {
                Autostart.Set(_cfg.Autostart);
            }
            else if (_cfg.Autostart && !Autostart.PointsToCurrentExe())
            {
                // exe переместили — чиним запись автозапуска
                Autostart.Set(true);
            }
            else if (!_cfg.Autostart && Autostart.IsEnabled())
            {
                Autostart.Set(false);
            }

            _pump = new MsgWindow();
            _pump.HotkeyPressed += delegate { OnHotkey(); };
            IntPtr dummy = _pump.Handle; // создаём хендл для BeginInvoke и WM_HOTKEY
            GC.KeepAlive(dummy);
            ApplyHotkey(false);

            _menu = BuildMenu();

            _tray = new NotifyIcon();
            _tray.ContextMenuStrip = _menu;
            _tray.Visible = true;
            _tray.Text = "KeyFlip";
            _tray.DoubleClick += delegate { ToggleEnabled(); };
            UpdateIcon(true);

            _timer = new WinTimer();
            _timer.Interval = 300;
            _timer.Tick += delegate
            {
                // восстановление состояния, если KEYUP потерялся (UAC, Win+L, Alt+Tab)
                if (_ctrlDown && !Native.IsDown(Native.VK_CONTROL))
                {
                    _ctrlDown = false;
                    _dirty = false;
                }

                IntPtr fg = Native.GetForegroundWindow();
                if (fg != _lastForeground)
                {
                    _lastForeground = fg;
                    _typed.Clear(); // сменилось окно — журнал набранного больше не относится к делу
                }
                UpdateIcon(false);
            };
            _timer.Start();

            InstallHooks();

            if (firstRun)
            {
                ShowBalloon(Strings.StartedTitle, Strings.StartedText(_cfg.Hotkey));
            }
        }

        // ---------- хуки ----------

        private void InstallHooks()
        {
            _kbProc = KeyboardProc;
            _msProc = MouseProc;
            IntPtr hMod = Native.GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName);
            _kbHook = Native.SetWindowsHookEx(Native.WH_KEYBOARD_LL, _kbProc, hMod, 0);
            _msHook = Native.SetWindowsHookEx(Native.WH_MOUSE_LL, _msProc, hMod, 0);
            if (_kbHook == IntPtr.Zero)
            {
                MessageBox.Show(Strings.HookFailed(Marshal.GetLastWin32Error()),
                    "KeyFlip", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveHooks()
        {
            if (_kbHook != IntPtr.Zero) { Native.UnhookWindowsHookEx(_kbHook); _kbHook = IntPtr.Zero; }
            if (_msHook != IntPtr.Zero) { Native.UnhookWindowsHookEx(_msHook); _msHook = IntPtr.Zero; }
        }

        private IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                try
                {
                    Native.KBDLLHOOKSTRUCT d = (Native.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.KBDLLHOOKSTRUCT));
                    if ((d.flags & Native.LLKHF_INJECTED) == 0)
                        HandleKey(wParam.ToInt32(), (int)d.vkCode, d.scanCode);
                }
                catch { }
            }
            return Native.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private IntPtr MouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int msg = wParam.ToInt32();
                bool click = msg == Native.WM_LBUTTONDOWN || msg == Native.WM_RBUTTONDOWN
                          || msg == Native.WM_MBUTTONDOWN || msg == Native.WM_XBUTTONDOWN
                          || msg == Native.WM_MOUSEWHEEL;
                if (click)
                {
                    // клик/прокрутка при зажатом Ctrl — это комбинация, а не одиночное нажатие
                    if (_ctrlDown) _dirty = true;
                    // курсор мог уехать куда угодно — журнал набранного больше не актуален
                    _typed.Clear();
                }
            }
            return Native.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        private void HandleKey(int msg, int vk, uint scan)
        {
            bool down = msg == Native.WM_KEYDOWN || msg == Native.WM_SYSKEYDOWN;
            bool up = msg == Native.WM_KEYUP || msg == Native.WM_SYSKEYUP;
            bool isCtrl = vk == Native.VK_LCONTROL || vk == Native.VK_RCONTROL;

            if (down) TrackTyping(vk, scan);

            if (down)
            {
                if (isCtrl)
                {
                    if (!_ctrlDown)
                    {
                        _ctrlDown = true;
                        _ctrlVk = vk;
                        _ctrlDownTick = Environment.TickCount;
                        _dirty = !TriggerMatches(vk) || OtherModifierDown();
                    }
                    else if (vk != _ctrlVk)
                    {
                        _dirty = true; // нажаты оба Ctrl
                    }
                }
                else if (_ctrlDown)
                {
                    _dirty = true; // Ctrl используется как модификатор
                }
            }
            else if (up && isCtrl && _ctrlDown && vk == _ctrlVk)
            {
                _ctrlDown = false;
                bool held = _cfg.MaxHoldMs > 0 && (Environment.TickCount - _ctrlDownTick) > _cfg.MaxHoldMs;
                bool fire = _cfg.Enabled && !_dirty && !held && !OtherModifierDown();
                _dirty = false;
                if (fire)
                {
                    try { _pump.BeginInvoke(new Action(SwitchLanguage)); }
                    catch { }
                }
            }
        }

        // ---------- журнал набранного текста ----------

        /// <summary>
        /// Запоминает набираемые символы, чтобы уметь перекодировать последнее слово
        /// без обращения к буферу обмена. Журнал сбрасывается всем, что уводит курсор.
        /// </summary>
        private void TrackTyping(int vk, uint scan)
        {
            bool ctrlAltWin = Native.IsDown(Native.VK_CONTROL) || Native.IsDown(Native.VK_MENU)
                           || Native.IsDown(Native.VK_LWIN) || Native.IsDown(Native.VK_RWIN);

            char ch = '\0';
            if (!ctrlAltWin) TryTranslateKey(vk, scan, out ch);

            _typed.Track(vk, ch, ctrlAltWin, IsConvertHotkey(vk));
        }

        /// <summary>Нажата ли прямо сейчас та самая комбинация перекодировки.</summary>
        private bool IsConvertHotkey(int vk)
        {
            HotkeySpec spec = _hotkeySpec;
            if (spec == null || (int)spec.Key != vk) return false;

            bool ctrl = Native.IsDown(Native.VK_CONTROL);
            bool shift = Native.IsDown(Native.VK_SHIFT);
            bool alt = Native.IsDown(Native.VK_MENU);
            bool win = Native.IsDown(Native.VK_LWIN) || Native.IsDown(Native.VK_RWIN);

            return ctrl == ((spec.Mods & HotkeySpec.MOD_CONTROL) != 0)
                && shift == ((spec.Mods & HotkeySpec.MOD_SHIFT) != 0)
                && alt == ((spec.Mods & HotkeySpec.MOD_ALT) != 0)
                && win == ((spec.Mods & HotkeySpec.MOD_WIN) != 0);
        }

        /// <summary>Во что превращается клавиша в текущей раскладке активного окна.</summary>
        private static bool TryTranslateKey(int vk, uint scan, out char ch)
        {
            ch = '\0';
            try
            {
                byte[] state = new byte[256];
                if (Native.IsDown(Native.VK_SHIFT))
                {
                    state[Native.VK_SHIFT] = 0x80;
                    state[Native.VK_LSHIFT] = 0x80;
                }
                if ((Native.GetKeyState(Native.VK_CAPITAL) & 1) != 0) state[Native.VK_CAPITAL] = 1;

                StringBuilder sb = new StringBuilder(8);
                // флаг 4 — не менять состояние клавиатуры (важно, чтобы не ломать мёртвые клавиши)
                int n = Native.ToUnicodeEx((uint)vk, scan, state, sb, sb.Capacity, 4, CurrentHkl());
                if (n == 1)
                {
                    ch = sb[0];
                    return true;
                }
            }
            catch { }
            return false;
        }

        private bool TriggerMatches(int vk)
        {
            if (_cfg.Trigger == "left") return vk == Native.VK_LCONTROL;
            if (_cfg.Trigger == "right") return vk == Native.VK_RCONTROL;
            return true;
        }

        private static bool OtherModifierDown()
        {
            return Native.IsDown(Native.VK_SHIFT) || Native.IsDown(Native.VK_MENU)
                || Native.IsDown(Native.VK_LWIN) || Native.IsDown(Native.VK_RWIN);
        }

        // ---------- горячая клавиша: конвертация раскладки текста ----------

        private void ApplyHotkey(bool notifyOnSuccess)
        {
            if (_hotkeyRegistered)
            {
                Native.UnregisterHotKey(_pump.Handle, HotkeyId);
                _hotkeyRegistered = false;
                _hotkeySpec = null;
            }

            HotkeySpec spec = HotkeySpec.Parse(_cfg.Hotkey);
            _hotkeySpec = spec;
            if (spec == null) return;

            _hotkeyRegistered = Native.RegisterHotKey(_pump.Handle, HotkeyId,
                spec.Mods | HotkeySpec.MOD_NOREPEAT, (uint)spec.Key);

            Log.Write("хоткей " + spec + ": " + (_hotkeyRegistered ? "зарегистрирован" : "ОШИБКА " + Marshal.GetLastWin32Error()));

            if (!_hotkeyRegistered)
            {
                ShowBalloon(Strings.HotkeyBusyTitle, Strings.HotkeyBusyText(spec.ToString()));
            }
            else if (notifyOnSuccess)
            {
                ShowBalloon(Strings.HotkeyDoneTitle, Strings.HotkeyDoneText(spec.ToString()));
            }
        }

        private void OnHotkey()
        {
            Log.Write("WM_HOTKEY");
            if (Interlocked.CompareExchange(ref _converting, 1, 0) != 0)
            {
                Log.Write("пропуск: предыдущая конвертация ещё идёт");
                return;
            }
            Thread t = new Thread(ConvertWorker);
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA); // буфер обмена требует STA
            t.Start();
        }

        /// <summary>
        /// Перекодирует текст в другую раскладку: сначала последнее набранное слово из
        /// внутреннего журнала (Backspace + прямой ввод, мгновенно), иначе выделение
        /// (через Ctrl+C).
        /// </summary>
        private void ConvertWorker()
        {
            try
            {
                // пользователь ещё держит сочетание — иначе наши Backspace уедут как Ctrl+Backspace
                WaitModifiersReleased(250);
                ReleaseModifiers();

                // в лог пишем только длины: перекодировать могут и пароль, и переписку
                Log.Write("журнал набранного: " + _typed.Length + " симв.");

                // журнал не пуст — значит человек только что печатал: меняем слово сразу,
                // без обращения к буферу обмена; иначе пробуем выделение
                if (TryConvertLastWord()) return;
                if (TryConvertSelection()) return;

                Log.Write("нечего перекодировать");
                System.Media.SystemSounds.Beep.Play();
            }
            catch (Exception ex) { Log.Write("ОШИБКА: " + ex); }
            finally { Interlocked.Exchange(ref _converting, 0); }
        }

        /// <summary>Есть ли выделение — проверяем по тому, изменился ли буфер после Ctrl+C.</summary>
        private bool TryConvertSelection()
        {
            string previous = ClipboardHelper.GetText();
            uint seqBefore = Native.GetClipboardSequenceNumber();

            SendKey(Native.VK_C, Native.VK_CONTROL);

            string text = null;
            uint seqAfterCopy = seqBefore;
            for (int waited = 0; waited < 250; waited += 5)
            {
                Thread.Sleep(5);
                uint seq = Native.GetClipboardSequenceNumber();
                if (seq == seqBefore) continue;
                seqAfterCopy = seq;
                text = ClipboardHelper.GetText();
                break;
            }
            Log.Write("выделение: " + (text == null ? "<нет>" : text.Length + " симв."));

            // копирования не было — буфер мы не трогали, восстанавливать нечего
            if (text == null) return false;

            bool converted_differs = false;
            if (text.Trim().Length > 0)
            {
                bool resultIsCyrillic;
                string converted = Translit.Convert(text, out resultIsCyrillic);
                converted_differs = converted != text;
                if (converted_differs)
                {
                    // печатаем напрямую: ввод заменяет выделение, класть результат в буфер не нужно
                    SendText(converted);
                    _typed.Clear();
                    if (_cfg.SwitchAfterConvert) SetLayoutFor(resultIsCyrillic);
                }
            }

            RestoreClipboard(previous, seqAfterCopy);
            return converted_differs;
        }

        /// <summary>
        /// Возвращает прежний текст буфера, но только если с момента нашего Ctrl+C
        /// буфер никто не перезаписал — иначе мы затёрли бы более свежее содержимое.
        /// </summary>
        private static void RestoreClipboard(string previous, uint seqAfterCopy)
        {
            if (previous == null) return;
            if (Native.GetClipboardSequenceNumber() != seqAfterCopy)
            {
                Log.Write("буфер изменился извне — прежнее содержимое не восстанавливаем");
                return;
            }
            ClipboardHelper.SetTextPrivate(previous);
        }

        /// <summary>Последнее слово берём из журнала набранного — мгновенно и без буфера обмена.</summary>
        private bool TryConvertLastWord()
        {
            string word, trailing;
            if (!Translit.SplitLastWord(_typed.Text, out word, out trailing)) return false;

            bool resultIsCyrillic;
            string converted = Translit.Convert(word, out resultIsCyrillic);
            if (converted == word) return false;

            Log.Write("последнее слово: " + word.Length + " симв. -> "
                      + (resultIsCyrillic ? "кириллица" : "латиница"));

            SendBackspaces(word.Length + trailing.Length);
            SendText(converted + trailing);

            _typed.ReplaceTail(word.Length + trailing.Length, converted + trailing);

            if (_cfg.SwitchAfterConvert) SetLayoutFor(resultIsCyrillic);
            return true;
        }

        /// <summary>Печатает строку символ за символом (независимо от текущей раскладки).</summary>
        private static void SendText(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            Native.INPUT[] inputs = new Native.INPUT[text.Length * 2];
            for (int i = 0; i < text.Length; i++)
            {
                inputs[i * 2] = UnicodeKey(text[i], false);
                inputs[i * 2 + 1] = UnicodeKey(text[i], true);
            }
            Native.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Native.INPUT)));
        }

        private static void SendBackspaces(int count)
        {
            if (count <= 0) return;
            Native.INPUT[] inputs = new Native.INPUT[count * 2];
            for (int i = 0; i < count; i++)
            {
                inputs[i * 2] = Key(Native.VK_BACK, false);
                inputs[i * 2 + 1] = Key(Native.VK_BACK, true);
            }
            Native.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Native.INPUT)));
        }

        private static Native.INPUT UnicodeKey(char ch, bool up)
        {
            Native.INPUT i = new Native.INPUT();
            i.type = Native.INPUT_KEYBOARD;
            i.u.ki.wVk = 0;
            i.u.ki.wScan = ch;
            i.u.ki.dwFlags = Native.KEYEVENTF_UNICODE | (up ? Native.KEYEVENTF_KEYUP : 0);
            i.u.ki.time = 0;
            i.u.ki.dwExtraInfo = IntPtr.Zero;
            return i;
        }

        private static void WaitModifiersReleased(int timeoutMs)
        {
            int waited = 0;
            while (waited < timeoutMs && (Native.IsDown(Native.VK_CONTROL) || Native.IsDown(Native.VK_SHIFT)
                   || Native.IsDown(Native.VK_MENU) || Native.IsDown(Native.VK_LWIN) || Native.IsDown(Native.VK_RWIN)))
            {
                Thread.Sleep(15);
                waited += 15;
            }
        }

        /// <summary>Снимает «залипшие» модификаторы, если пользователь всё ещё держит сочетание.</summary>
        private static void ReleaseModifiers()
        {
            int[] keys = { Native.VK_LCONTROL, Native.VK_RCONTROL, Native.VK_LSHIFT, Native.VK_RSHIFT,
                           Native.VK_LMENU, Native.VK_RMENU, Native.VK_LWIN, Native.VK_RWIN };
            List<Native.INPUT> list = new List<Native.INPUT>();
            foreach (int vk in keys)
                if (Native.IsDown(vk)) list.Add(Key(vk, true));
            if (list.Count > 0)
                Native.SendInput((uint)list.Count, list.ToArray(), Marshal.SizeOf(typeof(Native.INPUT)));
        }

        /// <summary>Переключает раскладку активного окна на кириллическую или латинскую.</summary>
        private static void SetLayoutFor(bool cyrillic)
        {
            IntPtr[] list = GetLayouts();
            if (list.Length < 2) return;

            IntPtr target = IntPtr.Zero;
            foreach (IntPtr hkl in list)
            {
                if (IsCyrillicLayout(hkl) == cyrillic) { target = hkl; break; }
            }
            if (target == IntPtr.Zero) return;

            IntPtr fg = Native.GetForegroundWindow();
            if (fg == IntPtr.Zero) return;

            uint pid;
            uint tid = Native.GetWindowThreadProcessId(fg, out pid);
            if (Native.GetKeyboardLayout(tid) == target) return;

            IntPtr wnd = fg;
            uint myTid = Native.GetCurrentThreadId();
            if (tid != myTid && Native.AttachThreadInput(myTid, tid, true))
            {
                IntPtr focus = Native.GetFocus();
                if (focus != IntPtr.Zero) wnd = focus;
                Native.AttachThreadInput(myTid, tid, false);
            }
            Native.PostMessage(wnd, Native.WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, target);
            if (wnd != fg)
                Native.PostMessage(fg, Native.WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, target);
        }

        private static readonly string[] CyrillicLangs =
        {
            "ru", "uk", "be", "bg", "sr", "mk", "kk", "ky", "mn", "tg", "tt", "ba", "cv", "sah", "ab", "os"
        };

        private static bool IsCyrillicLayout(IntPtr hkl)
        {
            CultureInfo ci = CultureOf(hkl);
            if (ci == null) return false;
            string lang = ci.TwoLetterISOLanguageName;
            foreach (string s in CyrillicLangs)
                if (string.Equals(s, lang, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }

        // ---------- переключение раскладки ----------

        private void SwitchLanguage()
        {
            try
            {
                switch (_cfg.Method)
                {
                    case "winspace": SendCombo(Native.VK_LWIN, Native.VK_SPACE); break;
                    case "altshift": SendCombo(Native.VK_MENU, Native.VK_SHIFT); break;
                    case "ctrlshift": SendCombo(Native.VK_CONTROL, Native.VK_SHIFT); break;
                    default: PostLayoutChange(); break;
                }
            }
            catch { }
            UpdateIcon(false);
        }

        private void PostLayoutChange()
        {
            IntPtr fg = Native.GetForegroundWindow();
            if (fg == IntPtr.Zero) return;

            IntPtr[] list = GetLayouts();
            if (list.Length < 2)
            {
                ShowBalloon(Strings.OneLayoutTitle, Strings.OneLayoutText);
                return;
            }

            uint pid;
            uint tid = Native.GetWindowThreadProcessId(fg, out pid);
            IntPtr cur = Native.GetKeyboardLayout(tid);

            int idx = -1;
            for (int i = 0; i < list.Length; i++) if (list[i] == cur) { idx = i; break; }
            IntPtr next = (idx < 0) ? list[0] : list[(idx + 1) % list.Length];

            IntPtr target = fg;
            uint myTid = Native.GetCurrentThreadId();
            if (tid != myTid && Native.AttachThreadInput(myTid, tid, true))
            {
                IntPtr focus = Native.GetFocus();
                if (focus != IntPtr.Zero) target = focus;
                Native.AttachThreadInput(myTid, tid, false);
            }

            Native.PostMessage(target, Native.WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, next);
            if (target != fg)
                Native.PostMessage(fg, Native.WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, next);
        }

        private static IntPtr[] GetLayouts()
        {
            uint n = Native.GetKeyboardLayoutList(0, null);
            if (n == 0) return new IntPtr[0];
            IntPtr[] list = new IntPtr[n];
            Native.GetKeyboardLayoutList((int)n, list);
            return list;
        }

        private static void SendCombo(int modVk, int keyVk)
        {
            Native.INPUT[] inputs = new Native.INPUT[4];
            inputs[0] = Key(modVk, false);
            inputs[1] = Key(keyVk, false);
            inputs[2] = Key(keyVk, true);
            inputs[3] = Key(modVk, true);
            Native.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Native.INPUT)));
        }

        /// <summary>Нажимает клавишу с произвольным набором модификаторов.</summary>
        private static void SendKey(int vk, params int[] mods)
        {
            List<Native.INPUT> list = new List<Native.INPUT>();
            foreach (int m in mods) list.Add(Key(m, false));
            list.Add(Key(vk, false));
            list.Add(Key(vk, true));
            for (int i = mods.Length - 1; i >= 0; i--) list.Add(Key(mods[i], true));
            Native.SendInput((uint)list.Count, list.ToArray(), Marshal.SizeOf(typeof(Native.INPUT)));
        }

        private static Native.INPUT Key(int vk, bool up)
        {
            Native.INPUT i = new Native.INPUT();
            i.type = Native.INPUT_KEYBOARD;
            i.u.ki.wVk = (ushort)vk;
            i.u.ki.wScan = (ushort)Native.MapVirtualKey((uint)vk, 0);
            i.u.ki.dwFlags = up ? Native.KEYEVENTF_KEYUP : 0;
            i.u.ki.time = 0;
            i.u.ki.dwExtraInfo = IntPtr.Zero;
            return i;
        }

        // ---------- значок в трее ----------

        private void UpdateIcon(bool force)
        {
            IntPtr hkl = CurrentHkl();
            string key = hkl.ToString() + "|" + _cfg.IconStyle + "|" + (_cfg.Enabled ? "1" : "0");
            if (!force && key == _lastIconKey) return;
            _lastIconKey = key;
            _lastHkl = hkl;

            Icon old = _currentIcon;
            _currentIcon = BuildIcon(hkl);
            _tray.Icon = _currentIcon;
            if (old != null) old.Dispose();

            string text = Strings.TrayTip(LayoutName(hkl), _cfg.Enabled);
            if (text.Length > 63) text = text.Substring(0, 60) + "...";
            _tray.Text = text;
        }

        private static IntPtr CurrentHkl()
        {
            IntPtr fg = Native.GetForegroundWindow();
            uint tid = 0;
            if (fg != IntPtr.Zero)
            {
                uint pid;
                tid = Native.GetWindowThreadProcessId(fg, out pid);
            }
            return Native.GetKeyboardLayout(tid);
        }

        private Icon BuildIcon(IntPtr hkl)
        {
            const int S = 32;
            using (Bitmap bmp = new Bitmap(S, S))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Transparent);
                    g.SmoothingMode = SmoothingMode.None;
                    g.PixelOffsetMode = PixelOffsetMode.Half;

                    RectangleF flagRect = new RectangleF(1.5f, 5.5f, 29f, 21f);
                    bool drawn = false;
                    if (_cfg.IconStyle != "code")
                        drawn = Flags.Draw(g, flagRect, CountryOf(hkl));

                    if (drawn)
                    {
                        using (Pen p = new Pen(Color.FromArgb(150, 40, 40, 40), 1f))
                            g.DrawRectangle(p, flagRect.X, flagRect.Y, flagRect.Width, flagRect.Height);
                    }
                    else
                    {
                        DrawCodeBadge(g, new RectangleF(1f, 3f, 30f, 26f), CodeOf(hkl));
                    }

                    if (!_cfg.Enabled)
                    {
                        using (SolidBrush b = new SolidBrush(Color.FromArgb(160, 110, 110, 110)))
                            g.FillRectangle(b, 0, 0, S, S);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        using (Pen p = new Pen(Color.FromArgb(220, 200, 40, 40), 3f))
                            g.DrawLine(p, 4, 28, 28, 4);
                    }
                }

                IntPtr h = bmp.GetHicon();
                try
                {
                    using (Icon tmp = Icon.FromHandle(h))
                        return (Icon)tmp.Clone();
                }
                finally { Native.DestroyIcon(h); }
            }
        }

        private static void DrawCodeBadge(Graphics g, RectangleF r, string code)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (SolidBrush b = new SolidBrush(Color.FromArgb(255, 32, 68, 128)))
            using (GraphicsPath path = RoundedRect(r, 5f))
                g.FillPath(b, path);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            using (Font f = new Font("Segoe UI", 14f, FontStyle.Bold, GraphicsUnit.Pixel))
            using (SolidBrush b = new SolidBrush(Color.White))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(code, f, b, r, sf);
            }
            g.SmoothingMode = SmoothingMode.None;
        }

        private static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            float d = radius * 2f;
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        // ---------- определение языка ----------

        private static CultureInfo CultureOf(IntPtr hkl)
        {
            int langId = (int)((long)hkl & 0xFFFF);
            try { return new CultureInfo(langId); }
            catch { return null; }
        }

        private static string CodeOf(IntPtr hkl)
        {
            CultureInfo ci = CultureOf(hkl);
            if (ci == null) return "??";
            return ci.TwoLetterISOLanguageName.ToUpperInvariant();
        }

        private static string LayoutName(IntPtr hkl)
        {
            CultureInfo ci = CultureOf(hkl);
            if (ci == null) return Strings.UnknownLayout;
            return ci.NativeName;
        }

        private static readonly Dictionary<string, string> LangToCountry = BuildLangMap();

        private static Dictionary<string, string> BuildLangMap()
        {
            Dictionary<string, string> m = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            m["ru"] = "RU"; m["en"] = "US"; m["uk"] = "UA"; m["be"] = "BY"; m["kk"] = "KZ";
            m["de"] = "DE"; m["fr"] = "FR"; m["es"] = "ES"; m["it"] = "IT"; m["pl"] = "PL";
            m["tr"] = "TR"; m["he"] = "IL"; m["ar"] = "SA"; m["zh"] = "CN"; m["ja"] = "JP";
            m["ko"] = "KR"; m["pt"] = "PT"; m["nl"] = "NL"; m["sv"] = "SE"; m["fi"] = "FI";
            m["nb"] = "NO"; m["nn"] = "NO"; m["no"] = "NO"; m["da"] = "DK"; m["is"] = "IS";
            m["cs"] = "CZ"; m["sk"] = "SK"; m["hu"] = "HU"; m["bg"] = "BG"; m["ro"] = "RO";
            m["el"] = "GR"; m["lt"] = "LT"; m["lv"] = "LV"; m["et"] = "EE"; m["hy"] = "AM";
            m["az"] = "AZ"; m["ka"] = "GE"; m["uz"] = "UZ"; m["ky"] = "KG"; m["vi"] = "VN";
            m["th"] = "TH"; m["hi"] = "IN"; m["id"] = "ID"; m["sr"] = "RS"; m["hr"] = "HR";
            m["sl"] = "SI"; m["mk"] = "RS"; m["sq"] = "RS"; m["ga"] = "IE"; m["mo"] = "MD";
            return m;
        }

        private static string CountryOf(IntPtr hkl)
        {
            CultureInfo ci = CultureOf(hkl);
            if (ci == null) return null;
            try
            {
                if (!ci.IsNeutralCulture && ci.LCID != 0x007F)
                {
                    RegionInfo ri = new RegionInfo(ci.Name);
                    string c = ri.TwoLetterISORegionName;
                    // для английского США/Британии флаг берём по региону, для остальных — тоже
                    if (!string.IsNullOrEmpty(c)) return c;
                }
            }
            catch { }
            string lang = ci.TwoLetterISOLanguageName;
            string country;
            if (LangToCountry.TryGetValue(lang, out country)) return country;
            return null;
        }

        // ---------- меню ----------

        private ContextMenuStrip BuildMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.ShowImageMargin = false;

            ToolStripMenuItem enabled = new ToolStripMenuItem(Strings.MenuDisable);
            enabled.Name = "enabled";
            enabled.Click += delegate { ToggleEnabled(); };

            ToolStripMenuItem method = new ToolStripMenuItem(Strings.MenuSwitchMethod);
            method.DropDownItems.Add(MethodItem(Strings.MethodPost, "post"));
            method.DropDownItems.Add(MethodItem(Strings.MethodWinSpace, "winspace"));
            method.DropDownItems.Add(MethodItem(Strings.MethodAltShift, "altshift"));
            method.DropDownItems.Add(MethodItem(Strings.MethodCtrlShift, "ctrlshift"));
            method.Name = "method";

            ToolStripMenuItem trigger = new ToolStripMenuItem(Strings.MenuTriggerKey);
            trigger.DropDownItems.Add(TriggerItem(Strings.TriggerBoth, "both"));
            trigger.DropDownItems.Add(TriggerItem(Strings.TriggerLeft, "left"));
            trigger.DropDownItems.Add(TriggerItem(Strings.TriggerRight, "right"));
            trigger.Name = "trigger";

            ToolStripMenuItem icon = new ToolStripMenuItem(Strings.MenuIcon);
            icon.DropDownItems.Add(IconItem(Strings.IconFlag, "flag"));
            icon.DropDownItems.Add(IconItem(Strings.IconCode, "code"));
            icon.Name = "icon";

            ToolStripMenuItem convert = new ToolStripMenuItem(Strings.MenuConversion);
            convert.Name = "convert";

            ToolStripMenuItem hotkeyItem = new ToolStripMenuItem(Strings.MenuHotkey(""));
            hotkeyItem.Name = "hotkey";
            hotkeyItem.Click += delegate { EditHotkey(); };

            ToolStripMenuItem switchAfter = new ToolStripMenuItem(Strings.MenuSwitchAfter);
            switchAfter.Name = "switchafter";
            switchAfter.Click += delegate
            {
                _cfg.SwitchAfterConvert = !_cfg.SwitchAfterConvert;
                _cfg.Save();
            };

            convert.DropDownItems.Add(hotkeyItem);
            convert.DropDownItems.Add(switchAfter);

            ToolStripMenuItem autostart = new ToolStripMenuItem(Strings.MenuAutostart(false));
            autostart.Name = "autostart";
            autostart.Click += delegate
            {
                _cfg.Autostart = !Autostart.IsEnabled();
                Autostart.Set(_cfg.Autostart);
                _cfg.Save();
            };

            ToolStripMenuItem language = new ToolStripMenuItem(Strings.MenuLanguage);
            language.Name = "language";
            language.DropDownItems.Add(LanguageItem(Strings.LanguageAuto, "auto"));
            language.DropDownItems.Add(LanguageItem(Strings.LanguageRu, "ru"));
            language.DropDownItems.Add(LanguageItem(Strings.LanguageEn, "en"));

            ToolStripMenuItem settings = new ToolStripMenuItem(Strings.MenuSettingsFile);
            settings.Click += delegate
            {
                try
                {
                    Directory.CreateDirectory(Config.Dir);
                    if (!File.Exists(Config.FilePath)) _cfg.Save();
                    Process.Start("notepad.exe", Config.FilePath);
                }
                catch { }
            };

            ToolStripMenuItem about = new ToolStripMenuItem(Strings.MenuAbout);
            about.Click += delegate
            {
                using (AboutForm dlg = new AboutForm()) dlg.ShowDialog();
            };

            ToolStripMenuItem exit = new ToolStripMenuItem(Strings.MenuExit);
            exit.Click += delegate { ExitApp(); };

            menu.Items.Add(enabled);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(method);
            menu.Items.Add(trigger);
            menu.Items.Add(icon);
            menu.Items.Add(convert);
            menu.Items.Add(autostart);
            menu.Items.Add(language);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(settings);
            menu.Items.Add(about);
            menu.Items.Add(exit);

            menu.Opening += delegate
            {
                ToolStripMenuItem en = (ToolStripMenuItem)menu.Items["enabled"];
                en.Text = _cfg.Enabled ? Strings.MenuDisable : Strings.MenuEnable;

                ToolStripMenuItem conv = (ToolStripMenuItem)menu.Items["convert"];
                HotkeySpec spec = HotkeySpec.Parse(_cfg.Hotkey);
                string hk = spec == null ? Strings.HotkeyNone : spec.ToString();
                ((ToolStripMenuItem)conv.DropDownItems["hotkey"]).Text = Strings.MenuHotkey(hk);
                ((ToolStripMenuItem)conv.DropDownItems["switchafter"]).Checked = _cfg.SwitchAfterConvert;

                bool autostartOn = Autostart.IsEnabled();
                ToolStripMenuItem au = (ToolStripMenuItem)menu.Items["autostart"];
                au.Text = Strings.MenuAutostart(autostartOn);
                au.Checked = autostartOn;

                SyncRadio((ToolStripMenuItem)menu.Items["method"], _cfg.Method);
                SyncRadio((ToolStripMenuItem)menu.Items["trigger"], _cfg.Trigger);
                SyncRadio((ToolStripMenuItem)menu.Items["icon"], _cfg.IconStyle);
                SyncRadio((ToolStripMenuItem)menu.Items["language"], _cfg.Language);
            };
            return menu;
        }

        private ToolStripMenuItem LanguageItem(string text, string value)
        {
            ToolStripMenuItem it = new ToolStripMenuItem(text);
            it.Tag = value;
            it.Click += delegate
            {
                _cfg.Language = value;
                _cfg.Save();
                Strings.Apply(value);
                RebuildMenu();
                UpdateIcon(true);
            };
            return it;
        }

        /// <summary>Пересобирает меню после смены языка интерфейса.</summary>
        private void RebuildMenu()
        {
            ContextMenuStrip old = _menu;
            _menu = BuildMenu();
            _tray.ContextMenuStrip = _menu;
            if (old != null) old.Dispose();
        }

        private static void SyncRadio(ToolStripMenuItem parent, string value)
        {
            foreach (ToolStripItem it in parent.DropDownItems)
            {
                ToolStripMenuItem mi = it as ToolStripMenuItem;
                if (mi != null) mi.Checked = string.Equals((string)mi.Tag, value, StringComparison.OrdinalIgnoreCase);
            }
        }

        private ToolStripMenuItem MethodItem(string text, string value)
        {
            ToolStripMenuItem it = new ToolStripMenuItem(text);
            it.Tag = value;
            it.Click += delegate { _cfg.Method = value; _cfg.Save(); };
            return it;
        }

        private ToolStripMenuItem TriggerItem(string text, string value)
        {
            ToolStripMenuItem it = new ToolStripMenuItem(text);
            it.Tag = value;
            it.Click += delegate { _cfg.Trigger = value; _cfg.Save(); };
            return it;
        }

        private ToolStripMenuItem IconItem(string text, string value)
        {
            ToolStripMenuItem it = new ToolStripMenuItem(text);
            it.Tag = value;
            it.Click += delegate { _cfg.IconStyle = value; _cfg.Save(); UpdateIcon(true); };
            return it;
        }

        private void EditHotkey()
        {
            // снимаем регистрацию, иначе текущее сочетание не дойдёт до диалога
            if (_hotkeyRegistered)
            {
                Native.UnregisterHotKey(_pump.Handle, HotkeyId);
                _hotkeyRegistered = false;
            }

            string chosen = null;
            using (HotkeyDialog dlg = new HotkeyDialog(_cfg.Hotkey))
            {
                if (dlg.ShowDialog() == DialogResult.OK) chosen = dlg.Result;
            }

            if (chosen != null)
            {
                _cfg.Hotkey = chosen;
                _cfg.Save();
                ApplyHotkey(chosen.Length > 0);
                if (chosen.Length == 0) ShowBalloon(Strings.HotkeyOffTitle, Strings.HotkeyOffText);
            }
            else
            {
                ApplyHotkey(false);
            }
        }

        private void ToggleEnabled()
        {
            _cfg.Enabled = !_cfg.Enabled;
            _cfg.Save();
            UpdateIcon(true);
        }

        private void ShowBalloon(string title, string text)
        {
            try
            {
                _tray.BalloonTipTitle = title;
                _tray.BalloonTipText = text;
                _tray.BalloonTipIcon = ToolTipIcon.Info;
                _tray.ShowBalloonTip(5000);
            }
            catch { }
        }

        private void ExitApp()
        {
            _timer.Stop();
            if (_hotkeyRegistered)
            {
                Native.UnregisterHotKey(_pump.Handle, HotkeyId);
                _hotkeyRegistered = false;
            }
            RemoveHooks();
            _tray.Visible = false;
            _tray.Dispose();
            if (_currentIcon != null) _currentIcon.Dispose();
            _pump.Dispose();
            ExitThread();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) RemoveHooks();
            base.Dispose(disposing);
        }
    }

    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            bool silent = false;
            string command = null;
            foreach (string a in args)
            {
                string arg = a.ToLowerInvariant().TrimStart('-', '/');
                if (arg == "silent" || arg == "quiet" || arg == "s") silent = true;
                else if (arg == "install" || arg == "uninstall" || arg == "autostart:on" || arg == "autostart:off")
                    command = arg;
            }

            if (command != null)
            {
                Strings.Apply(Config.Load().Language);
                Log.Enabled = true; // установка и удаление всегда пишутся в лог: это редкие операции
                Log.Write("=== команда " + command + (silent ? " --silent" : "") + " ===");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                switch (command)
                {
                    case "install": Installer.Install(silent); return;
                    case "uninstall": Installer.Uninstall(silent); return;
                    case "autostart:on":
                        Autostart.Set(true);
                        Config on = Config.Load(); on.Autostart = true; on.Save();
                        return;
                    case "autostart:off":
                        Autostart.Set(false);
                        Config off = Config.Load(); off.Autostart = false; off.Save();
                        return;
                }
            }

            bool created;
            using (Mutex mutex = new Mutex(true, "Local\\KeyFlip_SingleInstance", out created))
            {
                if (!created) return; // уже запущено

                try { Native.SetProcessDPIAware(); }
                catch { }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TrayApp());
                GC.KeepAlive(mutex);
            }
        }
    }
}
