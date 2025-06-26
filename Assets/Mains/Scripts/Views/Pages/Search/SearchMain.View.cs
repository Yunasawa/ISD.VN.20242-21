using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchMain
    {
        private class View : PageView
        {
            private SearchMain _b;

            private TextField _searchInput;
            private VisualElement _searchEnter;
            private VisualElement _productTypeField;
            private VisualElement _searchButton;
            private VisualElement _suggestionView;
            private Label _suggestionLabel;
            private ListView _suggestionList;
            private Label _emptyText;
            private SerializableDictionary<Product.Type, ProductTypeButtonUI> _productTypeButtons = new();

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SearchMain;

                _b.OnSuggestionDisplayed += OnSuggestionDisplayed;
                _b.OnEmptySearchingInputNotified += OnEmptySearchingInputNotified;
                _b.OnNoSearchingMatchNotified += OnNoSearchingMatchNotified;
                _b.OnEmptySuggestionDisplayed += OnEmptySuggestionDisplayed;
            }

            public override void Collect(VisualElement root)
            {
                var closeButton = root.Q("LabelField");
                closeButton.RegisterCallback<PointerUpEvent>(OnClicked_CloseButton);

                _searchInput = root.Q("SearchField").Q("SearchBar").Q("SearchInput") as TextField;
                _searchInput.RegisterValueChangedCallback(OnValueChanged_SearchInput);
                _searchInput.RegisterCallback<FocusInEvent>(OnFocusIn_SearchInput);

                _searchEnter = root.Q("SearchField").Q("CloseButton");
                _searchEnter.RegisterCallback<PointerUpEvent>(OnClicked_SearchEnter);

                _searchButton = root.Q("SearchButton");
                _searchButton.RegisterCallback<PointerUpEvent>(OnClicked_SearchButton);

                _productTypeField = root.Q("ProductType");

                _suggestionView = root.Q("SuggestionView");

                _suggestionLabel = _suggestionView.Q("Label") as Label;

                var suggestionScroll = _suggestionView.Q("SuggestionScroll") as ScrollView;
                _suggestionView.Remove(suggestionScroll);

                _suggestionList = _suggestionView.Q("SuggestionList") as ListView;

                _emptyText = _suggestionView.Q("EmptyResultLabel") as Label;

                var typeField = _productTypeField.Q("TypeField");
                foreach (Product.Type type in Enum.GetValues(typeof(Product.Type)))
                {
                    var productTypeButton = new ProductTypeButtonUI(typeField, type);
                    _productTypeButtons.Add(type, productTypeButton);
                }
            }

            public override void Begin()
            {
                _searchEnter.SetDisplay(DisplayStyle.None);
                _suggestionList.SetDisplay(DisplayStyle.None);
                _suggestionView.SetDisplay(DisplayStyle.None);

                _productTypeButtons[Product.Type.None].OnSelected_Button();

                _suggestionList.fixedItemHeight = 100;
                _suggestionList.makeItem = () => new SearchingSuggestionItemUI();
            }

            public override void Refresh()
            {
                _searchInput.SetValueWithoutNotify(string.Empty);
            }

            private void OnClicked_CloseButton(PointerUpEvent evt)
            {
                Marker.OnPageNavigated?.Invoke(ViewType.MainViewHomePage, true, false);
            }

            private void OnClicked_SearchButton(PointerUpEvent evt)
            {
                _b.OnSearchButtonClicked?.Invoke();
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
                _b.OnSearchInputChanged?.Invoke(evt.newValue);
            }

            private void OnFocusIn_SearchInput(FocusInEvent evt)
            {
                _productTypeField.SetDisplay(DisplayStyle.None);
                _searchButton.SetDisplay(DisplayStyle.None);

                _searchEnter.SetDisplay(DisplayStyle.Flex);
                _suggestionView.SetDisplay(DisplayStyle.Flex);

                evt?.StopPropagation();
            }

            private void OnSuggestionDisplayed(List<(SearchingSuggestionType type, string value)> suggestions)
            {
                _suggestionList.itemsSource = suggestions;
                _suggestionList.bindItem = (element, index) =>
                {
                    var item = element as SearchingSuggestionItemUI;
                    item.OnSelected = ApplySearchingInput;
                    if (index < suggestions.Count)
                    {
                        item.Apply(suggestions[index].type, suggestions[index].value);
                    }
                };
            }

            private void OnEmptySearchingInputNotified()
            {
                _suggestionLabel.SetText("Searching history");
                _emptyText.SetText("You haven't searched for anything yet!");
            }

            private void OnNoSearchingMatchNotified()
            {
                _suggestionLabel.SetText("Suggestions");
                _emptyText.SetText("No suggestion matching your search!");
            }

            private void OnEmptySuggestionDisplayed(bool emptyResult)
            {
                _emptyText.SetDisplay(emptyResult ? DisplayStyle.Flex : DisplayStyle.None);
                _suggestionList.SetDisplay(emptyResult ? DisplayStyle.None : DisplayStyle.Flex);
            }

            private void ApplySearchingInput(string input)
            {
                _searchInput.SetValueWithoutNotify(input);

                OnClicked_SearchEnter(null);

                _b.OnSearchInputApplied?.Invoke(input);
            }
        }
    }

    public partial class SearchMain
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
