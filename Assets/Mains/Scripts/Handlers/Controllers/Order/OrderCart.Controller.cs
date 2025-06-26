using System.Collections.Generic;
using System.Linq;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public partial class OrderCart
    {
        private class Controller : PageController
        {
            private UID _accountID => Main.Runtime.Data.AccountID;
            private SerializableDictionary<UID, List<UID>> _cartedProducts => Main.Runtime.Data.CartedProducts;
            private SerializableDictionary<UID, uint> _orderedAmounts => Main.Runtime.OrderedAmounts;
            private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

            private OrderCart _b;

            private List<UID> _cartedProductList;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as OrderCart;

                ProductCartItemUI.OnAmountAdjusted = OnCartItemAmountAdjusted;
            }

            public override void Refresh()
            {
                _cartedProductList = _cartedProducts.TryGetValue(_accountID, out var list) ? list : new();

                _b.OnCartListDisplayed?.Invoke(_cartedProductList);
            }

            private void OnCartItemAmountAdjusted()
            {
                float totalPrice = _orderedAmounts.Sum(p => _products[p.Key].LastPrice * p.Value);

                _b.OnCartItemAmountAdjusted?.Invoke(totalPrice);
            }
        }
    }
}