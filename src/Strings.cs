using System;
using System.Globalization;

namespace KeyFlip
{
    /// <summary>
    /// Все тексты интерфейса на двух языках. Чтобы добавить третий, достаточно
    /// расширить метод S и разбор значения language в настройках.
    /// </summary>
    internal static class Strings
    {
        private static bool _ru = true;

        public static bool IsRussian { get { return _ru; } }

        /// <summary>language: auto | ru | en. auto — по языку интерфейса Windows.</summary>
        public static void Apply(string language)
        {
            if (string.Equals(language, "ru", StringComparison.OrdinalIgnoreCase)) _ru = true;
            else if (string.Equals(language, "en", StringComparison.OrdinalIgnoreCase)) _ru = false;
            else _ru = IsSystemRussian();
        }

        private static bool IsSystemRussian()
        {
            try
            {
                string lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                return string.Equals(lang, "ru", StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }

        private static string S(string ru, string en) { return _ru ? ru : en; }

        // ---------- меню ----------
        public static string MenuEnable { get { return S("Включить", "Enable"); } }
        public static string MenuDisable { get { return S("Выключить", "Disable"); } }

        public static string MenuSwitchMethod { get { return S("Способ переключения", "Switching method"); } }
        public static string MethodPost { get { return S("Сообщение окну (по умолчанию)", "Window message (default)"); } }
        public static string MethodWinSpace { get { return S("Win + Пробел", "Win + Space"); } }
        public static string MethodAltShift { get { return "Alt + Shift"; } }
        public static string MethodCtrlShift { get { return "Ctrl + Shift"; } }

        public static string MenuTriggerKey { get { return S("Клавиша", "Trigger key"); } }
        public static string TriggerBoth { get { return S("Любой Ctrl", "Either Ctrl"); } }
        public static string TriggerLeft { get { return S("Только левый Ctrl", "Left Ctrl only"); } }
        public static string TriggerRight { get { return S("Только правый Ctrl", "Right Ctrl only"); } }

        public static string MenuIcon { get { return S("Значок", "Tray icon"); } }
        public static string IconFlag { get { return S("Флаг страны", "Country flag"); } }
        public static string IconCode { get { return S("Код языка", "Language code"); } }

        public static string MenuConversion { get { return S("Конвертация текста", "Text conversion"); } }
        public static string MenuHotkey(string hotkey)
        {
            return S("Сочетание клавиш: ", "Hotkey: ") + hotkey + "...";
        }
        public static string HotkeyNone { get { return S("отключено", "disabled"); } }
        public static string MenuSwitchAfter { get { return S("Переключать раскладку после конвертации", "Switch layout after conversion"); } }

        public static string MenuAutostart(bool on)
        {
            return S("Автозапуск с Windows: ", "Start with Windows: ")
                 + (on ? S("включён", "on") : S("выключен", "off"));
        }

        public static string MenuLanguage { get { return S("Язык", "Language"); } }
        public static string LanguageAuto { get { return S("Как в Windows", "Same as Windows"); } }
        public static string LanguageRu { get { return "Русский"; } }
        public static string LanguageEn { get { return "English"; } }

        public static string MenuSettingsFile { get { return S("Открыть файл настроек", "Open settings file"); } }
        public static string MenuAbout { get { return S("О программе", "About"); } }
        public static string MenuExit { get { return S("Выход", "Exit"); } }

        // ---------- уведомления ----------
        public static string StartedTitle { get { return S("KeyFlip запущен", "KeyFlip is running"); } }
        public static string StartedText(string hotkey)
        {
            return S("Ctrl — переключить раскладку. " + hotkey + " — перевести последнее слово "
                     + "или выделенный текст в другую раскладку. Автозапуск включён.",
                     "Ctrl switches the keyboard layout. " + hotkey + " converts the last word "
                     + "or the selected text to the other layout. Start with Windows is on.");
        }

        public static string HotkeyBusyTitle { get { return S("Сочетание занято", "Hotkey unavailable"); } }
        public static string HotkeyBusyText(string hotkey)
        {
            return S(hotkey + " уже используется другой программой. Выберите другое в меню → «Конвертация текста».",
                     hotkey + " is already used by another program. Pick another one in the menu → \"Text conversion\".");
        }

        public static string HotkeyDoneTitle { get { return S("Готово", "Done"); } }
        public static string HotkeyDoneText(string hotkey)
        {
            return S("Конвертация текста: " + hotkey, "Text conversion: " + hotkey);
        }

        public static string HotkeyOffTitle { get { return S("Конвертация выключена", "Conversion disabled"); } }
        public static string HotkeyOffText { get { return S("Сочетание клавиш не задано.", "No hotkey is set."); } }

        public static string OneLayoutTitle { get { return S("Только одна раскладка", "Only one layout"); } }
        public static string OneLayoutText
        {
            get
            {
                return S("Добавьте второй язык ввода в параметрах Windows, иначе переключать нечего.",
                         "Add a second input language in Windows settings — there is nothing to switch to.");
            }
        }

        public static string HookFailed(int code)
        {
            return S("Не удалось установить хук клавиатуры (код " + code + ").",
                     "Failed to install the keyboard hook (error " + code + ").");
        }

        public static string TrayTip(string layout, bool enabled)
        {
            string tip = "KeyFlip — " + layout;
            if (!enabled) tip += S(" (выключено)", " (disabled)");
            return tip;
        }

        public static string UnknownLayout { get { return S("неизвестная раскладка", "unknown layout"); } }

        // ---------- окно выбора сочетания ----------
        public static string HotkeyDialogTitle { get { return S("Горячая клавиша конвертации", "Text conversion hotkey"); } }
        public static string HotkeyDialogHint
        {
            get
            {
                return S("Нажмите нужное сочетание клавиш.\nМожно с модификаторами (Ctrl, Shift, Alt, Win) или просто F-клавишу.",
                         "Press the key combination you want.\nWith modifiers (Ctrl, Shift, Alt, Win) or just a function key.");
            }
        }
        public static string HotkeyNotSet { get { return S("(не задано)", "(not set)"); } }
        public static string ButtonDisable { get { return S("Отключить", "Disable"); } }
        public static string ButtonCancel { get { return S("Отмена", "Cancel"); } }
        public static string ButtonClose { get { return S("Закрыть", "Close"); } }

        // ---------- установка ----------
        public static string InstalledText(string dir)
        {
            return S("Установлено в:\n" + dir
                     + "\n\nЯрлык добавлен в меню «Пуск», автозапуск включён."
                     + "\nУдалить можно в «Параметры → Приложения».",
                     "Installed to:\n" + dir
                     + "\n\nA Start menu shortcut was created, start with Windows is on."
                     + "\nYou can uninstall it from Settings → Apps.");
        }

        public static string InstallFailed(string message, string logPath)
        {
            return S("Не удалось установить: " + message + "\n\nПодробности: " + logPath,
                     "Installation failed: " + message + "\n\nDetails: " + logPath);
        }

        public static string ConfirmUninstall { get { return S("Удалить KeyFlip?", "Uninstall KeyFlip?"); } }
        public static string ConfirmRemoveSettings { get { return S("Удалить также настройки?", "Remove the settings as well?"); } }
        public static string Uninstalled { get { return S("Программа удалена.", "KeyFlip has been uninstalled."); } }

        // ---------- окно «О программе» ----------
        public static string AboutTitle { get { return S("О программе", "About"); } }
        public static string AboutTagline
        {
            get
            {
                return S("Переключение раскладки и починка текста,\nнабранного не в той раскладке.",
                         "Keyboard layout switching and a fix for text\ntyped in the wrong layout.");
            }
        }
        public static string AboutCtrl { get { return S("переключить раскладку (одиночное нажатие)", "switch the layout (single press)"); } }
        public static string AboutHotkey { get { return S("перекодировать выделение или последнее слово", "convert the selection or the last word"); } }
        public static string AboutTray { get { return S("флаг языка в трее; правый клик — настройки", "language flag in the tray; right-click for settings"); } }
        public static string AboutLicense { get { return S("Свободное ПО, лицензия MIT", "Free software under the MIT license"); } }
        public static string AboutSettingsFolder { get { return S("Папка настроек", "Settings folder"); } }
        public static string AboutVersion(string version) { return S("Версия ", "Version ") + version; }
    }
}
