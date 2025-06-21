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
            public Type Type = Type.None;
            public string Title = string.Empty;
            public string[] Creators = new string[0];
            public SerializableDateTime PublicationDate;
            public float Price = 0;
            public ushort Quantity = 0;
            public string Description = string.Empty;
            public string[] Genres = new string[0];
            public string Format = string.Empty;
            public bool HasStreamer = false;
            public ushort SoldAmount = 0;
            public ProductReview Review = new();
            public SerializableDictionary<Property, string> Properties = new();

            public bool IsFree => Price == 0;
            public float LastPrice => Price * (1 - Main.Runtime.Discount / 100f);
        }
    }
}