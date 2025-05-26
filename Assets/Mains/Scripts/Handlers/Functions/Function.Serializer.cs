using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
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

        public static void SerializeFeedbackDatabase(this DatabaseContainerSO _database, string[] fields)
        {
            var uid = UID.Parse(fields[0]);

            var feedback = new ReviewFeedback();

            _database.Feedbacks[uid] = feedback;

            feedback.CustomerID = UID.Parse(fields[1]);
            feedback.Ratings[RatingType.Status] = ReviewRating.Parse(fields[2]);
            feedback.Ratings[RatingType.Quality] = ReviewRating.Parse(fields[3]);
            feedback.Ratings[RatingType.Usability] = ReviewRating.Parse(fields[4]);
            feedback.Comment = fields[5];
        }
    }
}