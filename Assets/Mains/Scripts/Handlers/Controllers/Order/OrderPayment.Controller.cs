using System;
using System.Collections.Generic;
using System.Linq;
using YNL.Utilities.Addons;

namespace YNL.JAMOS
{
    public partial class OrderPayment
    {
        private class Controller : PageController
        {
            private SerializableDictionary<UID, uint> _orderedAmounts => Main.Runtime.OrderedAmounts;
            private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

            private OrderPayment _b;

            private PaymentMethod _selectedMethod;
            private DeliveryType _selectedDeliveryType = DeliveryType.Normal;
            private float _totalPrice;
            private Dictionary<DeliveryType, float> _deliveryCharges = new();

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as OrderPayment;

                _b.OnOrderPaymentRequested += OnOrderPaymentRequested;
                PaymentMethodItem.OnSelected += OnPaymentMethodSelected;
                Marker.OnDeliveryTypeSelected += OnDeliveryFieldSelected;
                Marker.OnNewOrderRequested += OnNewOrderRequested;
                Marker.OnSignedInOrSignedUp += OnSignedInOrSignedUp;
            }

            ~Controller()
            {
                PaymentMethodItem.OnSelected -= OnPaymentMethodSelected;
                Marker.OnDeliveryTypeSelected -= OnDeliveryFieldSelected;
                Marker.OnNewOrderRequested -= OnNewOrderRequested;
                Marker.OnSignedInOrSignedUp -= OnSignedInOrSignedUp;
            }

            public override void Begin()
            {
                foreach (DeliveryType type in Enum.GetValues(typeof(DeliveryType)))
                {
                    _deliveryCharges[type] = 0;
                }
            }

            public override void Refresh()
            {
                OnPaymentPageRefreshed();
            }

            private void OnPaymentMethodSelected(PaymentMethod method)
            {
                _selectedMethod = method;
            }

            private void OnDeliveryFieldSelected(DeliveryType type)
            {
                _selectedDeliveryType = type;
            }

            private void OnNewOrderRequested()
            {
                foreach (DeliveryType type in Enum.GetValues(typeof(DeliveryType)))
                {
                    _deliveryCharges[type] = type.GetAverageCharge();
                }
                Marker.OnDeliveryChargeCalculated?.Invoke(_deliveryCharges);

                _b.OnDeliveryPriceDisplayed?.Invoke(_deliveryCharges[_selectedDeliveryType]);
            }

            private void OnPaymentPageRefreshed()
            {
                var charge = _deliveryCharges[_selectedDeliveryType];
                float totalPrice = _orderedAmounts.Sum(p => _products[p.Key].LastPrice * p.Value);
                var rawDelivery = _selectedDeliveryType.GetAverageCharge();

                float deliveryDiscount = new[] { 0, 10, 20, 50, 100 }[new System.Random().Next(5)];
                float totalDelivery = rawDelivery * (1 - deliveryDiscount / 100f);
                float savedDelivery = rawDelivery - totalDelivery;

                _totalPrice = totalPrice + totalDelivery;

                _b.OnPaymentPageRefreshed?.Invoke(_selectedDeliveryType, charge, totalPrice, rawDelivery, savedDelivery, _totalPrice);
            }

            private void OnOrderPaymentRequested()
            {
                Marker.OnPageNavigated?.Invoke(ViewType.OrderViewResultPage, true, true);

                var code = Function.GetOrderCode();
                Main.Runtime.Data.Orders.Add(code, new OrderItem().Initialize());
                Marker.OnOrderCodeCreated?.Invoke(code);

                Marker.OnRuntimeSavingRequested?.Invoke();

                var vndPrice = _totalPrice.ToVND();
                if (_selectedMethod == PaymentMethod.VNPay)
                {
                    Marker.OnVNPayPaymentRequested?.Invoke(code, vndPrice);
                }

                Main.Runtime.OrderedAmounts.Clear();
                _totalPrice = 0;

                _b.OnTotalPriceDisplayed?.Invoke(_totalPrice);
            }

            private void OnSignedInOrSignedUp()
            {
                _orderedAmounts.Clear();
            }
        }
    }
}
