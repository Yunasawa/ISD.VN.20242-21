using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class OrderViewCartPageUI : ViewPageUI
    {
        private UID _accountID => Main.Runtime.Data.AccountID;
        private SerializableDictionary<UID, List<UID>> _cartedProducts => Main.Runtime.Data.CartedProducts;
        private SerializableDictionary<UID, uint> _orderedAmounts => Main.Runtime.OrderedAmounts;
        private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        private Label _cartLabel;
        private ListView _cartList;
        private Label _priceText;

        private List<UID> _cartedProductList;

        protected override void Collect()
        {
            var labelField = Root.Q("LabelField");
            labelField.RegisterCallback<PointerUpEvent>(OnClicked_LabelField);

            _cartLabel = labelField.Q<Label>("LabelTitle");

            var cartScroll = Root.Q("CartScroll");
            Root.Remove(cartScroll);

            _cartList = Root.Q<ListView>("CartList");

            var orderField = Root.Q("BottomField").Q("OrderField");

            var selectAllButton = Root.Q("BottomField").Q<Button>("SelectAll");

            _priceText = orderField.Q<Label>("PriceText");

            var purchaseButton = orderField.Q<Button>("PurchaseButton");
            purchaseButton.clicked += OnClicked_PurchaseButton;
        }

        protected override void Initialize()
        {
            _cartedProductList = _cartedProducts.TryGetValue(_accountID, out var list) ? list : new();

            _cartList.Q("unity-content-container").SetFlexGrow(1);
            _cartList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _cartList.itemsSource = _cartedProductList;
            _cartList.makeItem = () => new ProductCartItemUI();
            _cartList.bindItem = (element, index) =>
            {
                var item = element as ProductCartItemUI;
                if (item != null && !_cartedProductList.IsNullOrEmpty() && Main.IsSystemStarted)
                {
                    item.Apply(_cartedProductList[index]);
                }
            };

            ProductCartItemUI.OnAmountAdjusted = OnCartItemAmountAdjusted;

            OnCartItemAmountAdjusted();
        }

        protected override void Refresh()
        {
            _cartLabel.SetText($"Cart ({_cartedProductList.Count})");
            _cartList.RebuildListView(_cartedProductList);
        }

        private void OnClicked_LabelField(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.MainViewHomePage, true, false);
        }

        private void OnClicked_PurchaseButton()
        {
            Marker.OnNewOrderRequested?.Invoke();
            Marker.OnPageNavigated?.Invoke(ViewType.OrderViewPaymentPage, true, true);
        }

        private void OnCartItemAmountAdjusted()
        {
            float totalPrice = _orderedAmounts.Sum(p => _products[p.Key].LastPrice * p.Value);

            _priceText.SetText($"Total <b><color=#DEF95D>${totalPrice.ToString("N2", CultureInfo.InvariantCulture)}</color></b>");
        }
    }
}