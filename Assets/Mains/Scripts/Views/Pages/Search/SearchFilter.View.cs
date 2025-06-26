using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchFilter
    {
        private class View : PageView
        {
            private SearchFilter _b;

            private VisualElement _background;
            private VisualElement _filteringPage;
            private VisualElement _labelField;
            private Button _resetButton;
            private VisualElement _applyButton;
            private Label _minLabel;
            private Label _maxLabel;
            private MinMaxSlider _slider;
            private VisualElement _productGenreField;

            private Dictionary<Product.Type, ProductTypeItem> _productTypeItems = new();
            private Dictionary<RatingScoreType, RatingScoreItem> _ratingScoreItems = new();

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SearchFilter;

                _b.OnViewOpenRequested += OnViewOpenRequested;
                _b.OnGenreListDisplayed += OnGenreListDisplayed;
                _b.OnPriceRangeDisplayed += OnPriceRangeDisplayed;
            }

            public override void Collect(VisualElement root)
            {
                _background = root.Q("ScreenBackground");
                _background.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);

                _filteringPage = root.Q("FilteringPage");

                _labelField = _filteringPage.Q("LabelField");
                _labelField.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);

                _resetButton = _filteringPage.Q("LabelField").Q("ResetButton") as Button;
                _resetButton.clicked += OnClicked_ResetButton;

                _applyButton = _filteringPage.Q("Toolbar").Q("ApplyButton");
                _applyButton.RegisterCallback<PointerUpEvent>(OnClicked_ApplyButton);

                var container = root.Q("FilterScroll").Q("unity-content-container");

                var priceRangeView = container.Q("PriceRangeView");

                _minLabel = priceRangeView.Q("PriceField").Q("MinPriceBox").Q("Text") as Label;

                _maxLabel = priceRangeView.Q("PriceField").Q("MaxPriceBox").Q("Text") as Label;

                _slider = priceRangeView.Q("PriceSlider") as MinMaxSlider;
                _slider.RegisterValueChangedCallback(OnValueChanged_Slider);

                var productTypeField = container.Q("ProductTypeField").Q("MediaType");
                foreach (Product.Type type in Enum.GetValues(typeof(Product.Type)))
                {
                    var field = productTypeField.Q(type.ToString());
                    var item = new ProductTypeItem(field, type);
                    _productTypeItems[type] = item;
                }

                var ratingScoreField = container.Q("ReviewScoreField").Q("SelectionField");
                foreach (RatingScoreType type in Enum.GetValues(typeof(RatingScoreType)))
                {
                    var field = ratingScoreField.Q(type.ToString());
                    var item = new RatingScoreItem(field, type);
                    _ratingScoreItems[type] = item;
                }

                _productGenreField = container.Q("ProductGenreField").Q("SelectionList");
            }

            public override void Begin()
            {
                _slider.value = new(0, 1);
                _productTypeItems[Product.Type.None].OnClicked_TypeItem();
                _ratingScoreItems[RatingScoreType.Any].OnClicked_TypeItem();
            }

            private void OnViewOpenRequested(bool isOpen)
            {
                if (isOpen)
                {
                    _background.SetPickingMode(PickingMode.Position);
                    _background.SetBackgroundColor(new Color(0.0865f, 0.0865f, 0.0865f, 0.725f));
                    _filteringPage.SetTranslate(0, 0, true);
                }
                else
                {
                    _background.SetBackgroundColor(Color.clear);
                    _background.SetPickingMode(PickingMode.Ignore);
                    _filteringPage.SetTranslate(0, 100, true);
                }
            }

            private void OnValueChanged_Slider(ChangeEvent<Vector2> evt)
            {
                _b.OnPriceRangeChanged?.Invoke(evt.newValue);
            }

            private void OnClicked_CloseButton(PointerUpEvent evt)
            {
                _b.OnPageOpened(false);
            }

            private void OnClicked_ResetButton()
            {
            }

            private void OnClicked_ApplyButton(PointerUpEvent evt)
            {
                _b.OnPageOpened(false);

                _b.OnApplyButtonClicked?.Invoke();
            }

            private void OnGenreListDisplayed(string[] genreString, List<string> filteredGenres)
            {
                _productGenreField.Clear();

                foreach (var genre in genreString)
                {
                    var item = new ProductGenreItemUI(genre, filteredGenres);
                    _productGenreField.Add(item);
                }
            }

            private void OnPriceRangeDisplayed(int minPrice, int maxPrice)
            {
                _minLabel.text = minPrice == 0 ? $"<b>FREE</b>" : $"<b>${minPrice.ToString("N0")}</b>";
                _maxLabel.text = maxPrice == 0 ? $"<b>FREE</b>" : $"<b>${maxPrice.ToString("N0")}</b>";
            }
        }
    }

    public partial class SearchFilter
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
