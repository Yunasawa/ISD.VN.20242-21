using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;

namespace YNL.JAMOS
{
    public partial class OrderViewDeliveryPageUI : ViewPageUI
    {
        private VisualElement _contentContainer;

        private Dictionary<DeliveryType, DeliveryField> _deliveryFields = new();

        protected override void VirtualAwake()
        {
            Marker.OnDeliveryChargeCalculated += OnDeliveryChargeCalculated;
        }

        private void OnDestroy()
        {
            Marker.OnDeliveryChargeCalculated -= OnDeliveryChargeCalculated;
        }

        protected override void Collect()
        {
            var labelField = Root.Q("LabelField");
            labelField.RegisterCallback<PointerUpEvent>(OnClicked_LabelField);

            _contentContainer = Root.Q("DeliveryContainer");
        }

        protected override void Initialize()
        {
            foreach (DeliveryType type in Enum.GetValues(typeof(DeliveryType)))
            {
                var field = new DeliveryField(_contentContainer.Q($"{type}Delivery"), type);
                _deliveryFields.Add(type, field);
            }

            _deliveryFields[DeliveryType.Normal].OnSelected_DeliveryField();
        }

        protected override void Refresh()
        {
            foreach (var field in _deliveryFields)
            {
                field.Value.Refresh();
            }
        }

        private void OnClicked_LabelField(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.OrderViewPaymentPage, true, true);
        }

        private void OnDeliveryChargeCalculated(Dictionary<DeliveryType, float> deliveryCharges)
        {
            foreach (var field in _deliveryFields)
            {
                field.Value.ApplyCharge(deliveryCharges[field.Key]);
            }
        }
    }
}