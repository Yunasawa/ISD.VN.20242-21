using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchViewMainPageUI
    {
        public class ProductTypeButtonUI
        {
            public static event Action<Product.Type> OnSelected;

            private VisualElement _icon;
            private Label _label;

            private bool _isSelected;
            private Product.Type _type;

            public ProductTypeButtonUI(VisualElement field, Product.Type type)
            {
                _type = type;

                var button = field.Q(type.ToString());

                _icon = button.Q("Icon");
                _label = button.Q("Label") as Label;

                button.RegisterCallback<PointerUpEvent>(OnSelected_Button);

                OnSelected += UpdateOnSelected;
            }
            ~ProductTypeButtonUI()
            {
                OnSelected -= UpdateOnSelected;
            }

            public void OnSelected_Button(PointerUpEvent evt = null)
            {
                _isSelected = true;

                UpdateUI();

                OnSelected?.Invoke(_type);
            }

            private void UpdateOnSelected(Product.Type type)
            {
                if (_type == type) return;

                _isSelected = false;

                UpdateUI();
            }

            private void UpdateUI()
            {
                _icon.SetBackgroundImageTintColor(_isSelected ? Global.HighlightColor : Color.white);
                _label.SetColor(_isSelected ? Global.HighlightColor : Color.white);
            }
        }
    }
}