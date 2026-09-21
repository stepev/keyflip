using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace KeyFlip
{
    /// <summary>
    /// Штатная установка «для текущего пользователя»: копия в %LOCALAPPDATA%\Programs,
    /// ярлык в меню «Пуск», запись в «Установленные приложения», автозапуск.
    /// Права администратора не нужны.
    /// </summary>
    internal static class Installer
    {
        public const string AppName = "KeyFlip";
        public const string DisplayName = "KeyFlip";
        public const string Version = "1.2.0";
        public const string Publisher = "KeyFlip contributors";

        private const string UninstallKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\KeyFlip";

        public static string InstallDir
        {
            get
            {
                return Path.Combine(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Programs"),
                    AppName);
            }
        }

        public static string InstalledExe { get { return Path.Combine(InstallDir, AppName + ".exe"); } }

        /// <summary>Путь к текущему exe в полной (не 8.3) форме.</summary>
        public static string CurrentExe { get { return NormalizePath(Application.ExecutablePath); } }

        public static bool IsInstalledCopy
        {
            get { return SamePath(CurrentExe, InstalledExe); }
        }

        /// <summary>
        /// Windows умеет отдавать путь в коротком виде (EVGEN~1 вместо длинного имени),
        /// поэтому перед сравнением пути раскрываются до полной формы.
        /// </summary>
        public static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            try
            {
                string full = Path.GetFullPath(path);
                StringBuilder sb = new StringBuilder(1024);
                int n = GetLongPathName(full, sb, sb.Capacity);
                if (n > 0 && n < sb.Capacity) return sb.ToString();
                return full;
            }
            catch { return path; }
        }

        public static bool SamePath(string a, string b)
        {
            return string.Equals(NormalizePath(a), NormalizePath(b), StringComparison.OrdinalIgnoreCase);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetLongPathName(string shortPath, StringBuilder longPath, int bufferSize);

        public static bool IsInstalled { get { return File.Exists(InstalledExe); } }

        public static string ShortcutPath
        {
            get
            {
                string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                return Path.Combine(programs, AppName + ".lnk");
            }
        }

        /// <summary>Устанавливает приложение и перезапускает его из установленной папки.</summary>
        public static void Install(bool silent)
        {
            try
            {
                Log.Write("установка: источник " + CurrentExe);
                Log.Write("установка: цель " + InstalledExe);

                if (!IsInstalledCopy) KillOtherInstances();
                Log.Write("установка: другие экземпляры закрыты");

                Directory.CreateDirectory(InstallDir);
                Log.Write("установка: папка создана");

                if (!IsInstalledCopy)
                    File.Copy(CurrentExe, InstalledExe, true);
                Log.Write("установка: файл скопирован");

                CreateShortcut(ShortcutPath, InstalledExe, DisplayName);
                Log.Write("установка: ярлык создан");

                WriteUninstallEntry();
                Log.Write("установка: запись в списке приложений создана");

                SetAutostart(true, InstalledExe);
                Log.Write("установка: автозапуск включён");

                Config cfg = Config.Load();
                cfg.Autostart = true;
                cfg.Save();

                if (!silent)
                {
                    MessageBox.Show(
                        "Установлено в:\n" + InstallDir +
                        "\n\nЯрлык добавлен в меню «Пуск», автозапуск включён." +
                        "\nУдалить можно в «Параметры → Приложения» или из меню в трее.",
                        DisplayName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (!IsInstalledCopy)
                {
                    // запускаем установленную копию с небольшой задержкой, чтобы текущая успела освободить mutex
                    LaunchDelayed(InstalledExe);
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Log.Write("установка: ОШИБКА " + ex);
                if (!silent)
                    MessageBox.Show("Не удалось установить: " + ex.Message
                        + "\n\nПодробности: " + Log.FilePath, DisplayName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Удаляет установленную копию, ярлык, автозапуск и запись в списке приложений.</summary>
        public static void Uninstall(bool silent)
        {
            if (!silent)
            {
                DialogResult r = MessageBox.Show(Strings.ConfirmUninstall, DisplayName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r != DialogResult.Yes) return;
            }

            bool removeSettings = false;
            if (!silent && Directory.Exists(Config.Dir))
            {
                removeSettings = MessageBox.Show(Strings.ConfirmRemoveSettings, DisplayName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            }

            SetAutostart(false, null);
            TryDelete(ShortcutPath);
            try { Registry.CurrentUser.DeleteSubKeyTree(UninstallKeyPath, false); }
            catch { }

            KillOtherInstances();

            if (removeSettings)
            {
                try { Directory.Delete(Config.Dir, true); }
                catch { }
            }

            if (IsInstalledCopy)
            {
                // себя удалить нельзя — поручаем это отдельному процессу
                DeleteDirDelayed(InstallDir);
                if (!silent)
                    MessageBox.Show(Strings.Uninstalled, DisplayName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(0);
            }
            else
            {
                try { if (Directory.Exists(InstallDir)) Directory.Delete(InstallDir, true); }
                catch { DeleteDirDelayed(InstallDir); }
                if (!silent)
                    MessageBox.Show(Strings.Uninstalled, DisplayName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public static void SetAutostart(bool enabled, string exePath)
        {
            try
            {
                using (RegistryKey k = Registry.CurrentUser.CreateSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\Run"))
                {
                    if (k == null) return;
                    if (enabled) k.SetValue(AppName, "\"" + NormalizePath(exePath ?? Application.ExecutablePath) + "\"");
                    else k.DeleteValue(AppName, false);
                }
            }
            catch { }
        }

        private static void WriteUninstallEntry()
        {
            try
            {
                using (RegistryKey k = Registry.CurrentUser.CreateSubKey(UninstallKeyPath))
                {
                    if (k == null) return;
                    long sizeKb = 0;
                    try { sizeKb = new FileInfo(InstalledExe).Length / 1024; }
                    catch { }
                    k.SetValue("DisplayName", DisplayName);
                    k.SetValue("DisplayVersion", Version);
                    k.SetValue("Publisher", Publisher);
                    k.SetValue("DisplayIcon", InstalledExe + ",0");
                    k.SetValue("InstallLocation", InstallDir);
                    k.SetValue("UninstallString", "\"" + InstalledExe + "\" --uninstall");
                    k.SetValue("QuietUninstallString", "\"" + InstalledExe + "\" --uninstall --silent");
                    k.SetValue("NoModify", 1, RegistryValueKind.DWord);
                    k.SetValue("NoRepair", 1, RegistryValueKind.DWord);
                    k.SetValue("EstimatedSize", (int)sizeKb, RegistryValueKind.DWord);
                    k.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                }
            }
            catch { }
        }

        private static void CreateShortcut(string lnkPath, string target, string description)
        {
            Type shellType = Type.GetTypeFromProgID("WScript.Shell");
            if (shellType == null) return;
            object shell = Activator.CreateInstance(shellType);
            try
            {
                object lnk = shellType.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, shell,
                    new object[] { lnkPath });
                Type lnkType = lnk.GetType();
                lnkType.InvokeMember("TargetPath", BindingFlags.SetProperty, null, lnk, new object[] { target });
                lnkType.InvokeMember("WorkingDirectory", BindingFlags.SetProperty, null, lnk,
                    new object[] { Path.GetDirectoryName(target) });
                lnkType.InvokeMember("Description", BindingFlags.SetProperty, null, lnk, new object[] { description });
                lnkType.InvokeMember("IconLocation", BindingFlags.SetProperty, null, lnk, new object[] { target + ",0" });
                lnkType.InvokeMember("Save", BindingFlags.InvokeMethod, null, lnk, null);
            }
            finally
            {
                if (shell != null && System.Runtime.InteropServices.Marshal.IsComObject(shell))
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(shell);
            }
        }

        /// <summary>
        /// Завершает только наши экземпляры — установленную копию и копию, из которой
        /// запущена установка. Совпадения по одному имени процесса недостаточно:
        /// так можно прибить чужую программу с тем же именем.
        /// </summary>
        private static void KillOtherInstances()
        {
            int me = Process.GetCurrentProcess().Id;
            string[] ours = { InstalledExe, CurrentExe };

            foreach (Process p in Process.GetProcessesByName(AppName))
            {
                try
                {
                    if (p.Id == me) continue;

                    string path = TryGetProcessPath(p);
                    if (path == null)
                    {
                        Log.Write("процесс " + p.Id + " пропущен: путь недоступен");
                        continue;
                    }

                    bool mine = false;
                    foreach (string candidate in ours)
                        if (SamePath(path, candidate)) mine = true;

                    if (!mine)
                    {
                        Log.Write("процесс " + p.Id + " пропущен: чужой путь " + path);
                        continue;
                    }

                    p.Kill();
                    p.WaitForExit(3000);
                    Log.Write("процесс " + p.Id + " завершён: " + path);
                }
                catch { }
                finally { p.Dispose(); }
            }
        }

        private static string TryGetProcessPath(Process p)
        {
            try { return p.MainModule.FileName; }
            catch { return null; }
        }

        private static void LaunchDelayed(string exe)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe",
                "/c ping -n 2 127.0.0.1 >nul & start \"\" \"" + exe + "\"");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }

        private static void DeleteDirDelayed(string dir)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe",
                "/c ping -n 3 127.0.0.1 >nul & rmdir /s /q \"" + dir + "\"");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(psi);
        }

        private static void TryDelete(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); }
            catch { }
        }
    }
}
