using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using YNL.Utilities.Addons;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class SearchViewResultPageUI : ViewPageUI
    {
        private SerializableDictionary<UID, Product.Data> _products => Main.Database.Products;
        private Product.Type _searchingProductType
        {
            get => Main.Runtime.SearchingProductType;
            set => Main.Runtime.SearchingProductType = value;
        }

        private Label _searchText;
        private VisualElement _typeIcon;
        private ListView _resultList;
        private Label _emptyText;

        private List<UID> _resultItems = new();

        protected override void VirtualAwake()
        {
            Marker.OnGenreSearchRequested += OnGenreSearchRequested;
        }

        private void OnDestroy()
        {
            Marker.OnGenreSearchRequested -= OnGenreSearchRequested;
        }

        protected override void Collect()
        {
            var resultPage = Root.Q("SearchingResultPage");

            var searchBar = resultPage.Q("SearchBar");
            searchBar.RegisterCallback<PointerUpEvent>(OnClicked_SearchBar);

            _searchText = searchBar.Q("SearchField").Q("SearchText") as Label;

            _typeIcon = searchBar.Q("SearchField").Q("TypeIcon");

            var toolBar = resultPage.Q("ToolBar");

            var sortButton = toolBar.Q("SortingButton");
            sortButton.RegisterCallback<PointerUpEvent>(OnClicked_SortButton);

            var filterButton = toolBar.Q("FilteringButton");
            filterButton.RegisterCallback<PointerUpEvent>(OnClicked_FilterButton);

            var resultScroll = resultPage.Q("ResultScroll");
            resultPage.Remove(resultScroll);

            _emptyText = resultPage.Q("EmptyText") as Label;

            _resultList = resultPage.Q("ResultList") as ListView;
            _resultList.Q("unity-content-container").SetFlexGrow(1);
            _resultList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            _resultList.itemsSource = _resultItems;
            _resultList.makeItem = () => new SearchingResultItemUI();
            _resultList.bindItem = (element, index) =>
            {
                var item = element as SearchingResultItemUI;
                if (item != null)
                {
                    item.Apply(_resultItems[index]);
                }
            };
        }

        protected override void Refresh()
        {
            _typeIcon.SetBackgroundImage(Main.Resources.Icons[Main.Runtime.SearchingProductType.ToString()]);

            var searchInput = string.IsNullOrEmpty(Main.Runtime.SearchingInput) ? "Anything" : Main.Runtime.SearchingInput;
            _searchText.SetText(searchInput);

            _resultItems.Clear();

            foreach (var product in _products)
            {
                if (_searchingProductType != Product.Type.None && _searchingProductType != product.Value.Type)
                {
                    continue;
                }

                if ((product.Value.Title.FuzzyContains(Main.Runtime.SearchingInput) ||
                    product.Value.Creators.Any(i => i.FuzzyContains(Main.Runtime.SearchingInput))))
                {
                    _resultItems.Add(product.Key);
                }
            }

            _resultList.RebuildListView(_resultItems);
        }

        private void OnClicked_SearchBar(PointerUpEvent evt)
        {
            Marker.OnViewPageSwitched?.Invoke(ViewType.SearchViewMainPage, true, false);
        }

        private void OnClicked_SortButton(PointerUpEvent evt)
        {

        }

        private void OnClicked_FilterButton(PointerUpEvent evt)
        {

        }

        private void OnGenreSearchRequested(string genre)
        {
            _typeIcon.SetBackgroundImage(Main.Resources.Icons["None"]);
            _searchText.SetText($"<color=#a0a0a0>Genre:</color> <b>{genre.AddSpaces()}</b>");

            _resultItems = _products.Where(i => i.Value.Genres.Contains(genre)).Select(i => i.Key).ToList();
            _resultList.RebuildListView(_resultItems);
        }
    }
}   