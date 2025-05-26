using System;
using System.Linq;
using UnityEngine;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public enum RatingType : byte { Status, Quality, Usability }

    [System.Serializable]
    public class ProductReview
    {
        public SerializableDictionary<UID, FeedbackStatus> Feedbacks = new();

        public ReviewRating AverageTotalRating => Feedbacks.Count > 0 ? Feedbacks.Sum(f => Main.Database.Feedbacks[f.Key].AverageRating) / Feedbacks.Count : -1;
        public ReviewRating AverageRating(RatingType type)
        {
            if (Feedbacks.Count <= 0) return -1;

            return Feedbacks.Sum(i => Main.Database.Feedbacks[i.Key].Ratings[type]) / Feedbacks.Count;
        }

        public int FeebackAmount => Feedbacks.Count;
    }

    [System.Serializable]
    public class FeedbackStatus
    {
        public UID FeedbackID;
        public uint Like;
    }

    [System.Serializable]
    public class ReviewFeedback
    {
        public UID CustomerID;
        public SerializableDictionary<RatingType, ReviewRating> Ratings = new();
        public string Comment = string.Empty;

        public ReviewRating AverageRating => Ratings.Sum(i => i.Value) / Ratings.Count;
    }
}
