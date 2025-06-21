using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public enum RatingScoreType : byte { Any, GE35, GE40, GE45 }

    public partial class SearchViewFilterPageUI : ViewPageUI
    {
        public static (int Min, int Max) PriceRange = (0, 100);

        private VisualElement _background;
        private VisualElement _filteringPage;
        private VisualElement _labelField;
        private Button _resetButton;
        private VisualElement _applyButton;
        private Label _minLabel;
        private Label _maxLabel;
        private MinMaxSlider _slider;
        private VisualElement _productGenreField;

        private Product.Type _selectedProductType;
        private Dictionary<Product.Type, ProductTypeItem> _productTypeItems = new();
        private Dictionary<RatingScoreType, RatingScoreItem> _ratingScoreItems = new();

        private List<string> _filteredGenres = new();
        private MRange _filteredPriceRange = new(0, 1000);
        private RatingScoreType _filteredRatingScore = RatingScoreType.GE45;
        private Product.Type _filteredProductType = Product.Type.None;

        protected override void VirtualAwake()
        {
            ProductTypeItem.OnSelected += OnProductTypeItemSelected;
            RatingScoreItem.OnSelected += OnRatingScoreItemSelected;
        }

        protected void OnDestroy()
        {
            ProductTypeItem.OnSelected -= OnProductTypeItemSelected;
            RatingScoreItem.OnSelected -= OnRatingScoreItemSelected;
        }

        protected override void Collect()
        {
            _background = Root.Q("ScreenBackground");
            _background.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);

            _filteringPage = Root.Q("FilteringPage");

            _labelField = _filteringPage.Q("LabelField");
            _labelField.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);

            _resetButton = _filteringPage.Q("LabelField").Q("ResetButton") as Button;
            _resetButton.clicked += OnClicked_ResetButton;

            _applyButton = _filteringPage.Q("Toolbar").Q("ApplyButton");
            _applyButton.RegisterCallback<PointerUpEvent>(OnClicked_ApplyButton);

            var container = Root.Q("FilterScroll").Q("unity-content-container");

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

        protected override void Initialize()
        {
            _slider.value = new(0, 1);
            _productTypeItems[Product.Type.None].OnClicked_TypeItem();
            _ratingScoreItems[RatingScoreType.Any].OnClicked_TypeItem();

            RecreateGenreItemUI();
        }

        protected override void Refresh()
        {
        }

        public override void OnPageOpened(bool isOpen, bool needRefresh = true)
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

            if (isOpen && needRefresh) Refresh();
        }

        private void OnValueChanged_Slider(ChangeEvent<Vector2> evt)
        {
            (float min, float max) ratio = (evt.newValue.x, evt.newValue.y);

            int minPrice = Mathf.RoundToInt(ratio.min.Remap(new(0, 1), new(PriceRange.Min, PriceRange.Max)));
            int maxPrice = Mathf.RoundToInt(ratio.max.Remap(new(0, 1), new(PriceRange.Min, PriceRange.Max)));

            _minLabel.text = minPrice == 0 ? $"<b>FREE</b>" : $"<b>${minPrice.ToString("N0")}</b>";
            _maxLabel.text = maxPrice == 0 ? $"<b>FREE</b>" : $"<b>${maxPrice.ToString("N0")}</b>";

            _filteredPriceRange.Min = minPrice;
            _filteredPriceRange.Max = maxPrice;
        }

        private void OnClicked_CloseButton(PointerUpEvent evt)
        {
            OnPageOpened(false);
        }

        private void OnClicked_ResetButton()
        {
        }

        private void OnClicked_ApplyButton(PointerUpEvent evt)
        {
            OnPageOpened(false);

            Marker.OnSearchResultFiltered?.Invoke(_filteredPriceRange, _filteredProductType, _filteredRatingScore, _filteredGenres);
        }

        private void OnProductTypeItemSelected(Product.Type type)
        {
            _filteredProductType = _selectedProductType = type;
            RecreateGenreItemUI();
        }

        private void OnRatingScoreItemSelected(RatingScoreType type)
        {
            _filteredRatingScore = type;
        }

        private void RecreateGenreItemUI()
        {
            _productGenreField.Clear();

            foreach (var genre in _selectedProductType.GetAllProductGenreString())
            {
                var item = new ProductGenreItemUI(genre, _filteredGenres);
                _productGenreField.Add(item);
            }
        }
    }
}