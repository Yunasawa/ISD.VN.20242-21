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
            private bool _validDelivery = true;
            private bool _isSelected;
            private float _deliveryCharge;

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
                Marker.OnSignedInOrSignedUp += OnSignedInOrSignedUp;
            }
            ~DeliveryField()
            {
                OnSelected -= UpdateOnSelected;
                Marker.OnSignedInOrSignedUp += OnSignedInOrSignedUp;
            }

            public void Refresh()
            {
                if (_validDelivery)
                {
                    _priceLabel.SetText($"<b>${_deliveryCharge:0.00}</b>");

                    var fromTime = DateTime.Now.ToString("MMMM dd");
                    var toTime = DateTime.Now.AddDays(_type.GetDeliveryTime()).ToString("MMMM dd");
                    _noteLabel.SetText($"Guaranteed delivery from <b>{fromTime}</b> to <b>{toTime}</b>.");
                }
            }

            public void ApplyCharge(float charge)
            {
                _deliveryCharge = charge;
            }

            public void OnSelected_DeliveryField(PointerUpEvent evt = null)
            {
                if (_validDelivery == false) return;

                _isSelected = true;
                UpdateUI();

                OnSelected?.Invoke(_type);

                Marker.OnDeliveryTypeSelected?.Invoke(_type);
            }

            private void OnSignedInOrSignedUp()
            {
                if (_type != DeliveryType.Rush) return;

                var account = Main.Database.Accounts[Main.Runtime.Data.AccountID];
                if (account.Address.City != "Ha Noi")
                {
                    _validDelivery = false;

                    _field.SetEnabled(false);
                    _priceLabel.SetText(string.Empty);
                    _noteLabel.SetText("<b>Rush Delivery</b> is only available within <b>Ha Noi</b>");
                }

                Refresh();
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