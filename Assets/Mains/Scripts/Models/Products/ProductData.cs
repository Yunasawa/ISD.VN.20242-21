using System;
using YNL.Utilities.Addons;
namespace YNL.JAMOS
{
    public enum ProductType : byte
    {
        Book, CD, DVD, LP
    }

    [System.Serializable]
    public class ProductData
    {
        public UID ID;
        public ProductType Type;
        public string Title;
        public string[] Creators;
        public SerializableDateTime PublicationDate;
        public float Price;
        public string Quantity;
        public string Description;
        public string ImageURL => DataManager.ProductImageURL[Type].Replace("@", ID.ToString());
        public ProductReview Review;
    }
}