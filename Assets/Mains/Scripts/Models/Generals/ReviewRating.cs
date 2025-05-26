using System;
using UnityEngine;

namespace YNL.JAMOS
{
    [System.Serializable]
    public struct ReviewRating : IComparable<ReviewRating>
    {
        [SerializeField] private float _rating;

        public static implicit operator float(ReviewRating rating) => rating._rating;
        public static implicit operator ReviewRating(float rating) => new() { _rating = rating };

        public static ReviewRating Parse(string rating)
        {
            if (float.TryParse(rating, out float value))
            {
                return new ReviewRating { _rating = value };
            }
            throw new FormatException($"Invalid ReviewRating format: {rating}");
        }

        public int CompareTo(ReviewRating other)
        {
            return _rating.CompareTo(other._rating);
        }
        public override string ToString() => _rating == -1 ? "-" : _rating.ToString("0.0");
    }
}