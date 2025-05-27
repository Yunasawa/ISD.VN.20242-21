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
            BookFormat,
            BookGenre
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
            public ProductReview Review;
            public SerializableDictionary<Property, string> Properties = new();
        }
    }
}