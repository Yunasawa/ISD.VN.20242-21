using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchViewFilterPageUI : ViewPageUI
    {
        public static (int Min, int Max) PriceRange = (5, 1000);

        private VisualElement _background;
        private VisualElement _page;
        private VisualElement _filteringPage;
        private VisualElement _closeButton;
        private Button _resetButton;
        private VisualElement _applyButton;
        private Label _minLabel;
        private Label _maxLabel;
        private MinMaxSlider _slider;
        private VisualElement _reviewScoreField;
        private VisualElement _cleanlinessField;
        private VisualElement _hotelTypeField;
        private VisualElement _hotelFacilitiesList;

        private MRange _currentPriceRange;

        protected override void Collect()
        {
            _background = Root.Q("ScreenBackground");
            _background.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);
            _page = Root.Q("FilteringPage");

            _filteringPage = Root.Q("FilteringPage");

            _closeButton = _filteringPage.Q("LabelField");
            _closeButton.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);

            _resetButton = _filteringPage.Q("LabelField").Q("ResetButton") as Button;
            _resetButton.clicked += OnClicked_ResetButton;

            _applyButton = _filteringPage.Q("Toolbar").Q("ApplyButton");
            _applyButton.RegisterCallback<PointerUpEvent>(OnClicked_ApplyButton);

            var priceRangeView = Root.Q("FilterScroll").Q("PriceRangeView");

            _minLabel = priceRangeView.Q("PriceField").Q("MinPriceBox").Q("Text") as Label;

            _maxLabel = priceRangeView.Q("PriceField").Q("MaxPriceBox").Q("Text") as Label;

            _slider = priceRangeView.Q("PriceSlider") as MinMaxSlider;
            _slider.RegisterValueChangedCallback(OnValueChanged_Slider);

            _reviewScoreField = _filteringPage.Q("FilterScroll").Q("ReviewScoreField").Q("SelectionField");
            _cleanlinessField = _filteringPage.Q("FilterScroll").Q("CleanlinessField").Q("SelectionField");
            _hotelTypeField = _filteringPage.Q("FilterScroll").Q("HotelTypeField").Q("SelectionField");

            _hotelFacilitiesList = _filteringPage.Q("FilterScroll").Q("HotelFacilitiesField").Q("SelectionList");
        }

        protected override void Initialize()
        {
            _hotelFacilitiesList.Clear();
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
                _page.SetTranslate(0, 0, true);
            }
            else
            {
                _background.SetBackgroundColor(Color.clear);
                _background.SetPickingMode(PickingMode.Ignore);
                _page.SetTranslate(0, 100, true);
            }

            if (isOpen && needRefresh) Refresh();
        }

        private void OnValueChanged_Slider(ChangeEvent<Vector2> evt)
        {
            (float min, float max) ratio = (evt.newValue.x, evt.newValue.y);

            int minPrice = Mathf.RoundToInt(ratio.min.Remap(new(0, 1), new(PriceRange.Min, PriceRange.Max)));
            int maxPrice = Mathf.RoundToInt(ratio.max.Remap(new(0, 1), new(PriceRange.Min, PriceRange.Max)));

            _minLabel.text = $"<b>{minPrice.ToString("N0")}$</b>";
            _maxLabel.text = maxPrice == PriceRange.Max ? $"<size=100>∞</size>" : $"<b>{maxPrice.ToString("N0")}$</b>";

            _currentPriceRange.Min = minPrice;
            _currentPriceRange.Max = maxPrice;
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
        }
    }
}