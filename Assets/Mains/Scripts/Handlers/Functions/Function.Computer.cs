using System;
using System.Linq;
using System.Text.RegularExpressions;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
    public static partial class Function
    {
        private static SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        public static UID[] GetNewProductsList()
        {
            return _products.Select(p => p.Key).ToArray();
        }

        public static string GetImageURL(this UID id)
        {
            var type = _products[id].Type;
            var url = DataManager.ProductImageURL.Replace("{Type}", type.ToString()).Replace("{ID}", id.ToString());
            return url;
        }

        public static string GetStreamURL(this UID id)
        {
            var type = _products[id].Type;
            var url = DataManager.ProductStreamURL
                .Replace("{Type}", type.ToString())
                .Replace("{ID}", id.ToString())
                .Replace("{Format}", id.GetStreamFormat());
            return url;
        }

        public static string GetDurationText(this UID id)
        {
            var product = Main.Database.Products[id];

            return product.Type switch
            {
                Product.Type.Book => $"{product.Properties[Product.Property.NumberOfPage]} pages",
                Product.Type.CD => $"{product.Properties[Product.Property.Duration]}",
                _ => throw new Exception("Invalid Product Property for duration text")
            };
        }

        public static string GetDescriptionText(this UID id)
        {
            var product = Main.Database.Products[id];

            return product.Description.Replace("#", "\r\n\r\n");
        }
    
        public static string GetStreamFormat(this UID id)
        {
            var product = Main.Database.Products[id];

            return product.Type switch
            {
                Product.Type.Book => "txt",
                Product.Type.CD => "mp3",
                Product.Type.DVD => "mp4",
                Product.Type.LP => "mp3",
                _ => ""
            };
        }
    }
}