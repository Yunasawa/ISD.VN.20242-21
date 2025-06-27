using System.Linq;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public partial class ManagerProduct
    {
        private class Controller : PageController
        {
            private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

            private ManagerProduct _b;

            private UID[] _productIDs;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as ManagerProduct;
            }

            public override void Refresh()
            {
                _productIDs = _products.Keys.ToArray();

                _b.OnProductListDisplayed?.Invoke(_productIDs);
            }
        }
    }
}