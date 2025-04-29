using System.Collections.Concurrent;

namespace MediaStore
{
    public class MediaUnit
    {
        public MediaType Type { get; set; }
        public MediaInformation Information { get; set; } = new();
        public MediaReview Review { get; set; } = new();
        public MediaPrice Price { get; set; } = new();
    }

    public class MediaInformation
    {
        public string Title { get; set; } = string.Empty;
        public ConcurrentDictionary<MediaProperty, string> Properties { get; set; } = new();
    }

    public class MediaPrice
    {
        public float BasePrice { get; set; } = 0;
        public float Discount { get; set; } = 0;
        public float Tax { get; set; } = 0;

        public float FinalPrice
        {
            get
            {
                var discountedPrice = BasePrice * (1 - Discount / 100);
                var taxAmount = discountedPrice * (Tax / 100);
                return discountedPrice + taxAmount;
            }
        }
    }

    public class MediaReview
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

    public class ReviewFeedback
    {
        public UID CustomerID { get; set; }
        public ReviewRating Rating { get; set; } = 0;
        public string Comment { get; set; } = string.Empty;
    }

    public struct ReviewRating
    {
        private float _rating;

        public static implicit operator float(ReviewRating rating) => rating._rating;
        public static implicit operator ReviewRating(float rating) => new() { _rating = rating };
    }

    public enum MediaType
    {
        CD,
        DVD,
        Book
    }

    public enum MediaProperty
    {
        
    }
}
