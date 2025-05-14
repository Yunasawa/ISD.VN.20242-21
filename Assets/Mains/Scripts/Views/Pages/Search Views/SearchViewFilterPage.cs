using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;

namespace YNL.Checkotel
{
    public partial class SearchViewFilterPage : MonoBehaviour, ICollectible, IInitializable, IRefreshable
    {
        public static (int Min, int Max) PriceRange = (5, 1000);

        public HotelFacility HotelFacility;

        private VisualElement _root;
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

        private void Awake()
        {
            Marker.OnSystemStart += Collect;
        }

        private void OnDestroy()
        {
            Marker.OnSystemStart -= Collect;
        }

        public void Collect()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _filteringPage = _root.Q("FilteringPage");

            _closeButton = _filteringPage.Q("LabelField").Q("CloseButton");
            _closeButton.RegisterCallback<PointerDownEvent>(OnClicked_CloseButton);

            _resetButton = _filteringPage.Q("LabelField").Q("ResetButton") as Button;
            _resetButton.clicked += OnClicked_ResetButton;

            _applyButton = _filteringPage.Q("Toolbar").Q("ApplyButton");
            _applyButton.RegisterCallback<PointerDownEvent>(OnClicked_ApplyButton);

            var priceRangeView = _root.Q("FilterScroll").Q("PriceRangeView");

            _minLabel = priceRangeView.Q("PriceField").Q("MinPriceBox").Q("Text") as Label;

            _maxLabel = priceRangeView.Q("PriceField").Q("MaxPriceBox").Q("Text") as Label;

            _slider = priceRangeView.Q("PriceSlider") as MinMaxSlider;
            _slider.RegisterValueChangedCallback(OnValueChanged_Slider);

            _reviewScoreField = _filteringPage.Q("FilterScroll").Q("ReviewScoreField").Q("SelectionField");
            _cleanlinessField = _filteringPage.Q("FilterScroll").Q("CleanlinessField").Q("SelectionField");
            _hotelTypeField = _filteringPage.Q("FilterScroll").Q("HotelTypeField").Q("SelectionField");

            _hotelFacilitiesList = _filteringPage.Q("FilterScroll").Q("HotelFacilitiesField").Q("SelectionList");

            Initialize();
        }

        public void Initialize()
        {
            _reviewScoreField.Clear();
            _reviewScoreField.Add(new FilterPropertyButtonUI("≥ 4.5", FilterSelectionType.ReviewScore, FilterPropertyType.GE45));
            _reviewScoreField.Add(new FilterPropertyButtonUI("≥ 4.0", FilterSelectionType.ReviewScore, FilterPropertyType.GE40));
            _reviewScoreField.Add(new FilterPropertyButtonUI("≥ 3.5", FilterSelectionType.ReviewScore, FilterPropertyType.GE35));

            _cleanlinessField.Clear();
            _cleanlinessField.Add(new FilterPropertyButtonUI("5.0", FilterSelectionType.Cleanliness, FilterPropertyType.E50));
            _cleanlinessField.Add(new FilterPropertyButtonUI("≥ 4.9", FilterSelectionType.Cleanliness, FilterPropertyType.GE49));
            _cleanlinessField.Add(new FilterPropertyButtonUI("≥ 4.8", FilterSelectionType.Cleanliness, FilterPropertyType.GE48));

            _hotelTypeField.Clear();
            _hotelTypeField.Add(new FilterPropertyButtonUI("Flash sale", FilterSelectionType.HotelType, FilterPropertyType.FlashSale));
            _hotelTypeField.Add(new FilterPropertyButtonUI("Hot", FilterSelectionType.HotelType, FilterPropertyType.Hot));
            _hotelTypeField.Add(new FilterPropertyButtonUI("New", FilterSelectionType.HotelType, FilterPropertyType.New));
            _hotelTypeField.Add(new FilterPropertyButtonUI("Stamp", FilterSelectionType.HotelType, FilterPropertyType.Stamp));
            _hotelTypeField.Add(new FilterPropertyButtonUI("Discount", FilterSelectionType.HotelType, FilterPropertyType.Discount));
            _hotelTypeField.Add(new FilterPropertyButtonUI("Coupon", FilterSelectionType.HotelType, FilterPropertyType.Coupon));

            _hotelFacilitiesList.Clear();
            foreach (HotelFacility type in Enum.GetValues(typeof(HotelFacility)))
            {
                _hotelFacilitiesList.Add(new FilteringSelectionItemUI(this, type));
            }

            Refresh();
        }

        public void Refresh()
        {
            _slider.value = new(0, 1);

            (_reviewScoreField.Children().ToArray()[0] as FilterPropertyButtonUI).OnClicked__Button();
            (_cleanlinessField.Children().ToArray()[0] as FilterPropertyButtonUI).OnClicked__Button();
            (_hotelTypeField.Children().ToArray()[0] as FilterPropertyButtonUI).OnClicked__Button();
        }

        private void OnValueChanged_Slider(ChangeEvent<Vector2> evt)
        {
            (float min, float max) ratio = (evt.newValue.x, evt.newValue.y);

            int minPrice = Mathf.RoundToInt(ratio.min.Remap(new(0, 1), new(PriceRange.Min, PriceRange.Max)));
            int maxPrice = Mathf.RoundToInt(ratio.max.Remap(new(0, 1), new(PriceRange.Min, PriceRange.Max)));

            _minLabel.text = $"<b>{minPrice.ToString("N0")}$</b>";
            _maxLabel.text = maxPrice == PriceRange.Max ? $"<size=100>∞</size>" : $"<b>{maxPrice.ToString("N0")}$</b>";
        }

        private void OnClicked_CloseButton(PointerDownEvent evt)
        {
        }

        private void OnClicked_ResetButton()
        {
        }

        private void OnClicked_ApplyButton(PointerDownEvent evt)
        {
        }
    }
}