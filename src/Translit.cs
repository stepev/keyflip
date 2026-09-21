using System;
using System.Collections.Generic;
using System.Text;

namespace KeyFlip
{
    /// <summary>
    /// Перекодировка текста между раскладками ЙЦУКЕН и QWERTY:
    /// "ghbdtn" -> "привет", "Ghbdtn!" -> "Привет!", "ghbdtn" &lt;- "привет".
    /// </summary>
    internal static class Translit
    {
        private const string EnLower = "qwertyuiop[]asdfghjkl;'zxcvbnm,.";
        private const string RuLower = "йцукенгшщзхъфывапролджэячсмитьбю";
        private const string EnUpper = "QWERTYUIOP{}ASDFGHJKL:\"ZXCVBNM<>";
        private const string RuUpper = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";

        // Пары для символов вне буквенных рядов: en -> ru
        private static readonly string[] Extra =
        {
            "`ё", "~Ё", "/.", "?,", "@\"", "#№", "$;", "^:", "&?", "|/"
        };

        private static readonly Dictionary<char, char> EnToRu = BuildEnToRu();
        private static readonly Dictionary<char, char> RuToEn = Invert(EnToRu);

        private static Dictionary<char, char> BuildEnToRu()
        {
            Dictionary<char, char> m = new Dictionary<char, char>();
            for (int i = 0; i < EnLower.Length; i++) m[EnLower[i]] = RuLower[i];
            for (int i = 0; i < EnUpper.Length; i++) m[EnUpper[i]] = RuUpper[i];
            foreach (string pair in Extra) m[pair[0]] = pair[1];
            return m;
        }

        private static Dictionary<char, char> Invert(Dictionary<char, char> src)
        {
            Dictionary<char, char> m = new Dictionary<char, char>();
            foreach (KeyValuePair<char, char> kv in src)
                if (!m.ContainsKey(kv.Value)) m[kv.Value] = kv.Key;
            return m;
        }

        private static bool IsCyrillic(char c)
        {
            return (c >= 'а' && c <= 'я') || (c >= 'А' && c <= 'Я') || c == 'ё' || c == 'Ё';
        }

        private static bool IsLatin(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }


        /// <summary>
        /// Выделяет из набранного текста последнее слово и хвостовые пробелы после него:
        /// "hello ghbdtn  " -> word="ghbdtn", trailing="  ". false — слова нет.
        /// </summary>
        public static bool SplitLastWord(string text, out string word, out string trailing)
        {
            word = null;
            trailing = "";
            if (string.IsNullOrEmpty(text)) return false;

            int end = text.Length;
            while (end > 0 && text[end - 1] == ' ') end--;
            int start = end;
            while (start > 0 && text[start - 1] != ' ') start--;
            if (end <= start) return false;

            word = text.Substring(start, end - start);
            trailing = text.Substring(end);
            return true;
        }

        /// <summary>
        /// Перекодирует текст в противоположную раскладку. Направление выбирается
        /// по преобладающему алфавиту. resultIsCyrillic — на каком языке получился результат.
        /// Возвращает исходную строку, если в ней нет букв.
        /// </summary>
        public static string Convert(string text, out bool resultIsCyrillic)
        {
            resultIsCyrillic = false;
            if (string.IsNullOrEmpty(text)) return text;

            int cyr = 0, lat = 0;
            foreach (char c in text)
            {
                if (IsCyrillic(c)) cyr++;
                else if (IsLatin(c)) lat++;
            }
            if (cyr == 0 && lat == 0) return text;

            bool toLatin = cyr > lat;
            resultIsCyrillic = !toLatin;
            Dictionary<char, char> map = toLatin ? RuToEn : EnToRu;

            StringBuilder sb = new StringBuilder(text.Length);
            foreach (char c in text)
            {
                char mapped;
                sb.Append(map.TryGetValue(c, out mapped) ? mapped : c);
            }
            return sb.ToString();
        }
    }
}
