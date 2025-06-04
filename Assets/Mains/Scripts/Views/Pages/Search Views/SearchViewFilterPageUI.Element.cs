using System;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchViewFilterPageUI
    { 
        public class ProductTypeItem
        {
            public static Action<Product.Type> OnSelected { get; set; }

            private Label _label;
            private VisualElement _icon;

            private Product.Type _type;
            private bool _isSelected;

            public ProductTypeItem(VisualElement field, Product.Type type)
            {
                _type = type;

                field.RegisterCallback<PointerUpEvent>(OnClicked_TypeItem);

                _label = field.Q<Label>("Label");
                _icon = field.Q("Icon");

                OnSelected += UpdateOnSelected;
            }
            ~ProductTypeItem()
            {
                OnSelected -= UpdateOnSelected;
            }

            public void OnClicked_TypeItem(PointerUpEvent evt = null)
            {
                _isSelected = true;
                UpdateUI();

                OnSelected?.Invoke(_type);

                //Main.Runtime.SelectedProductType = _type;
            }

            private void UpdateOnSelected(Product.Type type)
            {
                if (_type == type) return;

                _isSelected = false;
                UpdateUI();
            }

            private void UpdateUI()
            {
                _label.SetColor(_isSelected ? "#DEF95D".ToColor() : Color.white);
                _icon.SetBackgroundImageTintColor(_isSelected ? "#DEF95D".ToColor() : Color.white);
            }
        }

        public class RatingScoreItem
        {
            public static Action<RatingScoreType> OnSelected { get; set; }

            private VisualElement _field;
            private Label _label;

            private RatingScoreType _type;
            private bool _isSelected;

            public RatingScoreItem(VisualElement field, RatingScoreType type)
            {
                _type = type;

                _field = field;
                _field.RegisterCallback<PointerUpEvent>(OnClicked_TypeItem);

                _label = field.Q<Label>("Text");

                OnSelected += UpdateOnSelected;
            }
            ~RatingScoreItem()
            {
                OnSelected -= UpdateOnSelected;
            }

            public void OnClicked_TypeItem(PointerUpEvent evt = null)
            {
                _isSelected = true;
                UpdateUI();

                OnSelected?.Invoke(_type);

                //Main.Runtime.SelectedRatingScoreType = _type;
            }

            private void UpdateOnSelected(RatingScoreType type)
            {
                if (_type == type) return;

                _isSelected = false;
                UpdateUI();
            }

            private void UpdateUI()
            {
                _field.SetBorderColor(_isSelected ? "#DEF95D".ToColor() : Color.clear);
                _field.SetBackgroundColor(_isSelected ? "#3B3E2E" : "#404040");
                _label.SetColor(_isSelected ? "#DEF95D".ToColor() : Color.white);
            }
        }
    }
}