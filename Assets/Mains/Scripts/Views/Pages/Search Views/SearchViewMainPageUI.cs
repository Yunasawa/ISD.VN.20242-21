using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public enum SearchingSuggestionType : byte { Creator, Book, CD, DVD, LP }

    public partial class SearchViewMainPageUI : ViewPageUI
    {
        private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        private VisualElement _closeButton;
        private TextField _searchInput;
        private VisualElement _productTypeField;
        private VisualElement _searchButton;
        private VisualElement _suggestionView;
        private Label _suggestionLabel;
        private ListView _suggestionList;
        private Label _emptyText;
        private SerializableDictionary<Product.Type, ProductTypeButtonUI> _productTypeButtons = new();

        private List<(SearchingSuggestionType type, string value)> _suggestionValues = new();

        protected override void VirtualAwake()
        {
        }

        private void OnDestroy()
        {
        }

        protected override void Collect()
        {
            _closeButton = Root.Q("LabelField");
            _closeButton.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);

            _searchInput = Root.Q("SearchField").Q("SearchInput") as TextField;
            _searchInput.RegisterValueChangedCallback(OnValueChanged_SearchInput);
            _searchInput.RegisterCallback<FocusInEvent>(OnFocusIn_SearchInput);

            _searchButton = Root.Q("SearchButton");
            _searchButton.RegisterCallback<PointerUpEvent>(OnClicked_SearchButton);

            _productTypeField = Root.Q("ProductType");

            _suggestionView = Root.Q("SuggestionView");

            _suggestionLabel = _suggestionView.Q("Label") as Label;

            var suggestionScroll = _suggestionView.Q("SuggestionScroll") as ScrollView;
            _suggestionView.Remove(suggestionScroll);

            _suggestionList = _suggestionView.Q("SuggestionList") as ListView;
            _suggestionList.fixedItemHeight = 100;
            _suggestionList.itemsSource = _suggestionValues;
            _suggestionList.makeItem = () => new SearchingSuggestionItemUI();
            _suggestionList.bindItem = (element, index) =>
            {
                var item = element as SearchingSuggestionItemUI;
                item.OnSelected = ApplySearchingInput;
                if (index < _suggestionValues.Count)
                {
                    item.Apply(_suggestionValues[index].type, _suggestionValues[index].value);
                }
            };

            _emptyText = _suggestionView.Q("EmptyResultLabel") as Label;
            
            var typeField = _productTypeField.Q("TypeField");
            foreach (Product.Type type in Enum.GetValues(typeof(Product.Type)))
            {
                var productTypeButton = new ProductTypeButtonUI(typeField, type);
                _productTypeButtons.Add(type, productTypeButton);
            }

            ProductTypeButtonUI.OnSelected += OnProductTypeSelected;
        }

        protected override void Initialize()
        {
            _suggestionList.SetDisplay(DisplayStyle.None);
            _suggestionView.SetDisplay(DisplayStyle.None);

            _productTypeButtons[Product.Type.None].OnSelected_Button();
        }

        private void OnClicked_CloseButton(PointerUpEvent evt)
        {
            Marker.OnViewPageSwitched?.Invoke(ViewType.MainViewHomePage, true, true);
        }

        private void OnClicked_SearchButton(PointerUpEvent evt)
        {
            Marker.OnViewPageSwitched?.Invoke(ViewType.SearchViewResultPage, true, true);
        }

        private void OnValueChanged_SearchInput(ChangeEvent<string> evt)
        {
            var value = evt.newValue;
            _suggestionValues.Clear();

            if (value == string.Empty)
            {
                _suggestionLabel.SetText("Searching history");
                _suggestionValues = Main.Runtime.Data.SearchingHistory;
                _emptyText.SetText("You haven't searched for anything yet!");
            }
            else
            {
                _suggestionLabel.SetText("Suggestions");
                _emptyText.SetText("No suggestion matching your search!");

                foreach (var pair in _products)
                {
                    var product = pair.Value;

                    if (product.Title.FuzzyContains(value))
                    {
                        _suggestionValues.Add((product.Type.ToProductType(), product.Title));
                    }
                    foreach (var creator in product.Creators)
                    {
                        if (creator.FuzzyContains(value))
                        {
                            _suggestionValues.Add((SearchingSuggestionType.Creator, creator));
                        }
                    }
                }
            }

            bool emptyResult = _suggestionValues.IsEmpty();

            _emptyText.SetDisplay(emptyResult ? DisplayStyle.Flex : DisplayStyle.None);
            _suggestionList.SetDisplay(emptyResult ? DisplayStyle.None : DisplayStyle.Flex);

            if (!emptyResult)
            {
                _suggestionList.RebuildListView(_suggestionValues);
            }
        }

        private void OnFocusIn_SearchInput(FocusInEvent evt)
        {
            _productTypeField.SetDisplay(DisplayStyle.None);
            _searchButton.SetDisplay(DisplayStyle.None);

            _suggestionView.SetDisplay(DisplayStyle.Flex);
        }

        private void ApplySearchingInput(string input)
        {
            _searchInput.SetValueWithoutNotify(input);

            _productTypeField.SetDisplay(DisplayStyle.Flex);
            _searchButton.SetDisplay(DisplayStyle.Flex);

            Main.Runtime.SearchingInput = input;
        }

        private void OnProductTypeSelected(Product.Type type)
        {
            Main.Runtime.SearchingProductType = type;
        }
    }
}