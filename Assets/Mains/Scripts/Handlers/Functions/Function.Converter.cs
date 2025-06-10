using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace YNL.JAMOS
{
    public static partial class Function
    {
        public static string ToRank(this ReviewRating rating)
        {
            if (rating >= 0 && rating < 1) return "Disappointing";
            else if (rating >= 1 && rating < 2) return "Moderate";
            else if (rating >= 2 && rating < 3) return "Adequate";
            else if (rating >= 3 && rating < 4) return "Impressive";
            else if (rating >= 4 && rating < 5) return "Exceptional";
            return "";
        }

        public static string ToSentenceCase<T>(this T input)
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

        public static string ToDateFormat(this byte number)
        {
            if (number <= 0) return number.ToString();

            int lastDigit = number % 10;
            int lastTwoDigits = number % 100;

            string suffix = (lastTwoDigits >= 11 && lastTwoDigits <= 13) ? "th"
                          : (lastDigit == 1) ? "st"
                          : (lastDigit == 2) ? "nd"
                          : (lastDigit == 3) ? "rd"
                          : "th";

            return number + suffix;
        }

        public static string ToRankText(this ProductReview review)
        {
            var score = review.AverageTotalRating;

            if (score > 4) return "Masterpiece";
            else if (score > 3) return "Outstanding";
            else if (score > 2) return "Solid";
            else if (score > 1) return "Passable";
            else if (score > 0) return "Uninspired";
            else return "Not ranked yet";
        }

        public static SearchingSuggestionType ToProductType(this Product.Type type)
        {
            return type switch
            {
                Product.Type.Book => SearchingSuggestionType.Book,
                Product.Type.CD => SearchingSuggestionType.CD,
                Product.Type.DVD => SearchingSuggestionType.DVD,
                Product.Type.LP => SearchingSuggestionType.LP,
                _ => throw new Exception("Can not cast Product.Type to SearchingSuggestionType")
            };
        }
    
        public static string ToPriceFormat(this float price)
        {
            return price.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}