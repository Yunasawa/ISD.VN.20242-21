using System.Collections.Generic;

namespace YNL.Checkotel
{
    public class HotelReview
    {
        public ReviewRating AverageRating
        {
            get
            {
                ReviewRating rating = 0;

                foreach (var feedback in Feedbacks)
                {
                    rating += feedback.Rating;
                }

                if (Feedbacks.Count > 0)
                {
                    return rating / Feedbacks.Count;
                }

                return -1;
            }
        }

        public List<ReviewFeedback> Feedbacks { get; set; } = new();
    }

    public struct ReviewRating
    {
        private float _rating;

        public static implicit operator float(ReviewRating rating) => rating._rating;
        public static implicit operator ReviewRating(float rating) => new() { _rating = rating };
    }

    public class ReviewFeedback
    {
        public UID CustomerID { get; set; }
        public ReviewRating Rating { get; set; } = 0;
        public string Comment { get; set; } = string.Empty;
    }
}
