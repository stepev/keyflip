using System;
using System.Text;

namespace KeyFlip
{
    /// <summary>
    /// Журнал набранного в текущем поле ввода: нужен, чтобы перекодировать последнее слово
    /// без обращения к буферу обмена. Логика намеренно отделена от Win32, чтобы её можно
    /// было проверить тестами.
    /// </summary>
    internal class TypedJournal
    {
        private const int MaxLength = 256;
        private readonly StringBuilder _sb = new StringBuilder();

        public string Text { get { return _sb.ToString(); } }
        public int Length { get { return _sb.Length; } }
        public void Clear() { _sb.Length = 0; }

        /// <summary>
        /// Учитывает нажатие клавиши.
        /// ch — символ, который она печатает ('\0', если не печатает или неизвестно);
        /// ctrlAltWin — зажат ли Ctrl/Alt/Win;
        /// isConvertHotkey — это нажатие и есть сочетание перекодировки (журнал нужно сохранить).
        /// </summary>
        public void Track(int vk, char ch, bool ctrlAltWin, bool isConvertHotkey)
        {
            switch (vk)
            {
                case Native.VK_SHIFT:
                case Native.VK_LSHIFT:
                case Native.VK_RSHIFT:
                case Native.VK_CONTROL:
                case Native.VK_LCONTROL:
                case Native.VK_RCONTROL:
                case Native.VK_MENU:
                case Native.VK_LMENU:
                case Native.VK_RMENU:
                case Native.VK_LWIN:
                case Native.VK_RWIN:
                case Native.VK_CAPITAL:
                    return; // модификаторы сами по себе ничего не печатают
            }

            if (ctrlAltWin)
            {
                // наше сочетание журнал сохраняет; любое другое (Ctrl+V, Ctrl+Z, Ctrl+A)
                // меняет текст непредсказуемо — дальше журналу верить нельзя
                if (!isConvertHotkey) Clear();
                return;
            }

            switch (vk)
            {
                case Native.VK_BACK:
                    if (_sb.Length > 0) _sb.Length--;
                    return;
                case Native.VK_RETURN:
                case Native.VK_TAB:
                case Native.VK_ESCAPE:
                case Native.VK_LEFT:
                case Native.VK_RIGHT:
                case Native.VK_UP:
                case Native.VK_DOWN:
                case Native.VK_HOME:
                case Native.VK_END:
                case Native.VK_PRIOR:
                case Native.VK_NEXT:
                case Native.VK_DELETE:
                case Native.VK_INSERT:
                    Clear(); // курсор уехал — журнал больше не соответствует тексту
                    return;
            }

            if (ch == '\0' || char.IsControl(ch)) { Clear(); return; }

            _sb.Append(ch);
            if (_sb.Length > MaxLength) _sb.Remove(0, _sb.Length - MaxLength);
        }

        /// <summary>После перекодировки: заменяет хвост журнала на то, что реально введено.</summary>
        public void ReplaceTail(int removed, string added)
        {
            if (removed > 0) _sb.Length = Math.Max(0, _sb.Length - removed);
            if (!string.IsNullOrEmpty(added)) _sb.Append(added);
        }
    }
}
