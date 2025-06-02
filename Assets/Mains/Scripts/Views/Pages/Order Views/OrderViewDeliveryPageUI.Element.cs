using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public enum DeliveryType : byte { Normal, Fast, Rush }

    public partial class OrderViewDeliveryPageUI
    {
        public class DeliveryField
        {
            public static Action<DeliveryType> OnSelected { get; set; }

            private VisualElement _field;
            private Label _priceLabel;
            private Label _noteLabel;
            private VisualElement _tickIcon;

            private DeliveryType _type;
            private bool _isSelected;

            public DeliveryField(VisualElement field, DeliveryType type)
            {
                _type = type;
                _field = field;
                _field.RegisterCallback<PointerUpEvent>(OnSelected_DeliveryField);

                var informationField = field.Q("InfomationField");
                _priceLabel = informationField.Q("PriceField").Q<Label>("PriceText");
                _noteLabel = informationField.Q("NoteField").Q<Label>("Note");
                _tickIcon = field.Q("Tick");

                OnSelected += UpdateOnSelected;
            }
            ~DeliveryField()
            {
                OnSelected -= UpdateOnSelected;
            }

            public void OnSelected_DeliveryField(PointerUpEvent evt = null)
            {
                _isSelected = true;
                UpdateUI();

                Main.Runtime.SelectedDeliveryType = _type;

                OnSelected?.Invoke(_type);
            }

            private void UpdateOnSelected(DeliveryType type)
            {
                if (_type == type) return;

                _isSelected = false;
                UpdateUI();
            }

            private void UpdateUI()
            {
                _tickIcon.SetBackgroundImageTintColor(_isSelected ? "#DEF95D".ToColor() : Color.clear);
            }
        }
    }
}