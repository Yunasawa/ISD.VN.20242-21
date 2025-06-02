using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public enum PaymentMethod : byte { CashOnDelivery, CreditCard, VISA, Paypal, VNPay }

    public partial class OrderViewPaymentPageUI : ViewPageUI
    {
        private List<UID> _cartedProducts => Main.Runtime.OrderedAmounts.Keys.ToList();

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
        private PaymentMethod _selectedMethod;

        protected override void VirtualAwake()
        {
            PaymentMethodItem.OnSelected += OnPaymentMethodSelected;
        }

        private void OnDestroy()
        {
            PaymentMethodItem.OnSelected -= OnPaymentMethodSelected;
        }

        protected override void Collect()
        {
            var labelField = Root.Q("LabelField");
            labelField.RegisterCallback<PointerUpEvent>(OnClicked_LabelField);

            var contentContainer = Root.Q("ContentScroll").Q("unity-content-container");

            var addressField = contentContainer.Q("AddressField");
            _nameText = addressField.Q("NameField").Q<Label>("Name");
            _phoneNumberText = addressField.Q("NameField").Q<Label>("Number");
            _addressText = addressField.Q<Label>("AddressText");

            var productView = contentContainer.Q("ProductView");
            productView.Remove(productView.Q("ProductScroll"));
            _productList = productView.Q<ListView>("ProductList");
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

            var orderField = Root.Q("OrderField").Q("OrderField");
            _priceText = orderField.Q<Label>("PriceText");
            _purchaseButton = orderField.Q<Button>("PurchaseButton");
        }

        protected override void Initialize()
        {
            _paymentMethodItems[PaymentMethod.CashOnDelivery].OnClicked_MethodField();
        }

        protected override void Refresh()
        {
            _productList.RebuildListView(_cartedProducts);
            UpdateDeliveryField();
        }

        private void OnClicked_LabelField(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.OrderViewCartPage, true, true);
        }

        private void OnClicked_DeliveryField(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.OrderViewDeliveryPage, true, true);
        }

        private void OnPaymentMethodSelected(PaymentMethod method)
        {
            _selectedMethod = method;
        }

        private void UpdateDeliveryField()
        {
            _deliveryTypeLabel.SetText($"{Main.Runtime.SelectedDeliveryType} Delivery");
            _deliveryPriceLabel.SetText($"<color=#909090><s>$12.39</s></color> <b>$10.39</b>");
            _deliveryTypeIcon.SetBackgroundImage(Main.Resources.Icons[$"{Main.Runtime.SelectedDeliveryType} Delivery"]);
            _deliveryNoteLabel.SetText("Guaranteed delivery from <b>May 18</b> to <b>May 19</b>.");
        }
    }
}