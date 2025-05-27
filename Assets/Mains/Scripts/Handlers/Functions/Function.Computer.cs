using System.Linq;
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
    }
}