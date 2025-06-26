using System.Collections.Generic;
using System.Globalization;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class OrderCart
    {
        private class View : PageView
        {
            private OrderCart _b;

            private Label _cartLabel;
            private ListView _cartList;
            private Label _priceText;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as OrderCart;

                _b.OnCartListDisplayed += OnCartListDisplayed;
                _b.OnCartItemAmountAdjusted += OnCartItemAmountAdjusted;
            }

            public override void Collect(VisualElement root)
            {
                var labelField = root.Q("LabelField");
                labelField.RegisterCallback<PointerUpEvent>(OnClicked_LabelField);

                _cartLabel = labelField.Q<Label>("LabelTitle");

                var cartScroll = root.Q("CartScroll");
                root.Remove(cartScroll);

                _cartList = root.Q<ListView>("CartList");

                var orderField = root.Q("BottomField").Q("OrderField");

                var selectAllButton = root.Q("BottomField").Q<Button>("SelectAll");

                _priceText = orderField.Q<Label>("PriceText");

                var purchaseButton = orderField.Q<Button>("PurchaseButton");
                purchaseButton.clicked += OnClicked_PurchaseButton;
            }

            public override void Begin()
            {
                _cartList.Q("unity-content-container").SetFlexGrow(1);
                _cartList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
                _cartList.makeItem = () => new ProductCartItemUI();
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

            private void OnCartItemAmountAdjusted(float totalPrice)
            {
                _priceText.SetText($"Total <b><color=#DEF95D>${totalPrice.ToString("N2", CultureInfo.InvariantCulture)}</color></b>");
            }

            private void OnCartListDisplayed(List<UID> cartList)
            {
                _cartLabel.SetText($"Cart ({cartList.Count})");

                _cartList.itemsSource = cartList;
                _cartList.bindItem = (element, index) =>
                {
                    var item = element as ProductCartItemUI;
                    if (item != null && !cartList.IsNullOrEmpty() && Main.IsSystemStarted)
                    {
                        item.Apply(cartList[index]);
                    }
                };
            }
        }
    }
}
