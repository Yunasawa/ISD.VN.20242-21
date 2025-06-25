using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
    using PT = Product.Type;
    using PP = Product.Property;

    public static partial class Function
    {
        public static string[] SplitCSV(this string csvData)
        {
            var pattern = @"(?:^|,)(?:""([^""]*)""|([^,]*))";
            var matches = Regex.Matches(csvData, pattern);

            List<string> fields = new List<string>();

            foreach (Match match in matches)
            {
                string value = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;
                fields.Add(value);
            }

            return fields.ToArray();
        }

        public static void SerializeFeedbackDatabase(this DatabaseContainerSO database, string[] fields)
        {
            var uid = UID.Parse(fields[0]);

            var feedback = new ReviewFeedback();

            database.Feedbacks[uid] = feedback;

            feedback.CustomerID = UID.Parse(fields[1]);
            feedback.Ratings[RatingType.Status] = ReviewRating.Parse(fields[2]);
            feedback.Ratings[RatingType.Quality] = ReviewRating.Parse(fields[3]);
            feedback.Ratings[RatingType.Usability] = ReviewRating.Parse(fields[4]);
            feedback.Comment = fields[5];
        }

        public static void SerializeProductDatabase(this List<(UID, Product.Data)> products, PT type, string[] fields)
        {
            var uid = UID.Parse(fields[0]);

            var product = new Product.Data();
            products.Add((uid, product));

            product.Type = type;
            product.Title = fields[1];
            product.Creators = fields[2].Split(", ");

            if (DateTime.TryParse(fields[3], out DateTime date))
            {
                product.PublicationDate = new(date);
            }
            else
            {
                product.PublicationDate = new(DateTime.Now);
            }

            product.Price = float.Parse(fields[4].RemoveAll("$"), CultureInfo.InvariantCulture);
            product.Quantity = ushort.Parse(fields[5]);
            product.Description = fields[6];
            product.Genres = fields[7].Split(", ");
            product.SoldAmount = ushort.Parse(fields[8]);
            
            var feedbacks = fields[9]
                .Split(";", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => int.Parse(i))
                .ToDictionary(n => n + 99000000, n => new FeedbackStatus());
            foreach (var pair in feedbacks)
            {
                product.Review.Feedbacks.Add(pair.Key.ToString(), pair.Value);
            }
            
            for (int i = 10; i < fields.Length; i++)
            {
                var property = GetPropertyFromIndex(type, i);
                product.Properties[property] = fields[i];
            }

            PP GetPropertyFromIndex(PT t, int i)
            {
                return (t, i) switch
                {
                    (PT.Book, 10) => PP.Language,
                    (PT.Book, 11) => PP.NumberOfPage,
                    (PT.CD, 10) => PP.Album,
                    (PT.CD, 11) => PP.Duration,
                    (PT.DVD, 10) => PP.Studio,
                    (PT.DVD, 11) => PP.Duration,
                    (PT.LP, 10) => PP.Album,
                    (PT.LP, 11) => PP.Duration,
                    _ => PP.Language
                };
            }
        }
    }
}