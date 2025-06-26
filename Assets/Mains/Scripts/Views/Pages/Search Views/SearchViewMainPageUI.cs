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

    public partial class SearchViewMainPageUI : PageBehaviour
    {
        private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;

        private TextField _searchInput;
        private VisualElement _searchEnter;
        private VisualElement _productTypeField;
        private VisualElement _searchButton;
        private VisualElement _suggestionView;
        private Label _suggestionLabel;
        private ListView _suggestionList;
        private Label _emptyText;
        private SerializableDictionary<Product.Type, ProductTypeButtonUI> _productTypeButtons = new();

        private List<(SearchingSuggestionType type, string value)> _suggestionValues = new();
        private string _searchingInput;
        private Product.Type _searchingProductType;

        protected override void VirtualAwake()
        {
        }

        private void OnDestroy()
        {
        }

        protected override void Construct()
        {
            var closeButton = Root.Q("LabelField");
            closeButton.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);

            _searchInput = Root.Q("SearchField").Q("SearchBar").Q("SearchInput") as TextField;
            _searchInput.RegisterValueChangedCallback(OnValueChanged_SearchInput);
            _searchInput.RegisterCallback<FocusInEvent>(OnFocusIn_SearchInput);

            _searchEnter = Root.Q("SearchField").Q("CloseButton");
            _searchEnter.RegisterCallback<PointerUpEvent>(OnClicked_SearchEnter);

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

        protected override void Begin()
        {
            _searchEnter.SetDisplay(DisplayStyle.None);
            _suggestionList.SetDisplay(DisplayStyle.None);
            _suggestionView.SetDisplay(DisplayStyle.None);

            _productTypeButtons[Product.Type.None].OnSelected_Button();
        }

        protected override void Refresh()
        {
            _searchInput.SetValueWithoutNotify(string.Empty);
        }

        private void OnClicked_CloseButton(PointerUpEvent evt)
        {
            Marker.OnPageNavigated?.Invoke(ViewType.MainViewHomePage, true, false);
        }

        private void OnClicked_SearchButton(PointerUpEvent evt)
        {
            Marker.OnSearchingInputEntered?.Invoke(_searchingInput, _searchingProductType);
            Marker.OnPageNavigated?.Invoke(ViewType.SearchViewResultPage, true, true);
        }

        private void OnClicked_SearchEnter(PointerUpEvent evt)
        {
            _productTypeField.SetDisplay(DisplayStyle.Flex);
            _searchButton.SetDisplay(DisplayStyle.Flex);

            _searchEnter.SetDisplay(DisplayStyle.None);
            _suggestionView.SetDisplay(DisplayStyle.None);

            evt?.StopPropagation();
        }

        private void OnValueChanged_SearchInput(ChangeEvent<string> evt)
        {
            var value = evt.newValue;
            _searchingInput = value;

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

                foreach (var product in _products.Values)
                {
                    if (_searchingProductType != Product.Type.None && _searchingProductType != product.Type)
                    {
                        continue;
                    }

                    if (product.Title.FuzzyContains(value))
                    {
                        _suggestionValues.Add((product.Type.ToProductType(), product.Title));
                    }

                    foreach (var creator in product.Creators.Where(c => c.FuzzyContains(value)))
                    {
                        _suggestionValues.Add((SearchingSuggestionType.Creator, creator));
                    }
                }
            }

            bool emptyResult = _suggestionValues.IsEmpty();

            _emptyText.SetDisplay(emptyResult ? DisplayStyle.Flex : DisplayStyle.None);
            _suggestionList.SetDisplay(emptyResult ? DisplayStyle.None : DisplayStyle.Flex);

            if (!emptyResult)
            {
                _suggestionValues.Sort();
                _suggestionList.RebuildListView(_suggestionValues);
            }
        }

        private void OnFocusIn_SearchInput(FocusInEvent evt)
        {
            _productTypeField.SetDisplay(DisplayStyle.None);
            _searchButton.SetDisplay(DisplayStyle.None);

            _searchEnter.SetDisplay(DisplayStyle.Flex);
            _suggestionView.SetDisplay(DisplayStyle.Flex);

            evt?.StopPropagation();
        }

        private void ApplySearchingInput(string input)
        {
            _searchInput.SetValueWithoutNotify(input);

            OnClicked_SearchEnter(null);

            _searchingInput = input;
        }

        private void OnProductTypeSelected(Product.Type type)
        {
            _searchingProductType = type;
        }
    }
}