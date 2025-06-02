using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace YNL.JAMOS
{
    public partial class OrderViewDeliveryPageUI : ViewPageUI
    {
        private Dictionary<DeliveryType, DeliveryField> _deliveryFields = new();

        protected override void Collect()
        {
            var labelField = Root.Q("LabelField");
            labelField.RegisterCallback<PointerUpEvent>(OnClicked_LabelField);

            var contaienr = Root.Q("DeliveryContainer");

            foreach (DeliveryType type in Enum.GetValues(typeof(DeliveryType)))
            {
                var field = new DeliveryField(contaienr.Q($"{type}Delivery"), type);
                _deliveryFields.Add(type, field);
            }
        }

        protected override void Initialize()
        {
            _deliveryFields[DeliveryType.Normal].OnSelected_DeliveryField();
        }

        private void OnClicked_LabelField(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.OrderViewPaymentPage, true, true);
        }
    }
}