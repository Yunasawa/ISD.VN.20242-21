using System.Collections.Generic;
using System.Globalization;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class OrderViewCartPageUI : ViewPageUI
    {
        private List<UID> _cartedProducts => Main.Runtime.Data.CartedProducts;
        
        private Label _cartLabel;
        private ListView _cartList;
        private Label _priceText;

        protected override void Collect()
        {
            var labelField = Root.Q("LabelField");
            labelField.RegisterCallback<PointerUpEvent>(OnClicked_LabelField);

            _cartLabel = labelField.Q<Label>("LabelTitle");

            var cartScroll = Root.Q("CartScroll");
            Root.Remove(cartScroll);

            _cartList = Root.Q<ListView>("CartList");
            _cartList.Q("unity-content-container").SetFlexGrow(1);
            _cartList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _cartList.itemsSource = _cartedProducts;
            _cartList.makeItem = () => new ProductCartItemUI();
            _cartList.bindItem = (element, index) =>
            {
                var item = element as ProductCartItemUI;
                if (item != null && !_cartedProducts.IsNullOrEmpty() && Main.IsSystemStarted)
                {
                    item.Apply(_cartedProducts[index]);
                }
            };

            var orderField = Root.Q("BottomField").Q("OrderField");

            var selectAllButton = Root.Q("BottomField").Q<Button>("SelectAll");

            _priceText = orderField.Q<Label>("PriceText");

            var purchaseButton = orderField.Q<Button>("PurchaseButton");
            purchaseButton.clicked += OnClicked_PurchaseButton;
        }

        protected override void Initialize()
        {
            ProductCartItemUI.OnAmountAdjusted = OnCartItemAmountAdjusted;

            OnCartItemAmountAdjusted();
        }

        protected override void Refresh()
        {
            _cartLabel.SetText($"Cart ({_cartedProducts.Count})");
            _cartList.RebuildListView(_cartedProducts);
        }

        private void OnClicked_LabelField(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.MainViewHomePage, true, false);
        }

        private void OnClicked_PurchaseButton()
        {
            Marker.OnPageNavigated?.Invoke(ViewType.OrderViewPaymentPage, true, true);
        }

        private void OnCartItemAmountAdjusted()
        {
            float totalPrice = 0;

            foreach (var pair in Main.Runtime.OrderedAmounts)
            {
                var product = Main.Database.Products[pair.Key];
                totalPrice += product.LastPrice * pair.Value;
            }

            _priceText.SetText($"Total <b><color=#DEF95D>${totalPrice.ToString("N2", CultureInfo.InvariantCulture)}</color></b>");
        }
    }
}