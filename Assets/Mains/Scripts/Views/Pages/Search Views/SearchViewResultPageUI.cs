using UnityEngine.UIElements;
using YNL.Utilities.UIToolkits;

namespace YNL.JAMOS
{
    public class SearchViewResultPageUI : ViewPageUI
    {
        private Label _searchText;
        private VisualElement _typeIcon;
        private ListView _resultList;
        private Label _emptyText;

        protected override void VirtualAwake()
        {

        }

        private void OnDestroy()
        {

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
        }

        protected override void Refresh()
        {
            _typeIcon.SetBackgroundImage(Main.Resources.Icons[Main.Runtime.SearchingProductType.ToString()]);

            var searchInput = string.IsNullOrEmpty(Main.Runtime.SearchingInput) ? "Anything" : Main.Runtime.SearchingInput;
            _searchText.SetText(searchInput);
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
    }
}   