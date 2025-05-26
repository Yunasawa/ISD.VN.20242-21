using System;
using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchViewMainPageUI : ViewPageUI
    {
        private VisualElement _closeButton;
        private TextField _searchInput;
        private VisualElement _productTypeField;
        private VisualElement _searchButton;
        private VisualElement _suggestionView;
        private Label _suggestionLabel;
        private ListView _suggestionList;
        private Label _emptyText;

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
            _searchInput.RegisterCallback<FocusOutEvent>(OnFocusOut_SearchInput);

            _searchButton = Root.Q("SearchButton");
            _searchButton.RegisterCallback<PointerUpEvent>(OnClicked_SearchButton);

            _productTypeField = Root.Q("ProductType");

            _suggestionView = Root.Q("SuggestionView");

            _suggestionLabel = _suggestionView.Q("Label") as Label;

            var suggestionScroll = _suggestionView.Q("SuggestionScroll") as ScrollView;
            _suggestionView.Remove(suggestionScroll);

            _suggestionList = _suggestionView.Q("SuggestionList") as ListView;

            _emptyText = _suggestionView.Q("EmptyResultLabel") as Label;
        }

        protected override void Initialize()
        {
            _emptyText.SetDisplay(DisplayStyle.None);
            _suggestionView.SetDisplay(DisplayStyle.None);
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

        }

        private void OnFocusIn_SearchInput(FocusInEvent evt)
        {
            _productTypeField.SetDisplay(DisplayStyle.None);
            _searchButton.SetDisplay(DisplayStyle.None);

            _suggestionView.SetDisplay(DisplayStyle.Flex);
        }

        private void OnFocusOut_SearchInput(FocusOutEvent evt)
        {
            _productTypeField.SetDisplay(DisplayStyle.Flex);
            _searchButton.SetDisplay(DisplayStyle.Flex);
        }
    }
}