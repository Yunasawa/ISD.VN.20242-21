using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public partial class Product
    {
        public enum Type : byte { None, Book, CD, DVD, LP }

        public enum Property : byte
        {
            Language,
            NumberOfPage,

            Album,
            Duration,

            Studio
        }

        [System.Serializable]
        public class Data
        {
            public Type Type;
            public string Title;
            public string[] Creators;
            public SerializableDateTime PublicationDate;
            public float Price;
            public ushort Quantity;
            public string Description;
            public string[] Genres;
            public string Format;
            public bool HasStreamer = false;
            public ushort SoldAmount;
            public ProductReview Review;
            public SerializableDictionary<Property, string> Properties = new();

            public bool IsFree => Price == 0;
            public float LastPrice => Price * (1 - Main.Runtime.Discount / 100f);
        }
    }
}