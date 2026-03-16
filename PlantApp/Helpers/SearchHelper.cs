using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PlantApp.Helpers
{
    public static class SearchHelper
    {
        // нормализую строку перед поиском
        private static string Normalize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = text.ToLower();

            // убираю скобки
            text = text.Replace("(", "").Replace(")", "");

            // убираю пробелы
            text = text.Replace(" ", "");

            return text;
        }

        public static List<string> GetBigrams(string text)
        {
            // сначала нормализую текст
            text = Normalize(text);

            var grams = new List<string>();

            // разбиваю слово на биграммы
            for (int i = 0; i < text.Length - 1; i++)
            {
                grams.Add(text.Substring(i, 2));
            }

            return grams;
        }

        public static double Compare(string source, string target)
        {
            var sourceGrams = GetBigrams(source);
            var targetGrams = GetBigrams(target);

            if (sourceGrams.Count == 0 || targetGrams.Count == 0)
                return 0;

            int matches = sourceGrams.Intersect(targetGrams).Count();

            // считаю коэффициент похожести
            return (double)matches / Math.Max(sourceGrams.Count, targetGrams.Count);
        }
    }
}
