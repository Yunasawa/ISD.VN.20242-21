using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YNL.Checkotel
{
    public static partial class Extension
    {
        public static class Value
        {
            public static string AddSpace<T>(T input)
            {
                return Regex.Replace(input.ToString(), "(?<!^)([A-Z])", " $1");
            }

            public static string ToSentenceCase<T>(T input)
            {
                string spaced = Regex.Replace(input.ToString(), "(?<!^)([A-Z](?=[a-z])|(?<=[a-z])[A-Z])", " $1");

                string[] words = spaced.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    if (!Regex.IsMatch(words[i], @"^[A-Z]+$"))
                    {
                        words[i] = words[i].ToLower();
                    }
                }

                TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
                words[0] = textInfo.ToTitleCase(words[0]);

                return string.Join(" ", words);
            }

            public static float ToClosestPrice(float value)
            {
                float roundedValue = (float)Math.Round(value, 1);

                return roundedValue - 0.01f;
            }

            public static float AddAverage(float previousAverage, int count, float newValue)
            {
                return (previousAverage * count + newValue) / (count + 1);
            }
        }
    }
}
