using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class OrderPayment
    {
        private class View : PageView
        {
            private List<UID> _cartedProducts => Main.Runtime.OrderedAmounts.Keys.ToList();

            private OrderPayment _b;

            private Label _nameText;
            private Label _phoneNumberText;
            private Label _addressText;
            private ListView _productList;

            private Label _deliveryTypeLabel;
            private Label _deliveryPriceLabel;
            private VisualElement _deliveryTypeIcon;
            private Label _deliveryNoteLabel;

            private Label _productPriceLabel;
            private Label _deliveryChargeLabel;
            private Label _deliveryDiscountLabel;
            private Label _totalPayment;

            private Label _priceText;
            private Button _purchaseButton;

            private SerializableDictionary<PaymentMethod, PaymentMethodItem> _paymentMethodItems = new();

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as OrderPayment;

                Marker.OnSignedInOrSignedUp += OnSignedInOrSignedUp;

                _b.OnPaymentPageRefreshed += OnPaymentPageRefreshed;
                _b.OnTotalPriceDisplayed += OnTotalPriceDisplayed;
                _b.OnDeliveryPriceDisplayed += OnDeliveryPriceDisplayed;
            }

            ~View()
            {
                Marker.OnSignedInOrSignedUp -= OnSignedInOrSignedUp;
            }

            public override void Collect(VisualElement root)
            {
                var labelField = root.Q("LabelField");
                labelField.RegisterCallback<PointerUpEvent>(OnClicked_LabelField);

                var contentContainer = root.Q("ContentScroll").Q("unity-content-container");

                var addressField = contentContainer.Q("AddressField");
                _nameText = addressField.Q("NameField").Q<Label>("Name");
                _phoneNumberText = addressField.Q("NameField").Q<Label>("Number");
                _addressText = addressField.Q<Label>("AddressText");

                var productView = contentContainer.Q("ProductView");
                productView.Remove(productView.Q("ProductScroll"));
                _productList = productView.Q<ListView>("ProductList");

                var deliveryField = contentContainer.Q("DeliveryView").Q("DeliveryField");
                deliveryField.RegisterCallback<PointerUpEvent>(OnClicked_DeliveryField);

                _deliveryTypeLabel = deliveryField.Q("PriceField").Q<Label>("TypeText");
                _deliveryPriceLabel = deliveryField.Q("PriceField").Q<Label>("PriceText");
                _deliveryTypeIcon = deliveryField.Q("NoteField").Q("Icon");
                _deliveryNoteLabel = deliveryField.Q("NoteField").Q<Label>("Note");

                var paymentField = contentContainer.Q("PaymentField").Q("MethodField");
                foreach (PaymentMethod method in Enum.GetValues(typeof(PaymentMethod)))
                {
                    var methodItem = new PaymentMethodItem(paymentField, method);
                    _paymentMethodItems.Add(method, methodItem);
                }

                var detailField = contentContainer.Q("OrderDetail").Q("DetailField");
                _productPriceLabel = detailField.Q("ProductPrice").Q<Label>("Price");
                _deliveryChargeLabel = detailField.Q("DeliveryPrice").Q<Label>("Price");
                _deliveryDiscountLabel = detailField.Q("DeliveryDiscount").Q<Label>("Price");
                _totalPayment = detailField.Q("TotalPrice").Q<Label>("Price");

                var orderField = root.Q("OrderField").Q("OrderField");
                _priceText = orderField.Q<Label>("PriceText");
                _purchaseButton = orderField.Q<Button>("OrderButton");
                _purchaseButton.RegisterCallback<PointerUpEvent>(OnClicked_PurchaseButton);
            }

            public override void Begin()
            {
                _paymentMethodItems[PaymentMethod.CashOnDelivery].OnClicked_MethodField();

                _productList.Q("unity-content-container").SetFlexGrow(1);
                _productList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
                _productList.itemsSource = _cartedProducts;
                _productList.makeItem = () => new ProductOrderItemUI();
                _productList.bindItem = (element, index) =>
                {
                    var item = element as ProductOrderItemUI;
                    if (item != null && !_cartedProducts.IsNullOrEmpty() && Main.IsSystemStarted)
                    {
                        item.Apply(_cartedProducts[index]);
                    }
                };
            }

            private void OnClicked_LabelField(PointerUpEvent evt)
            {
                Marker.OnPageNavigated?.Invoke(ViewType.OrderViewCartPage, true, true);
            }

            private void OnClicked_DeliveryField(PointerUpEvent evt)
            {
                Marker.OnPageNavigated?.Invoke(ViewType.OrderViewDeliveryPage, true, true);
            }

            private void OnClicked_PurchaseButton(PointerUpEvent evt)
            {
                _b.OnOrderPaymentRequested?.Invoke();
            }

            private void OnPaymentPageRefreshed(DeliveryType deliveryType, float charge, float totalPrice, float rawDelivery, float savedDelivery, float totalPayment)
            {
                _productList.RebuildListView(_cartedProducts);

                _deliveryTypeLabel.SetText($"{deliveryType} Delivery");
                _deliveryPriceLabel.SetText($"<b>${charge:0.00}</b>");
                _deliveryTypeIcon.SetBackgroundImage(Main.Resources.Icons[$"{deliveryType} Delivery"]);
                _deliveryNoteLabel.SetText("Guaranteed delivery from <b>May 18</b> to <b>May 19</b>.");

                _productPriceLabel.SetText($"${totalPrice.ToPriceFormat()}");

                _deliveryChargeLabel.SetText($"${rawDelivery.ToPriceFormat()}");

                _deliveryDiscountLabel.SetText($"-${savedDelivery.ToPriceFormat()}");

                _totalPayment.SetText($"${totalPayment.ToPriceFormat()}");

                _priceText.SetText($"Total <size=50><b><color=#DEF95D>${totalPayment.ToPriceFormat()}</color></b></size>\r\nSave <color=#DEF95D>${savedDelivery.ToPriceFormat()}</color>");
            }

            private void OnSignedInOrSignedUp()
            {
                var account = Main.Database.Accounts[Main.Runtime.Data.AccountID];
                _nameText.SetText(account.Name);
                _phoneNumberText.SetText(account.PhoneNumber);
                _addressText.SetText($"{account.Address.Address}, {account.Address.City}");
            }

            private void OnTotalPriceDisplayed(float price)
            {
                _priceText.SetText($"Total <size=50><b><color=#DEF95D>${price.ToPriceFormat()}</color></b></size>\r\nSave <color=#DEF95D>${0f.ToPriceFormat()}</color>");
            }

            private void OnDeliveryPriceDisplayed(float price)
            {
                _deliveryPriceLabel.SetText($"<b>${price:0.00}</b>");
            }
        }
    }

    public partial class OrderPayment
    {
        public class PaymentMethodItem
        {
            public static Action<PaymentMethod> OnSelected { get; set; }

            private VisualElement _checkBox;

            private PaymentMethod _method;
            private bool _isSelected;

            public PaymentMethodItem(VisualElement paymentField, PaymentMethod method)
            {
                _method = method;

                var methodField = paymentField.Q(method.ToString());
                methodField.RegisterCallback<PointerUpEvent>(OnClicked_MethodField);

                _checkBox = methodField.Q("Toggle");

                OnSelected += UpdateOnSelected;
            }
            ~PaymentMethodItem()
            {
                OnSelected -= UpdateOnSelected;
            }

            public void OnClicked_MethodField(PointerUpEvent evt = null)
            {
                _isSelected = true;
                UpdateFieldStyle();

                OnSelected?.Invoke(_method);
            }

            private void UpdateOnSelected(PaymentMethod method)
            {
                if (_method == method) return;

                _isSelected = false;
                UpdateFieldStyle();
            }

            private void UpdateFieldStyle()
            {
                _checkBox.SetBackgroundImage(Main.Resources.Icons[_isSelected ? "Check" : "Uncheck"]);
                _checkBox.SetBackgroundImageTintColor(_isSelected ? "#DEF95D" : "#FFFFFF");
            }
        }
    }
}
