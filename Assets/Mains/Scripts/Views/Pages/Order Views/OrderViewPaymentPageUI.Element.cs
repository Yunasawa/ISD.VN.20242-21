using System;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class OrderViewPaymentPageUI
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