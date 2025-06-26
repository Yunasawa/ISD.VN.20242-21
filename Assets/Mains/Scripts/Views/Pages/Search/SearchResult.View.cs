using System.Collections.Generic;
using UnityEngine.UIElements;
using YNL.Utilities.Extensions;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public partial class SearchResult
    {
        private class View : PageView
        {
            private SearchResult _b;

            private SearchSort _sortPage;
            private SearchFilter _filterPage;

            private Label _searchText;
            private VisualElement _typeIcon;
            private ListView _resultList;

            public override void Initialize(PageBehaviour behaviour)
            {
                _b = behaviour as SearchResult;

                _b.OnResultPageRefreshed += OnResultPageRefreshed;
                _b.OnSearchResultDisplayed += OnSearchResultDisplayed;

                Marker.OnGenreSearchRequested += OnGenreSearchRequested;
            }

            ~View()
            {
                Marker.OnGenreSearchRequested -= OnGenreSearchRequested;
            }

            public override void Collect(VisualElement root)
            {
                var resultPage = root.Q("SearchingResultPage");

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

                _resultList = resultPage.Q("ResultList") as ListView;
            }

            public override void Begin()
            {
                _resultList.Q("unity-content-container").SetFlexGrow(1);
                _resultList.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
                _resultList.makeItem = () => new SearchingResultItemUI();
            }

            public void SetPopupPage(SearchSort sort, SearchFilter filter)
            {
                _sortPage = sort;
                _filterPage = filter;
            }

            private void OnResultPageRefreshed(Product.Type type, string input)
            {
                _typeIcon.SetBackgroundImage(Main.Resources.Icons[type.ToString()]);
                _searchText.SetText(input);
            }

            private void OnClicked_SearchBar(PointerUpEvent evt)
            {
                Marker.OnPageNavigated?.Invoke(ViewType.SearchViewMainPage, true, false);
            }

            private void OnClicked_SortButton(PointerUpEvent evt)
            {
                _sortPage.OnPageOpened(true, false);
            }

            private void OnClicked_FilterButton(PointerUpEvent evt)
            {
                _filterPage.OnPageOpened(true, false);
            }

            private void OnGenreSearchRequested(string genre)
            {
                _typeIcon.SetBackgroundImage(Main.Resources.Icons["None"]);
                _searchText.SetText($"<color=#a0a0a0>Genre:</color> <b>{genre.AddSpaces()}</b>");
            }

            private void OnSearchResultDisplayed(List<UID> results)
            {
                _resultList.itemsSource = results;
                _resultList.bindItem = (element, index) =>
                {
                    var item = element as SearchingResultItemUI;
                    if (item != null)
                    {
                        item.Apply(results[index]);
                    }
                };
            }
        }
    }
}
