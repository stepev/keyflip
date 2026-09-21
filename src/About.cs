using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace KeyFlip
{
    /// <summary>Окно «О программе»: что делает, версия, лицензия, ссылка на исходники.</summary>
    internal class AboutForm : Form
    {
        public const string ProjectUrl = "https://github.com/stepev/keyflip";

        public AboutForm()
        {
            Text = Strings.AboutTitle;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(440, 306);
            Font = new Font("Segoe UI", 9f);
            BackColor = Color.White;

            try
            {
                string exeIcon = Application.ExecutablePath;
                if (File.Exists(exeIcon)) Icon = Icon.ExtractAssociatedIcon(exeIcon);
            }
            catch { }

            PictureBox logo = new PictureBox();
            logo.SetBounds(20, 20, 48, 48);
            logo.SizeMode = PictureBoxSizeMode.Zoom;
            try { if (Icon != null) logo.Image = Icon.ToBitmap(); }
            catch { }

            Label title = new Label();
            title.Text = "KeyFlip";
            title.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            title.SetBounds(80, 18, 200, 30);

            Label version = new Label();
            version.Text = Strings.AboutVersion(Installer.Version);
            version.ForeColor = Color.FromArgb(110, 110, 110);
            version.SetBounds(82, 48, 200, 18);

            Label tagline = new Label();
            tagline.Text = Strings.AboutTagline;
            tagline.SetBounds(20, 80, 400, 36);

            Label keys = new Label();
            keys.SetBounds(20, 126, 400, 84);
            keys.Text = "Ctrl  —  " + Strings.AboutCtrl + Environment.NewLine
                      + CurrentHotkey() + "  —  " + Strings.AboutHotkey + Environment.NewLine
                      + (Strings.IsRussian ? "Значок" : "Tray icon") + "  —  " + Strings.AboutTray;

            Label license = new Label();
            license.Text = Strings.AboutLicense + "  ·  © 2026 KeyFlip contributors";
            license.ForeColor = Color.FromArgb(110, 110, 110);
            license.SetBounds(20, 218, 400, 18);

            LinkLabel link = new LinkLabel();
            link.Text = ProjectUrl;
            link.SetBounds(20, 238, 400, 18);
            link.LinkClicked += delegate { Open(ProjectUrl); };

            Button close = new Button();
            close.Text = Strings.ButtonClose;
            close.SetBounds(340, 264, 80, 28);
            close.Click += delegate { Close(); };

            AcceptButton = close;
            CancelButton = close;

            Controls.Add(logo);
            Controls.Add(title);
            Controls.Add(version);
            Controls.Add(tagline);
            Controls.Add(keys);
            Controls.Add(license);
            Controls.Add(link);
            Controls.Add(close);
        }

        private static string CurrentHotkey()
        {
            HotkeySpec spec = HotkeySpec.Parse(Config.Load().Hotkey);
            return spec == null ? Strings.HotkeyNone : spec.ToString();
        }

        private static void Open(string target)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(target);
                psi.UseShellExecute = true;
                Process.Start(psi);
            }
            catch { }
        }
    }
}
