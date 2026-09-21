using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyFlip
{
    /// <summary>Сочетание клавиш: модификаторы + клавиша (включая F1..F24 и любые другие).</summary>
    internal class HotkeySpec
    {
        public const uint MOD_ALT = 0x0001;
        public const uint MOD_CONTROL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;
        public const uint MOD_WIN = 0x0008;
        public const uint MOD_NOREPEAT = 0x4000;

        public uint Mods;
        public Keys Key;

        public HotkeySpec(uint mods, Keys key)
        {
            Mods = mods;
            Key = key;
        }

        private static readonly Dictionary<string, Keys> Aliases = BuildAliases();

        private static Dictionary<string, Keys> BuildAliases()
        {
            Dictionary<string, Keys> m = new Dictionary<string, Keys>(StringComparer.OrdinalIgnoreCase);
            m["backspace"] = Keys.Back; m["bksp"] = Keys.Back; m["bs"] = Keys.Back;
            m["esc"] = Keys.Escape;
            m["enter"] = Keys.Return; m["return"] = Keys.Return;
            m["del"] = Keys.Delete; m["ins"] = Keys.Insert;
            m["pgup"] = Keys.PageUp; m["pgdn"] = Keys.PageDown;
            m["capslock"] = Keys.CapsLock; m["scrolllock"] = Keys.Scroll; m["scroll"] = Keys.Scroll;
            m["numlock"] = Keys.NumLock; m["break"] = Keys.Pause; m["pause"] = Keys.Pause;
            m["printscreen"] = Keys.PrintScreen; m["prtsc"] = Keys.PrintScreen; m["snapshot"] = Keys.PrintScreen;
            m["apps"] = Keys.Apps; m["menu"] = Keys.Apps; m["contextmenu"] = Keys.Apps;
            m["space"] = Keys.Space; m["tab"] = Keys.Tab;
            m["`"] = Keys.Oemtilde; m["tilde"] = Keys.Oemtilde;
            m["-"] = Keys.OemMinus; m["="] = Keys.Oemplus;
            m["["] = Keys.OemOpenBrackets; m["]"] = Keys.OemCloseBrackets;
            m[";"] = Keys.OemSemicolon; m["'"] = Keys.OemQuotes;
            m[","] = Keys.Oemcomma; m["."] = Keys.OemPeriod;
            m["/"] = Keys.OemQuestion; m["\\"] = Keys.OemPipe;
            return m;
        }

        private static readonly Dictionary<Keys, string> Names = BuildNames();

        private static Dictionary<Keys, string> BuildNames()
        {
            Dictionary<Keys, string> m = new Dictionary<Keys, string>();
            m[Keys.Back] = "Backspace"; m[Keys.Escape] = "Esc"; m[Keys.Return] = "Enter";
            m[Keys.Next] = "PgDn"; m[Keys.PageUp] = "PgUp"; m[Keys.Scroll] = "ScrollLock";
            m[Keys.Pause] = "Pause"; m[Keys.Apps] = "Menu"; m[Keys.PrintScreen] = "PrintScreen";
            m[Keys.Oemtilde] = "`"; m[Keys.OemMinus] = "-"; m[Keys.Oemplus] = "=";
            m[Keys.OemOpenBrackets] = "["; m[Keys.OemCloseBrackets] = "]";
            m[Keys.OemSemicolon] = ";"; m[Keys.OemQuotes] = "'";
            m[Keys.Oemcomma] = ","; m[Keys.OemPeriod] = "."; m[Keys.OemQuestion] = "/";
            m[Keys.OemPipe] = "\\";
            return m;
        }

        public static bool IsModifierKey(Keys k)
        {
            return k == Keys.ControlKey || k == Keys.ShiftKey || k == Keys.Menu
                || k == Keys.LWin || k == Keys.RWin
                || k == Keys.LControlKey || k == Keys.RControlKey
                || k == Keys.LShiftKey || k == Keys.RShiftKey
                || k == Keys.LMenu || k == Keys.RMenu;
        }

        /// <summary>Разбирает строку вида "Ctrl+Shift+Backspace", "F12", "Win+Alt+Q". null — если пусто/не разобрано.</summary>
        public static HotkeySpec Parse(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            s = s.Trim();
            if (s.Length == 0 || s.Equals("none", StringComparison.OrdinalIgnoreCase)
                || s.Equals("нет", StringComparison.OrdinalIgnoreCase)) return null;

            uint mods = 0;
            Keys key = Keys.None;
            foreach (string rawPart in s.Split('+'))
            {
                string p = rawPart.Trim();
                if (p.Length == 0) continue;
                string lower = p.ToLowerInvariant();
                if (lower == "ctrl" || lower == "control") { mods |= MOD_CONTROL; continue; }
                if (lower == "shift") { mods |= MOD_SHIFT; continue; }
                if (lower == "alt") { mods |= MOD_ALT; continue; }
                if (lower == "win" || lower == "windows" || lower == "meta") { mods |= MOD_WIN; continue; }

                Keys parsed;
                if (Aliases.TryGetValue(p, out parsed)) { key = parsed; continue; }
                if (p.Length == 1 && p[0] >= '0' && p[0] <= '9') { key = (Keys)((int)Keys.D0 + (p[0] - '0')); continue; }
                if (p.Length == 1 && char.IsLetter(p[0])) { key = (Keys)char.ToUpperInvariant(p[0]); continue; }
                if (Enum.TryParse(p, true, out parsed) && parsed != Keys.None) { key = parsed; continue; }
                return null;
            }
            if (key == Keys.None || IsModifierKey(key)) return null;
            return new HotkeySpec(mods, key);
        }

        public static string Format(uint mods, Keys key)
        {
            StringBuilder sb = new StringBuilder();
            if ((mods & MOD_CONTROL) != 0) sb.Append("Ctrl+");
            if ((mods & MOD_ALT) != 0) sb.Append("Alt+");
            if ((mods & MOD_SHIFT) != 0) sb.Append("Shift+");
            if ((mods & MOD_WIN) != 0) sb.Append("Win+");
            if (key != Keys.None) sb.Append(KeyName(key));
            return sb.ToString();
        }

        public static string KeyName(Keys key)
        {
            string name;
            if (Names.TryGetValue(key, out name)) return name;
            if (key >= Keys.D0 && key <= Keys.D9) return ((char)('0' + (key - Keys.D0))).ToString();
            if (key >= Keys.A && key <= Keys.Z) return key.ToString();
            return key.ToString();
        }

        public override string ToString()
        {
            return Format(Mods, Key);
        }
    }

    /// <summary>Окно «нажмите сочетание клавиш».</summary>
    internal class HotkeyDialog : Form
    {
        private readonly Label _preview;
        private uint _mods;
        private Keys _key = Keys.None;

        /// <summary>Итог: строка сочетания, "" — отключить, null — отмена.</summary>
        public string Result { get; private set; }

        public HotkeyDialog(string current)
        {
            Text = Strings.HotkeyDialogTitle;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            KeyPreview = true;
            ClientSize = new Size(380, 160);
            Font = new Font("Segoe UI", 9f);
            TopMost = true;

            Label hint = new Label();
            hint.Text = "Нажмите нужное сочетание клавиш.\nМожно с модификаторами (Ctrl, Shift, Alt, Win) или просто F-клавишу.";
            hint.SetBounds(14, 12, 352, 40);

            _preview = new Label();
            _preview.TextAlign = ContentAlignment.MiddleCenter;
            _preview.BorderStyle = BorderStyle.FixedSingle;
            _preview.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            _preview.SetBounds(14, 56, 352, 40);
            _preview.Text = string.IsNullOrEmpty(current) ? Strings.HotkeyNotSet : current;

            HotkeySpec cur = HotkeySpec.Parse(current);
            if (cur != null) { _mods = cur.Mods; _key = cur.Key; }

            Button ok = new Button();
            ok.Text = "OK";
            ok.SetBounds(120, 110, 80, 28);
            ok.Click += delegate
            {
                if (_key == Keys.None) { Result = ""; }
                else { Result = HotkeySpec.Format(_mods, _key); }
                DialogResult = DialogResult.OK;
                Close();
            };

            Button off = new Button();
            off.Text = Strings.ButtonDisable;
            off.SetBounds(14, 110, 100, 28);
            off.Click += delegate
            {
                Result = "";
                DialogResult = DialogResult.OK;
                Close();
            };

            Button cancel = new Button();
            cancel.Text = Strings.ButtonCancel;
            cancel.SetBounds(286, 110, 80, 28);
            cancel.Click += delegate
            {
                Result = null;
                DialogResult = DialogResult.Cancel;
                Close();
            };

            Controls.Add(hint);
            Controls.Add(_preview);
            Controls.Add(ok);
            Controls.Add(off);
            Controls.Add(cancel);

            KeyDown += OnKey;
        }

        private void OnKey(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;

            uint mods = 0;
            if (e.Control) mods |= HotkeySpec.MOD_CONTROL;
            if (e.Shift) mods |= HotkeySpec.MOD_SHIFT;
            if (e.Alt) mods |= HotkeySpec.MOD_ALT;
            if (Native.IsDown(Native.VK_LWIN) || Native.IsDown(Native.VK_RWIN)) mods |= HotkeySpec.MOD_WIN;

            if (HotkeySpec.IsModifierKey(e.KeyCode))
            {
                _preview.Text = mods == 0 ? Strings.HotkeyNotSet : HotkeySpec.Format(mods, Keys.None) + "...";
                return;
            }

            _mods = mods;
            _key = e.KeyCode;
            _preview.Text = HotkeySpec.Format(_mods, _key);
        }
    }
}
